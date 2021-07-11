using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MyBoilerPlate.Business.Entities
{
    [Table("Employee")]
    public class Employee : AuditableEntityBase<Employee>, IDeleteableEntity
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EmployeeType")]
        public int? EmployeeTypeId { get; set; }

        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string OfficialDocumentId { get; set; }

        #endregion

        #region Relationships

        public virtual EmployeeType EmployeeType { get; set; }

        #endregion

        #region Interface Implentations

        [Required]
        public bool IsDeleted { get; set; }

        #endregion
    }
}
