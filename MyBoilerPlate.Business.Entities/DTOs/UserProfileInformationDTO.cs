using Core.Common.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyBoilerPlate.Business.Entities.DTOs
{
    public class UserProfileInformationDTO : DTOBase<UserProfileInformationDTO>
    {
        public UserProfileDTO User { get; set; }
        
        public string[] Permissions { get; set; }
    }
}
