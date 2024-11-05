namespace ReactApp1.Server.Models.Models.Base;

public class PaginatedItemsResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
}
