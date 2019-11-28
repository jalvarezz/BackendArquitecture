using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Contracts
{
    public interface IIdentifiableEntity
    {
        long EntityId { get; set; }
    }
}
