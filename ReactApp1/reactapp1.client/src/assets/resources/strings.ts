// src/resources/strings.js
const ScriptResources = {
    Cancel: 'Cancel',
    Save: 'Save',
    Delete: 'Delete',
    Edit: 'Edit',
    Loading: 'Loading...',
    BackToTheMainList: 'Back to the main list',
    Storage: 'Storage:',
    ReceiveTime: 'Receive Time:',
    StartTime: 'Start Time:',
    EndTime: 'End Time:',
    ReservationsList: 'Reservations List:',
    CustomerPhoneNumber: 'Customer Phone Number:',
    EstablishmentId: 'Establishment Id:',
    EstablishmentAddressId: 'Establishment Address Id:',
    EstablishmentAddress: 'Establishment Address:',
    Date: 'Date:',
    DateRequired: 'Date Required!',
    WorkingHours: 'Working Hours:',
    ErrorFetchingWorkingHours: 'Error Fetching Working Hours:',
    ErrorFetchingEstablishmentDetails: 'Error Fetching Establishment Details:',
    ErrorFetchingReservationDetails: 'Error Fetching Reservation Details:',
    Service: 'Service:',
    ServiceRequired: 'Service Required!',
    SelectService: 'Select Service:',
    TimeSlot: 'Time Slot:',
    SelectTimeSlot: 'Select Time Slot:',
    CreateNewReservation: 'Create New Reservation:',
    EditReservation: 'Edit Reservation:',
    CreatedByEmployeeId: 'Created By Employee Id:',
    CreatedByEmployee: 'Created By Employee:',
    UnknownEmployee: 'Unknown Employee',
    AlcoholicBeverage: 'Alcoholic Beverage:',
    NewReservationInformation: 'New Reservation Information:',
    ReservationInformation: 'Reservation Information:',
    CustomerPhoneNumberRequired: 'Customer phone number is required!',
    Tax: 'Tax:',
    Cost: 'Cost:',
    Name: 'Name:',
    ItemId: 'Item ID:',
    ErrorFetchingItems: 'Error fetching items:',
    ErrorFetchingReservations: 'Error fetching reservations:',
    ErrorFetchingEmployeeDetails: 'Error fetching employee details:',
    ErrorFetchingServiceDetails: 'Error fetching service details:',
    NameIsRequired: 'Name is required.',
    CostMustBeGreaterThanZero: 'Cost must be greater than zero.',
    TaxCannotBeNegative: 'Tax cannot be negative.',
    StorageCannotBeEmpty: 'Storage cannot be empty.',
    ErrorSavingItem: 'Error saving item:',
    FailedToDeleteItem: 'Failed to delete the item.',
    ErrorDeletingItem: 'Error deleting the item:',
    ErrorDeletingReservation: 'Error deleting the reservation:',
    NewItemInformation: 'New Item Information',
    ItemInformation: 'Item Information',
    ErrorFetchingServices: 'Error Fetching Services',
    ErrorDeletingService: 'Error Deleting Service:',
    ErrorSavingService: 'Error Saving Service:',
    ServicesList: 'Services List:',
    ServiceLength: 'Service Length:',
    NewService: 'New Service:',
    ServiceDetail: 'Service Details:',
    ServiceId: 'Service Id:',
    CreateNew: 'Create New',
    ItemsList: 'Items List',
    Actions: 'Actions',
    LoginFailed: 'Login failed',
    LoginSuccessful: 'Login successful',
    Login: 'Login',
    Error: 'Error:',
    Email: 'Email',
    Password: 'Password',
    POSSystem: 'POS System',
    Home: 'Home',
    Items: 'Items',
    Inventory: 'Inventory',
    Employees: 'Employees',
    Establishments: 'Establishments',
    Profile: 'Profile',
    Preferences: 'Preferences',
    Logout: 'Logout',
    Register: 'Register',
    ItemsPerPage: 'Items per page:',
    TotalItems: 'Total Items:',
    DuplicateKeysError: 'Duplicate keys found in ScriptResources',
    AddStorage: 'Add Storage',
    DeductStorage: 'Deduct Storage',
    AddStorageNumber: 'Add Storage Number',
    StorageAdded: 'Enter the storage number you want to add',
    FailedToUpdateStorage: 'Failed to update storage',
    StorageShouldBePositive: 'Storage should be positive',
    LastName : 'Last Name:',
    PhoneNumber: 'Phone Number:',
    EmployeesList: 'Employees List',
    NewEmployeeInformation: 'New Employee Information',
    EmployeesInformation: 'Employees Information',
    EmployeeId: 'Employee ID:',
    PersonalCode: 'Personal Code:',
    BirthDate: 'Birth Date:',
    EmployeeEmail: 'Email:',
    Country: 'Country:',
    City: 'City:',
    Street: 'Street:',
    StreetNumber: 'Street Number:',
    HouseNumber: 'House Number:',
    InvalidEmail: 'Invalid email',
    EmailIsRequired: 'Email is required',
    Title: 'Title:',
    OrdersList: 'Orders List',
    OrderId: 'Order ID:',
    Status: 'Status:',
    OrderStatusEnum_None: 'None',
    OrderStatusEnum_Open: 'Open',
    OrderStatusEnum_Closed: 'Closed',
    OrderStatusEnum_Cancelled: 'Cancelled',
    OrderStatusEnum_Completed: 'Completed',
    OrderInformation: 'Order Information',
    Order: 'Order',
    NotAvailable: 'Not available',
    DiscountPercentage: 'Discount Percentage:',
    DiscountFixed: 'Discount Fixed:',
    Refunded: 'Refunded:',
    No: 'No',
    Yes: 'Yes',
    CreatedBy: 'Created By:',
    ErrorAddingItem: 'Error adding item:',
    AddItem: 'Add Item',
    AddNewItem: 'Add New Item',
    Add: 'Add',
    Checkout: 'Checkout',
    ErrorClosingOrder: 'Error closing order:',
    ErrorCancellingOrder: 'Error cancelling order',
    NoItems: 'No items',
    TotalPrice: 'Total Price:',
    Eur: 'EUR',
    Count: 'Count:',
    SelectCount: 'Select count',
    AddPayment: 'Add payment',
    PaymentType: 'Payment type',
    Cash: 'Cash',
    GiftCard: 'Giftcard',
    Card: 'Card',
    Amount: 'Amount',
    Payments: 'Payments',
    NoPayments: 'No payments',
    Pay: 'Pay',
    ErrorPayment: 'Error making payment',
    TotalPaid: 'Total paid',
    TotalLeftToPay: 'Total left to pay',
    GiftCardCode: 'Giftcard code',
    PaymentNone: 'None',
    PaymentCash: 'Cash',
    PaymentGiftcard: 'Giftcard',
    PaymentCard: 'Card',
    Currency: 'eur',
    Refund: 'Refund',
    ErrorRefund: 'Error refunding order',
    Processing: 'Processing...',
    RefundWarning: "Do you really want to refund all payments made for this order? This action is irreversible.",
    Receipt: "Receipt",
    Download: "Download",
    NoDataFound: "No data found",
    OrderDetails: "Order Details",
    Employee: 'Employee',
    Euro: '€',
    ErrorTip: 'Error setting tip',
    TipType: 'Tip type',
    Percentage: 'Percentage',
    Fixed: 'Fixed',
    ApplyTip: 'Apply tip',
    Tip: 'Tip',
    TipAmount: 'Tip amount',

};

function checkForDuplicateKeys(resourceObject: Record<string, string>): void {
    const keys = Object.keys(resourceObject);
    const uniqueKeys = new Set(keys);

    if (uniqueKeys.size !== keys.length) {
        alert(ScriptResources.DuplicateKeysError);
        throw new Error(ScriptResources.DuplicateKeysError);
    }
}

checkForDuplicateKeys(ScriptResources);

export default ScriptResources;