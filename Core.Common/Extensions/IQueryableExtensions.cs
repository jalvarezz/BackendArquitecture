using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            //If there is no property, do not sort
            if (string.IsNullOrEmpty(propertyName))
                return source;

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            var property = type.GetNestedPropertyInfo(propertyName);
            var propertyAccess = parameter.GetNestedMemberExpression(propertyName);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string direction)
        {
            //If there is no property, do not sort
            if (string.IsNullOrEmpty(propertyName))
                return source;

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            var property = type.GetNestedPropertyInfo(propertyName);
            var propertyAccess = parameter.GetNestedMemberExpression(propertyName);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), direction == "ASC" ? "OrderBy" : "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
