using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Contracts
{
    public interface IConcurrencyEntity
    {
        byte[] RowVersion { get; set; }
    }
}
