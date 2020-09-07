using Core.Common.Base;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Common.Extensions
{
    public static class CoreExtensions
    {
        static Dictionary<string, bool> _BrowsableProperties = new Dictionary<string, bool>();
        static Dictionary<string, PropertyInfo[]> _BrowsablePropertyInfos = new Dictionary<string, PropertyInfo[]>();

        public static bool IsNavigable(this PropertyInfo obj)
        {
            return Attribute.IsDefined(obj, obj.PropertyType);
        }

        public static bool IsBrowsable(this object obj, PropertyInfo property)
        {
            string key = $"{obj.GetType()}.{property.Name}";

            if(!_BrowsableProperties.ContainsKey(key))
            {
                bool browsable = property.IsNavigable();
                _BrowsableProperties.Add(key, browsable);
            }

            return _BrowsableProperties[key];
        }

        public static PropertyInfo[] GetBrowsableProperties(this object obj)
        {
            string key = obj.GetType().ToString();

            if(!_BrowsablePropertyInfos.ContainsKey(key))
            {
                List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
                PropertyInfo[] properties = obj.GetType().GetProperties();
                foreach(PropertyInfo property in properties)
                {
                    if((property.PropertyType.IsSubclassOf(typeof(ObjectBase)) || property.PropertyType.GetInterface("IList") != null) &&
                        IsBrowsable(obj, property))
                    {
                        propertyInfoList.Add(property);
                    }
                }

                _BrowsablePropertyInfos.Add(key, propertyInfoList.ToArray());
            }

            return _BrowsablePropertyInfos[key];
        }
    }
}
