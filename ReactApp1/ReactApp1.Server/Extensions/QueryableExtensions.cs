namespace ReactApp1.Server.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            return source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
