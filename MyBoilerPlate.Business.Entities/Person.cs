using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace TechAssist.Business.Entities
{
    [DataContract]
    [Table("Person")]
    public class Person : EntityBase, IAuditableEntity, IDeferrableEntity
    {
        #region Properties
        [DataMember]
        [Key]
        [Column("PersonId")]
        [MaxLength(10)]
        public long Id { get; set; }

        [DataMember]
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [DataMember]
        [Required]
        public string Message { get; set; }

        #endregion

        #region Interface Implentations

        [DataMember]
        [Required]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        [Required]
        public Guid CreatedById { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        [DataMember]
        public Guid? UpdatedById { get; set; }
        
        [IgnoreDataMember]
        [NotMapped]
        public long EntityId
        {
            get { return Id; }
            set { Id = value; }
        }

        [DataMember]
        [Required]
        public bool Deferred { get; set; }

        #endregion
    }
}
