using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyBoilerPlate.Web.Models
{
    [DataContract]
    public class EmployeeTypeViewModel
    {
        #region Properties

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        #endregion
    }
}
