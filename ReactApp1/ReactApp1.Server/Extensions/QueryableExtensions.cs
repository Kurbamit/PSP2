using System.Linq.Expressions;

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
        
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }

            return source;
        }
    }
}
