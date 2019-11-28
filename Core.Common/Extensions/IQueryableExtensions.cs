using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            //If there is no property, do not sort
            if (string.IsNullOrEmpty(propertyName))
                return source;

            if (!propertyName.Contains(",")) //If contains a ',' then it must be a multi property sort
            {
                bool isDecending = propertyName.Contains(" DESC");
                var trimmedPropertyName = isDecending ? propertyName.Replace(" DESC", "") : propertyName.Replace(" ASC", "");

                var type = typeof(T);
                var parameter = Expression.Parameter(type, "p");
                var property = type.GetNestedPropertyInfo(trimmedPropertyName);
                var propertyAccess = parameter.GetNestedMemberExpression(trimmedPropertyName);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), isDecending ? "OrderByDescending" : "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
                return source.Provider.CreateQuery<T>(resultExp);
            }
            else
            {
                foreach (var propName in propertyName.Split(','))
                {
                    bool isDecending = propName.Contains(" DESC");
                    var trimmedPropertyName = isDecending ? propName.Replace(" DESC", "").Trim() : propName.Replace(" ASC", "").Trim();

                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(T)).Find(trimmedPropertyName, false);

                    if(!isDecending)
                        source = source.OrderBy(x => prop.GetValue(x));
                    else
                        source = source.OrderByDescending(x => prop.GetValue(x));
                }

                return source;
            }
        }

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

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string direction = "ASC")
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
