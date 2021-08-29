using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.Common.Extensions;
using System;
using System.Text.Json.Serialization;
using Core.Common.Contracts;

namespace Core.Common.Base
{
    public abstract class EntityBase
    {

    }

    /// <summary>
    /// Base client applied to the Business Entities
    /// </summary>
    [DataContract]
    public abstract class EntityBase<T> : EntityBase, IEquatable<T>
    {
        #region Property Change Notification


        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is T))
                return false;

            return this.Equals((T)obj);
        }

        public bool Equals(T other)
        {
            //create variables to store object values
            object value1, value2;

            PropertyInfo[] properties = other.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //get all public properties of the object using reflection  
            foreach (PropertyInfo propertyInfo in properties)
            {
                //get the property values of both the objects
                value1 = propertyInfo.GetValue(other, null);
                value2 = propertyInfo.GetValue(this, null);

                if (!Equals(value1, value2))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = this.GetHashCodeOnProperties();
            return hashCode;
        }
    }

    [DataContract]
    public abstract class EntityBase<T, TKey> : EntityBase<T>, IIdentifiableEntity<TKey>
    {
        [NotMapped]
        [IgnoreDataMember]
        [JsonIgnore]
        public virtual TKey EntityId { get; set; }
    }

    [DataContract]
    public class StoredProcedureEntityBase : EntityBase<StoredProcedureEntityBase>
    {

    }

    [DataContract]
    public class FunctionEntityBase : EntityBase<FunctionEntityBase>
    {

    }
}
