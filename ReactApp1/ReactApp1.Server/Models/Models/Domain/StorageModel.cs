namespace ReactApp1.Server.Models.Models.Domain;

public class StorageModel
{
    public int StorageId { get; set; }
    public int EstablishmentId { get; set; }
    public int ItemId { get; set; }
    public int Count { get; set; }

    public StorageModel()
    {
    }
    
    public StorageModel(int storageId, int establishmentId, int itemId, int count)
    {
        StorageId = storageId;
        EstablishmentId = establishmentId;
        ItemId = itemId;
        Count = count;
    }

    public StorageModel(Storage storage)
    : this(storage.StorageId, storage.EstablishmentId, storage.ItemId, storage.Count)
    {
    }
}