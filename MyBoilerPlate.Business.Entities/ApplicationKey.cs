using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace MyBoilerPlate.Business.Entities.IDENTITY
{
    [Table("ApplicationKey")]
    public class ApplicationKey : AuditableEntityBase<ApplicationKey>, IDeleteableEntity, IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationKeyId { get; set; }

        [Required]
        [ForeignKey("Application")]
        public int? ApplicationId { get; set; }

        [MaxLength(96)]
        [Required]
        [Column("AccessKey")]
        public string AccessKey { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        #region Relationships

        public virtual Application Application { get; set; }

        #endregion
    }
}
