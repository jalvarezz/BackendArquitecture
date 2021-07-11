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
    [Table("EmployeeType")]
    public class EmployeeType : AuditableEntityBase<Employee>, IDeleteableEntity
    {
        #region Properties
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        #endregion

        #region Interface Implentations

        [Required]
        public bool IsDeleted { get; set; }

        #endregion
    }
}
