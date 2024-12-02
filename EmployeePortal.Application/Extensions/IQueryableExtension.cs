using EmployeePortal.Application;
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Domain.Entities;
using System.Linq.Expressions;

namespace EmployeePortal.Application
{
    public static class IQueryableExtension
    {
        /// <summary>
        /// If the navigation property is soft deleted then the orginal entity is not selected so we have to ignore the soft delete of the navigation property
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> IgnoreDeletedNavigationProperties<TEntity>(this IQueryable<TEntity> source) where TEntity : class, IBaseEntity
        {
            source = source.IgnoreQueryFilters();
            source = source.Where(s => !s.IsDeleted);
            return source;
        }
        public async static Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
                                       PaginationFilter filter) where T : class
        {
            if (filter == null)
                filter = new PaginationFilter()
                {
                    PageNumber = 1,
                    PageSize = null
                };

            // Check order by
            if (!string.IsNullOrEmpty(filter.SortBy))
                query = query.OrderBy(filter.SortBy, filter.Desc);


            // Check if page number is less than 1
            if (filter.PageNumber < 1)
                filter.PageNumber = 1;


            var result = new PagedResult<T>();
            result.CurrentPage = filter.PageNumber;
            result.RowCount = await query.CountAsync();


            // If the page size is null, then all the rows are return
            result.PageSize = filter.PageSize ?? result.RowCount;

            // Check if page size is less than 1, Atleast one page should be displayed 
            if (result.PageSize < 1)
                result.PageSize = 1;

            var pageCount = (double)result.RowCount / result.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (filter.PageNumber - 1) * result.PageSize;
            result.Results = await query.Skip(skip).Take(result.PageSize).ToListAsync();

            return result;
        }



        /// <summary>
        /// Sort the given query
        /// var thingsQuery = _context.Things
        ///                 .Include(t => t.Other)
        ///                 .Where(t => t.Deleted == false)
        ///                 .OrderBy(sortModels);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortModel"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortBy, bool desc)
        {
            if (string.IsNullOrEmpty(sortBy))
                return source;
            var expression = source.Expression;
            var parameter = Expression.Parameter(typeof(T), "x");
            var selector = Expression.PropertyOrField(parameter, sortBy);
            var method = desc?
                "OrderByDescending" : "OrderBy";

            expression = Expression.Call(typeof(Queryable), method,
                new Type[] { source.ElementType, selector.Type },
                expression, Expression.Quote(Expression.Lambda(selector, parameter)));
            return source.Provider.CreateQuery<T>(expression) ;


        }


    }
}
