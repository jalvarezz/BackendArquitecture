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
    [Table("EmployeeType")]
    public class EmployeeType : EntityBase, IAuditableEntity, IDeferrableEntity
    {
        #region Properties
        [DataMember]
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

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

        [DataMember]
        [Required]
        public bool Deferred { get; set; }

        #endregion
    }
}
