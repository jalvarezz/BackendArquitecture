using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyBoilerPlate.Web.Models
{
    [DataContract]
    public class EmployeeViewModel
    {
        #region Properties

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int? EmployeeTypeId { get; set; }

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
        public virtual EmployeeTypeViewModel EmployeeType { get; set; }

        #endregion
    }
}
