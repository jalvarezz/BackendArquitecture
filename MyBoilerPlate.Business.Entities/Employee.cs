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
    public class Employee : EntityBase, IAuditableEntity, IDeferrableEntity
    {
        #region Properties
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [ForeignKey("EmployeeType")]
        public int EmployeeTypeId { get; set; }

        [DataMember]
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        [DataMember]
        [Required]
        [MaxLength(20)]
        public string OfficialDocumentId { get; set; }

        #endregion

        #region Relationships

        [DataMember]
        public virtual EmployeeType EmployeeType { get; set; }

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
