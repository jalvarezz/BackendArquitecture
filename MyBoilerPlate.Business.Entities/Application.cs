using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace MyBoilerPlate.Business.Entities
{
    [Table("Application")]
    public class Application : AuditableEntityBase<Application>, IDeleteableEntity, IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }

        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public DateTime? ActiveFrom { get; set; }

        public DateTime? ActiveTo { get; set; }

        public int EmailMaxAttempt { get; set; }

        public int SMSMaxAttempt { get; set; }

        public bool IsDeleted { get; set; }

        #region Relationships

        [JsonIgnore]
        public virtual ICollection<ApplicationKey> ApplicationKeys { get; set; }
        
        #endregion
    }
}
