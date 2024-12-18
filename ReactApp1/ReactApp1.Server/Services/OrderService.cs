using System.Security.Principal;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Exceptions.GiftCardExceptions;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Exceptions.OrderExceptions;
using ReactApp1.Server.Exceptions.StorageExceptions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
using Stripe;

namespace ReactApp1.Server.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IFullOrderRepository _fullOrderRepository;
        private readonly IFullOrderServiceRepository _fullOrderServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IGiftCardRepository _giftcardRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IPaymentService _paymentService;
        private readonly IDiscountRepository _discountRepository;
        private readonly ITaxService _taxService;
        private readonly IFullOrderTaxRepository _fullOrderTaxRepository;
        private readonly IFullOrderServiceTaxRepository _fullOrderServiceTaxRepository;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, IServiceRepository serviceRepository,
            IFullOrderRepository fullOrderRepository, IFullOrderServiceRepository fullOrderServiceRepository, IEmployeeRepository employeeRepository,
            ILogger<OrderService> logger, IPaymentRepository paymentRepository, IGiftCardRepository giftcardRepository, IPaymentService paymentService,
            IDiscountRepository discountRepository, ITaxService taxService, IFullOrderTaxRepository fullOrderTaxRepository,
            IFullOrderServiceTaxRepository fullOrderServiceTaxRepository)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _serviceRepository = serviceRepository;
            _fullOrderRepository = fullOrderRepository;
            _fullOrderServiceRepository = fullOrderServiceRepository;
            _employeeRepository = employeeRepository;
            _paymentRepository = paymentRepository;
            _giftcardRepository = giftcardRepository;
            _logger = logger;
            _paymentService = paymentService;
            _discountRepository = discountRepository;
            _taxService = taxService;
            _fullOrderTaxRepository = fullOrderTaxRepository;
            _fullOrderServiceTaxRepository = fullOrderServiceTaxRepository;
        }
        
        public async Task<OrderItemsPayments> OpenOrder(int? createdByEmployeeId, int? establishmentId)
        {
            if (!createdByEmployeeId.HasValue || !establishmentId.HasValue)
            {
                _logger.LogError("Failed to open order: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            var emptyOrder = await _orderRepository.AddEmptyOrderAsync(createdByEmployeeId.Value, establishmentId.Value);

            return new OrderItemsPayments(emptyOrder, null, null, null);
        }
        
        public async Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize, IPrincipal user)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize, user);
            foreach (var order in orders.Items)
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(order.CreatedByEmployeeId, user);
                if(employee != null)
                    order.CreatedByEmployeeName = employee.FirstName + " " + employee.LastName;
            }
            
            return orders;
        }

        public async Task<OrderItemsPayments> GetOrderById(int orderId, IPrincipal user)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, user);
            if (order == null)
            {
                _logger.LogInformation($"Order with id: {orderId} not found");
                return new OrderItemsPayments(null, null, null, null);
            }
            
            var employee = await _employeeRepository.GetEmployeeByIdAsync(order.CreatedByEmployeeId, user);
            if(employee != null)
                order.CreatedByEmployeeName = employee.FirstName + " " + employee.LastName;

            var orderItems = await GetOrderItems(orderId, user);

            var orderServices = await GetOrderServices(orderId, user);
            
            var orderWithTotalPrice = await CalculateTotalPriceForOrder(order, orderItems, orderServices);

            var orderPayments = await GetOrderPayments(orderId);

            var orderWithTotalPaidAndLeftToPay = CalculateTotalPaidAndLeftToPayForOrder(orderWithTotalPrice, orderPayments);
            
            return new OrderItemsPayments(orderWithTotalPaidAndLeftToPay, orderItems, orderServices, orderPayments);
        }
        
        private async Task<List<ItemModel>> GetOrderItems(int orderId, IPrincipal user)
        {
            var fullOrders = await _fullOrderRepository.GetOrderItemsAsync(orderId);
            
            var orderItems = new List<ItemModel>();
            foreach (var fullOrder in fullOrders)
            {
                var item = await _itemRepository.GetItemByIdFromFullOrderAsync(fullOrder.ItemId, orderId);
                
                if(item == null)
                    continue;
                
                if (fullOrder.DiscountId.HasValue)
                {
                    var discount = await GetDiscountById(fullOrder.DiscountId.Value);
                    item.Discount = discount.Value;
                    item.DiscountName = discount.DiscountName + " (" + discount.Value + "%)";
                }

                item.Taxes = await _fullOrderTaxRepository.GetFullOrderItemTaxesAsync(fullOrder.FullOrderId);
          
                item.Count = fullOrder.Count;
                orderItems.Add(item);
            }

            return orderItems;
        }
        
        private async Task<List<ServiceModel>> GetOrderServices(int orderId, IPrincipal user)
        {
            var fullOrderServices = await _fullOrderServiceRepository.GetOrderServicesAsync(orderId);

            var orderServices = new List<ServiceModel>();
            foreach (var fullOrderService in fullOrderServices)
            {
                var service = await _serviceRepository.GetServiceByIdFromFullOrderAsync(fullOrderService.ServiceId, orderId);

                if (service == null)
                    continue;

                if (fullOrderService.DiscountId.HasValue)
                {
                    var discount = await GetDiscountById(fullOrderService.DiscountId.Value);
                    service.Discount = discount.Value;
                    service.DiscountName = discount.DiscountName + " (" + discount.Value + "%)";
                }

                service.Taxes = await _fullOrderServiceTaxRepository.GetFullOrderServiceTaxesAsync(fullOrderService.FullOrderServiceId);

                service.Count = fullOrderService.Count;
                orderServices.Add(service);
            }

            return orderServices;
        }

        private async Task<DiscountModel> GetDiscountById(int discountId)
        {
            var discount = await _discountRepository.GetDiscountAsync(discountId);
            return discount;
        }
        
        private async Task<List<PaymentModel>> GetOrderPayments(int orderId)
        {
            var payments = await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
            return payments;
        }

        private async Task<OrderModel> CalculateTotalPriceForOrder(OrderModel order, List<ItemModel> orderItems, List<ServiceModel> orderServices)
        {
            decimal totalPrice = 0;
            foreach (var item in orderItems)
            {
                decimal itemCost = item.Cost ?? 0;
                decimal itemCount = item.Count ?? 0;

                // Apply discount to the item if it exists
                if (item.Discount.HasValue)
                {
                    itemCost -= Math.Round(itemCost * (item.Discount.Value / 100), 2);
                }

                foreach(var tax in item.Taxes)
                {
                    totalPrice += (Math.Round(tax.Percentage / 100 * itemCost, 2) * itemCount);
                }

                totalPrice += itemCost * itemCount;
            }

            foreach (var service in orderServices)
            {
                decimal serviceCost = service.Cost ?? 0;
                decimal serviceCount = service.Count ?? 0;

                // Apply discount to the item if it exists
                if (service.Discount.HasValue)
                {
                    serviceCost -= Math.Round(serviceCost * (service.Discount.Value / 100), 2);
                }

                foreach (var tax in service.Taxes)
                {
                    totalPrice += (Math.Round(tax.Percentage / 100 * serviceCost, 2) * serviceCount);
                }

                totalPrice += serviceCost * serviceCount;
            }

            // Apply discount to the total price
            if (order.DiscountId.HasValue)
            {
                var discount = await GetDiscountById(order.DiscountId.Value);
                totalPrice -= Math.Round(totalPrice * (discount.Value / 100), 2);
            }

            if (order.TipFixed != null)
            {
                totalPrice += (order.TipFixed ?? 0);
                order.TipAmount = (order.TipFixed ?? 0);
            }
            else if (order.TipPercentage != null)
            {
                decimal tip = Math.Round(totalPrice * ((order.TipPercentage ?? 0) / 100), 2);
                totalPrice += tip;
                order.TipAmount = tip;
            }

            order.TotalPrice = Math.Round(totalPrice, 2);
            
            return order;
        }
        private OrderModel CalculateTotalPaidAndLeftToPayForOrder(OrderModel order, List<PaymentModel> orderPayments)
        {
            decimal totalPaid = orderPayments.Sum(payment => payment.Value);

            order.TotalPaid = totalPaid;
            order.LeftToPay = order.TotalPrice - order.TotalPaid;

            return order;
        }

        public async Task AddItemToOrder(FullOrderModel fullOrder, int? userId, IPrincipal user)
        {
            if (!userId.HasValue)
            {
                _logger.LogError("Failed to add item to order: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            // Before adding an item to an order, check if:
            // 1. The order exists
            // 2. The item exists and there is enough stock in storage
            await GetOrderIfExistsAndStatusIs(fullOrder.OrderId, (int)OrderStatusEnum.Open,user, "AddItemToOrder");

            await ItemIsAvailableInStorage();
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);
            
            // Reduce reserved item count in storage
            await _itemRepository.AddStorageAsync(fullOrder.ItemId, -fullOrder.Count);

            // If the item is already in the order (fullOrder record which links the order with the item exists in the database)
            // update its quantity by adding new count to existing count
            // Otherwise, create a new record for it
            if (existingFullOrder != null)
            {
                await _fullOrderRepository.UpdateItemInOrderCountAsync(fullOrder);
            }
            else
            {
                await _fullOrderRepository.AddItemToOrderAsync(fullOrder, userId.Value);

                //refetch to get back the id
                existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);

                //Save tax historic data
                var taxes = await _taxService.GetItemTaxes(fullOrder.ItemId);

                foreach(var tax in taxes)
                {
                    var fullOrderTax = new FullOrderTaxModel
                    {
                        FullOrderId = existingFullOrder.FullOrderId,
                        Percentage = tax.Percentage,
                        Description = tax.Description
                    };

                    await _fullOrderTaxRepository.AddItemToFullOrderTaxAsync(fullOrderTax);
                }
            }

            async Task ItemIsAvailableInStorage()
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
            }
        }

        public async Task AddServiceToOrder(FullOrderServiceModel fullOrderServiceModel, int? userId, IPrincipal user)
        {
            if (!userId.HasValue)
            {
                _logger.LogError("Failed to add item to order: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            var existingFullOrderService = await _fullOrderServiceRepository.GetFullOrderServiceAsync(fullOrderServiceModel.OrderId, fullOrderServiceModel.ServiceId);

            if (existingFullOrderService != null)
            {
                await _fullOrderServiceRepository.UpdateServiceInOrderCountAsync(fullOrderServiceModel);
            }
            else
            {
                await _fullOrderServiceRepository.AddServiceToOrderAsync(fullOrderServiceModel, userId.Value);

                //refetch to get back the id
                existingFullOrderService = await _fullOrderServiceRepository.GetFullOrderServiceAsync(fullOrderServiceModel.OrderId, fullOrderServiceModel.ServiceId);

                //Save tax historic data
                var taxes = await _taxService.GetServiceTaxes(fullOrderServiceModel.ServiceId);

                foreach (var tax in taxes)
                {
                    var fullOrderTax = new FullOrderServiceTaxModel
                    {
                        FullOrderServiceId = existingFullOrderService.FullOrderServiceId,
                        Percentage = tax.Percentage,
                        Description = tax.Description
                    };

                    await _fullOrderServiceTaxRepository.AddItemToFullOrderServiceTaxAsync(fullOrderTax);
                }
            }
        }
        
        public async Task RemoveItemFromOrder(FullOrderModel fullOrder, IPrincipal user)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(fullOrder.OrderId, (int)OrderStatusEnum.Open, user, "RemoveItemFromOrder");
            if (existingOrderWithOpenStatus == null)
                return;
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);
            if (existingFullOrder == null)
            {
                _logger.LogError($"The specified item {fullOrder.ItemId} is not linked to the given order {fullOrder.OrderId}");
                throw new ItemNotFoundInOrderException(fullOrder.OrderId, fullOrder.ItemId);
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

        public async Task RemoveServiceFromOrder(FullOrderServiceModel fullOrderService, IPrincipal user)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(fullOrderService.OrderId, (int)OrderStatusEnum.Open, user, "RemoveItemFromOrder");
            if (existingOrderWithOpenStatus == null)
                return;

            var existingFullOrderService = await _fullOrderServiceRepository.GetFullOrderServiceAsync(fullOrderService.OrderId, fullOrderService.ServiceId);
            if (existingFullOrderService == null)
            {
                _logger.LogError($"The specified service {fullOrderService.ServiceId} is not linked to the given order {fullOrderService.OrderId}");
                throw new ItemNotFoundInOrderException(fullOrderService.ServiceId, fullOrderService.OrderId);
            }

            if (fullOrderService.Count < existingFullOrderService.Count)
            {
                fullOrderService.Count = -fullOrderService.Count;
                _logger.LogInformation($"Updating service count {fullOrderService.Count}");
                await _fullOrderServiceRepository.UpdateServiceInOrderCountAsync(fullOrderService);
                return;
            }
            await _fullOrderServiceRepository.DeleteServiceFromOrderAsync(fullOrderService);
        }

        public async Task UpdateOrder(OrderModel order, IPrincipal user)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(order.OrderId, (int)OrderStatusEnum.Open, user, "UpdateOrder");
            if (existingOrderWithOpenStatus == null)
                throw new OrderNotFoundException(order.OrderId);
            
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task CloseOrder(int orderId, IPrincipal user)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(orderId, (int)OrderStatusEnum.Open, user, "CloseOrder");
            if(existingOrderWithOpenStatus == null)
                return;

            existingOrderWithOpenStatus.Status = (int)OrderStatusEnum.Closed;
            
            await _orderRepository.UpdateOrderAsync(existingOrderWithOpenStatus);
        }
        
        private async Task<OrderModel?> GetOrderIfExistsAndStatusIs(int orderId, int orderStatus, IPrincipal user, string? operation = null)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, user);
            if (order == null)
            {
                _logger.LogError($"Operation '{operation}' failed: Order {orderId} not found");
                throw new OrderNotFoundException(orderId);
            }

            if (order.Status != orderStatus)
            {
                _logger.LogError($"Operation '{operation}' failed: Order status is {order.Status}");
                throw new OrderStatusConflictException(order.Status.ToString());
            }

            return order;
        }
        public async Task CancelOrder(int orderId, IPrincipal user)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIs(orderId, (int)OrderStatusEnum.Open, user, "CancelOrder");
            if (existingOrderWithOpenStatus == null)
                return;

            existingOrderWithOpenStatus.Status = (int)OrderStatusEnum.Cancelled;

            //Add items back to storage

            var orderItems = await GetOrderItems(orderId, user);
            foreach (var item in orderItems)
            {
                int itemCount = item.Count ?? 0;
                await _itemRepository.AddStorageAsync(item.ItemId, itemCount);
            }

            // TODO refund if payments made

            await _orderRepository.UpdateOrderAsync(existingOrderWithOpenStatus);
        }
        public async Task PayOrder(PaymentModel payment, IPrincipal user)
        {
            var existingOrderWithClosedStatus = (await GetOrderById(payment.OrderId, user)).Order;

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

            if (existingOrderWithClosedStatus.LeftToPay == payment.Value)
            {
                existingOrderWithClosedStatus.Status = (int)OrderStatusEnum.Completed;
                await _orderRepository.UpdateOrderAsync(existingOrderWithClosedStatus);
            }

            await _paymentRepository.AddPaymentAsync(payment);
        }
        public async Task RefundOrder(int orderId, IPrincipal user)
        {
            var order = await GetOrderIfExistsAndStatusIs(orderId, (int)OrderStatusEnum.Completed, user, "RefundOrder");

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

        public async Task<byte[]> DownloadReceipt(int orderId, IPrincipal user)
        {
            var order = await GetOrderById(orderId, user);
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
        public async Task TipOrder(TipModel tip, IPrincipal user)
        {
            var order = await GetOrderIfExistsAndStatusIs(tip.OrderId, (int)OrderStatusEnum.Open, user, "TipOrder");
            if (order == null)
            {
                return;
            }

            if(tip.Type == (int)TipEnum.Fixed)
            {
                order.TipPercentage = null;
                order.TipFixed = tip.Amount;


            } else if(tip.Type == (int)TipEnum.Percentage)
            {
                order.TipFixed = null;
                order.TipPercentage = tip.Amount;
            }

            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task DiscountOrder(DiscountModel discount, IPrincipal user)
        {
            // If discount applied to a specific item in the order
            if (discount.ItemId.HasValue)
            {
                var fullOrder = await _fullOrderRepository.GetFullOrderAsync(discount.OrderId, discount.ItemId.Value);

                if (fullOrder == null)
                {
                    return;
                }
                
                fullOrder.DiscountId = discount.DiscountId;

                await _fullOrderRepository.UpdateFullOrderDiscountAsync(fullOrder);
            }
            else if (discount.ServiceId.HasValue)
            {
                var fullOrderService = await _fullOrderServiceRepository.GetFullOrderServiceAsync(discount.OrderId, discount.ServiceId.Value);

                if (fullOrderService == null)
                {
                    return;
                }

                fullOrderService.DiscountId = discount.DiscountId;

                await _fullOrderServiceRepository.UpdateFullOrderServiceDiscountAsync(fullOrderService);
            }
            else
            {
                var order = await GetOrderIfExistsAndStatusIs(discount.OrderId, (int)OrderStatusEnum.Open, user, "DiscountOrder");

                if (order == null)
                {
                    return;
                }

                order.DiscountId = discount.DiscountId;

                await _orderRepository.UpdateOrderAsync(order);
            }
        }
    }
}

