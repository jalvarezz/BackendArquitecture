using System;

namespace Core.Common.Contracts
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        Guid CreatedById { get; set; }
        DateTime? UpdatedDate { get; set; }
        Guid? UpdatedById { get; set; }
    }
}
