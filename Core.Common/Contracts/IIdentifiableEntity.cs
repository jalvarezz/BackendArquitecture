using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Common.Contracts
{
    public interface IIdentifiableEntity
    {

    }

    public interface IIdentifiableEntity<T> : IIdentifiableEntity
    {
        T EntityId { get; }
    }
}
