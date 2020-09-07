using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Common.Extensions
{
    public static class ExpressionExtensions
    {
        public static MemberExpression GetNestedMemberExpression(this ParameterExpression parameter, string propertyName)
        {
            if (parameter == null) { return null; }

            MemberExpression memExp = null;

            foreach (string part in propertyName.Split('.'))
            {
                if (memExp == null)
                    memExp = Expression.Property(parameter, part);
                else
                    memExp = Expression.Property(memExp, part);
            }

            return memExp;
        }

        public static PropertyInfo GetNestedPropertyInfo(this Type source, string propertyName)
        {
            if (source == null) { return null; }

            PropertyInfo info = null;
            
            foreach (string part in propertyName.Split('.'))
            {
                if (info == null)
                {
                    info = source.GetProperty(part);
                }
                else
                    info = info.PropertyType.GetProperty(part);
            }

            return info;
        }
    }
}
