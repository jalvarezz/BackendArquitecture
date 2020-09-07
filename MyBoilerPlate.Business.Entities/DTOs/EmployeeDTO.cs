using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MyBoilerPlate.Business.Entities.DTOs
{
    public class EmployeeDTO : DTOBase<EmployeeDTO>
    {
        #region Properties

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int EmployeeTypeId { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string OfficialDocumentId { get; set; }
        #endregion

        #region Relationships

        [DataMember]
        public virtual EmployeeTypeDTO EmployeeType { get; set; }

        #endregion
    }
}
