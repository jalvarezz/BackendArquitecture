using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common.Contracts;

namespace Core.Common.Base
{
    public abstract class AuditableEntityBase<T> : EntityBase<T>, IAuditableEntity
    {
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public Guid CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }

    public abstract class AuditableEntityBase<T, TKey> : EntityBase<T, TKey>, IAuditableEntity
    {
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public Guid CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}
