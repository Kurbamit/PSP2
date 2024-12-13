using Azure;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Exceptions.GiftCardExceptions;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Exceptions.OrderExceptions;
using ReactApp1.Server.Exceptions.StorageExceptions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IFullOrderRepository _fullOrderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IGiftCardRepository _giftcardRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IPaymentService _paymentService;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, 
            IFullOrderRepository fullOrderRepository, IEmployeeRepository employeeRepository, ILogger<OrderService> logger,
            IPaymentRepository paymentRepository, IGiftCardRepository giftcardRepository, IPaymentService paymentService)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _fullOrderRepository = fullOrderRepository;
            _employeeRepository = employeeRepository;
            _paymentRepository = paymentRepository;
            _giftcardRepository = giftcardRepository;
            _logger = logger;
            _paymentService = paymentService;
        }
        
        public async Task<OrderItemsPayments> OpenOrder(int? createdByEmployeeId, int? establishmentId)
        {
            if (!createdByEmployeeId.HasValue || !establishmentId.HasValue)
            {
                _logger.LogError("Failed to open order: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            var emptyOrder = await _orderRepository.AddEmptyOrderAsync(createdByEmployeeId.Value, establishmentId.Value);

            return new OrderItemsPayments(emptyOrder, null, null);
        }
        
        public async Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);
            foreach (var order in orders.Items)
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(order.CreatedByEmployeeId);
                if(employee != null)
                    order.CreatedByEmployeeName = employee.FirstName + " " + employee.LastName;
            }
            
            return orders;
        }

        public async Task<OrderItemsPayments> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogInformation($"Order with id: {orderId} not found");
                return new OrderItemsPayments(null, null, null);
            }
            
            var employee = await _employeeRepository.GetEmployeeByIdAsync(order.CreatedByEmployeeId);
            if(employee != null)
                order.CreatedByEmployeeName = employee.FirstName + " " + employee.LastName;

            var orderItems = await GetOrderItems(orderId);

            var orderWithTotalPrice = CalculateTotalPriceForOrder(order, orderItems);

            var orderPayments = await GetOrderPayments(orderId);

            var orderWithTotalPaidAndLeftToPay = CalculateTotalPaidAndLeftToPayForOrder(orderWithTotalPrice, orderPayments);

            return new OrderItemsPayments(orderWithTotalPaidAndLeftToPay, orderItems, orderPayments);
        }
        
        private async Task<List<ItemModel>> GetOrderItems(int orderId)
        {
            var fullOrders = await _fullOrderRepository.GetOrderItemsAsync(orderId);
            
            var orderItems = new List<ItemModel>();
            foreach (var fullOrder in fullOrders)
            {
                var item = await _itemRepository.GetItemByIdFromFullOrderAsync(fullOrder.ItemId, orderId);
                
                if(item == null)
                    continue;
                
                item.Count = fullOrder.Count;
                orderItems.Add(item);
            }

            return orderItems;
        }
        private async Task<List<PaymentModel>> GetOrderPayments(int orderId)
        {
            var payments = await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
            return payments;
        }

        private OrderModel CalculateTotalPriceForOrder(OrderModel order, List<ItemModel> orderItems)
        {
            decimal totalPrice = 0;
            foreach (var item in orderItems)
            {
                decimal itemCost = item.Cost ?? 0;
                decimal itemCount = item.Count ?? 0;

                
                totalPrice += itemCost * itemCount;
            }
            
            // TODO: Apply discount and taxes to the total price
            
            order.TotalPrice = totalPrice;

            return order;
        }
        private OrderModel CalculateTotalPaidAndLeftToPayForOrder(OrderModel order, List<PaymentModel> orderPayments)
        {
            decimal totalPaid = orderPayments.Sum(payment => payment.Value);

            order.TotalPaid = totalPaid;
            order.LeftToPay = order.TotalPrice - order.TotalPaid;

            return order;
        }

        public async Task AddItemToOrder(FullOrderModel fullOrder, int? userId)
        {
            if (!userId.HasValue)
            {
                _logger.LogError("Failed to add item to order: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            // Before adding an item to an order, check if:
            // 1. The order exists
            // 2. The item exists and there is enough stock in storage
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(fullOrder.OrderId, (int)OrderStatusEnum.Open, "AddItemToOrder");
            if (existingOrderWithOpenStatus == null) 
                return;

            var itemIsAvailableInStorage = await ItemIsAvailableInStorage();
            if (!itemIsAvailableInStorage)
                return;
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);
            
            // Reduce reserved item count in storage
            await _itemRepository.AddStorageAsync(fullOrder.ItemId, -fullOrder.Count);
            
            var task = existingFullOrder != null
                // If the item is already in the order (fullOrder record which links the order with the item exists in the database)
                // update its quantity by adding new count to existing count
                ? _fullOrderRepository.UpdateItemInOrderCountAsync(fullOrder)
                // Otherwise, create a new record for it
                : _fullOrderRepository.AddItemToOrderAsync(fullOrder, userId.Value);
            
            await task;

            async Task<bool> ItemIsAvailableInStorage()
            {
                var storage = await _itemRepository.GetItemStorageAsync(fullOrder.ItemId);
                if (storage == null)
                {
                    _logger.LogError($"Failed to add item {fullOrder.ItemId} to order {fullOrder.OrderId}: Item not found in storage");
                    throw new ItemNotFoundException(fullOrder.ItemId);
                }
                
                if (storage.Count < fullOrder.Count)
                {
                    _logger.LogError($"Not enough stock for item {fullOrder.ItemId}. Requested: {fullOrder.Count}, Available: {storage.Count} for order {fullOrder.OrderId}");
                    throw new StockExhaustedException(fullOrder.ItemId, storage.Count);
                }

                return true;
            }
        }
        
        public async Task RemoveItemFromOrder(FullOrderModel fullOrder)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(fullOrder.OrderId, (int)OrderStatusEnum.Open, "RemoveItemFromOrder");
            if (existingOrderWithOpenStatus == null)
                return;
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);
            if (existingFullOrder == null)
            {
                _logger.LogError($"The specified item {fullOrder.ItemId} is not linked to the given order {fullOrder.OrderId}");
                throw new ItemNotFoundInOrderException(fullOrder.ItemId, fullOrder.OrderId);
            }
            
            // Ensure we don't return more items to storage than were originally reserved, this check handles that
            var itemsToRemoveCount =  Math.Min(fullOrder.Count, existingFullOrder.Count);
            
            await _itemRepository.AddStorageAsync(fullOrder.ItemId, itemsToRemoveCount);
            
            if (fullOrder.Count < existingFullOrder.Count)
            {
                // If the count of item to remove is smaller, update the quantity, but don't remove the item from the order
                fullOrder.Count = -fullOrder.Count;
                _logger.LogInformation($"Updating item count {fullOrder.Count}");
                await _fullOrderRepository.UpdateItemInOrderCountAsync(fullOrder);
                return;

            }
            // Otherwise, delete the item from the order
            await  _fullOrderRepository.DeleteItemFromOrderAsync(fullOrder);
        }
        
        public async Task UpdateOrder(OrderModel order)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(order.OrderId, (int)OrderStatusEnum.Open, "UpdateOrder");
            if (existingOrderWithOpenStatus == null)
                throw new OrderNotFoundException(order.OrderId);
            
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task CloseOrder(int orderId)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(orderId, (int)OrderStatusEnum.Open, "CloseOrder");
            if(existingOrderWithOpenStatus == null)
                return;

            existingOrderWithOpenStatus.Status = (int)OrderStatusEnum.Closed;
            
            await _orderRepository.UpdateOrderAsync(existingOrderWithOpenStatus);
        }
        
        private async Task<OrderModel?> GetOrderIfExistsAndStatusIs(int orderId, int orderStatus, string? operation = null)
        {
            var order = (await GetOrderById(orderId)).Order;
            if (order == null)
            {
                _logger.LogError($"Operation '{operation}' failed: Order {orderId} not found");
                throw new OrderNotFoundException(orderId);
            }

            if (order.Status != orderStatus)
            {
                _logger.LogError($"Operation '{operation}' failed: Order status is {orderStatus}");
                throw new OrderStatusConflictException(orderStatus.ToString());
            }

            return order;
        }
        public async Task CancelOrder(int orderId)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(orderId, (int)OrderStatusEnum.Open, "CancelOrder");
            if (existingOrderWithOpenStatus == null)
                return;

            existingOrderWithOpenStatus.Status = (int)OrderStatusEnum.Cancelled;

            //Add items back to storage

            var orderItems = await GetOrderItems(orderId);
            foreach (var item in orderItems)
            {
                int itemCount = item.Count ?? 0;
                await _itemRepository.AddStorageAsync(item.ItemId, itemCount);
            }

            // TODO refund if payments made

            await _orderRepository.UpdateOrderAsync(existingOrderWithOpenStatus);
        }
        public async Task PayOrder(PaymentModel payment)
        {
            var existingOrderWithClosedStatus = await GetOrderIfExistsAndStatusIs(payment.OrderId, (int)OrderStatusEnum.Closed, "PayOrder");
            if (existingOrderWithClosedStatus == null)
                return;

            if (payment.Type == (int)PaymentTypeEnum.GiftCard)
            {
                GiftCardModel? giftcard = await _giftcardRepository.GetGiftCardByCodeAsync(payment.GiftCardCode);
                if (giftcard == null || giftcard.ExpirationDate < DateTime.Now)
                {
                    throw new GiftcardInvalidException(payment.GiftCardCode);
                }

                if (giftcard.Amount < payment.Value)
                {
                    throw new GiftcardNotEnoughFundsException(giftcard.Amount, payment.Value);
                }

                giftcard.Amount -= payment.Value;
                payment.GiftCardId = giftcard.GiftCardId;

                await _giftcardRepository.UpdateGiftCardAsync(giftcard);

            } 
            else if (payment.Type == (int)PaymentTypeEnum.Card)
            {
               
            }

            if (existingOrderWithClosedStatus.LeftToPay == payment.Value)
            {
                existingOrderWithClosedStatus.Status = (int)OrderStatusEnum.Completed;
                await _orderRepository.UpdateOrderAsync(existingOrderWithClosedStatus);
            }

            await _paymentRepository.AddPaymentAsync(payment);
        }
        public async Task RefundOrder(int orderId)
        {
            var order = await GetOrderIfExistsAndStatusIs(orderId, (int)OrderStatusEnum.Completed, "RefundOrder");
            if (order == null || order.Refunded)
                return;

            var orderPayments = await GetOrderPayments(orderId);

            foreach (var payment in orderPayments)
            {
                if(payment.Type == (int)PaymentTypeEnum.Cash)
                {
                    // idk?
                }
                else if(payment.Type == (int)PaymentTypeEnum.GiftCard)
                {
                    GiftCardModel? giftcard = await _giftcardRepository.GetGiftCardByIdAsync(payment.GiftCardId);

                    if(giftcard == null)
                    {
                        _logger.LogError($"Could not refund payment {payment.PaymentId}. Giftcard not found.");
                        continue;
                    }

                    giftcard.Amount += payment.Value;

                    //Add 1 extra week to spend the money
                    if(giftcard.ExpirationDate < DateTime.Now)
                    {
                        giftcard.ExpirationDate = DateTime.Now.AddDays(7);
                    }
                    else 
                    {
                        giftcard.ExpirationDate = giftcard.ExpirationDate.AddDays(7);
                    }

                    await _giftcardRepository.UpdateGiftCardAsync(giftcard);
                }
                else if(payment.Type == (int)PaymentTypeEnum.Card)
                {
                    await _paymentService.RefundPaymentIntent(payment.StripePaymentId);
                }
            }
            
            order.Refunded = true;
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<byte[]> DownloadReceipt(int orderId)
        {
            var order = await GetOrderById(orderId);
            if (order.Order == null)
            {
                _logger.LogError($"Failed to download receipt: Order {orderId} not found");
                throw new OrderNotFoundException(orderId);
            }

            if (order.Order.Status != (int)OrderStatusEnum.Completed)
            {
                throw new OrderStatusConflictException(((OrderStatusEnum)order.Order.Status).ToString());
            }

            return await _orderRepository.DownloadReceipt(order);
        }
    }
}

