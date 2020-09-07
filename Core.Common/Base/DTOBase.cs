using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.Common.Extensions;
using System;

namespace Core.Common.Base
{
    /// <summary>
    /// Base client applied to the Business Entities
    /// </summary>
    [DataContract]
    public abstract class DTOBase<T> : IEquatable<T>
    {
        public override bool Equals(object obj)
        {
            return this.Equals((T)obj);
        }

        public bool Equals(T x)
        {
            //create variables to store object values
            object value1 = null, value2 = null;

            PropertyInfo[] properties = x.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //get all public properties of the object using reflection  
            foreach(PropertyInfo propertyInfo in properties)
            {
                //get the property values of both the objects
                value1 = propertyInfo.GetValue(x, null);
                value2 = propertyInfo.GetValue(this, null);

                if(Equals(value1, value2))
                    continue;
                else
                    return false;
            }

            return true;
        }

        public int GetHashCode(T obj)
        {
            int hashCode = this.GetHashCodeOnProperties();
            return hashCode;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode((T)Convert.ChangeType(this, typeof(T)));
        }
    }
}
