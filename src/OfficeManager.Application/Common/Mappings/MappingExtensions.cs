using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using System.Linq.Expressions;

namespace OfficeManager.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this List<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string columnName, bool isAscending = true)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                  new Type[] { source.ElementType, property.Type },
                                  source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}
