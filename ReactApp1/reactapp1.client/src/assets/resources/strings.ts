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
    AlcoholicBeverage: 'Alcoholic Beverage:',
    Tax: 'Tax:',
    Cost: 'Cost:',
    Name: 'Name:',
    ItemId: 'Item ID:',
    ErrorFetchingItems: 'Error fetching items:',
    NameIsRequired: 'Name is required.',
    CostMustBeGreaterThanZero: 'Cost must be greater than zero.',
    TaxCannotBeNegative: 'Tax cannot be negative.',
    StorageCannotBeEmpty: 'Storage cannot be empty.',
    ErrorSavingItem: 'Error saving item:',
    FailedToDeleteItem: 'Failed to delete the item.',
    ErrorDeletingItem: 'Error deleting the item:',
    NewItemInformation: 'New Item Information',
    ItemInformation: 'Item Information',
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