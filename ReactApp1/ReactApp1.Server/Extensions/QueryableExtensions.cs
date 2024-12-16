using System.Linq.Expressions;
using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;

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

        public static IQueryable<Employee> FilterByAuthorizedUser(this IQueryable<Employee> source, IPrincipal user)
        {
            if (user.GetUserTitle() == TitleEnum.MasterAdmin)
                return source;

            return source.Where(f => f.EstablishmentId == user.GetUserEstablishmentId());
        }
    }
}
