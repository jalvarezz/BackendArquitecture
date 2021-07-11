using Core.Common.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyBoilerPlate.Business.Entities.DTOs
{
    public class UserProfileDTO : DTOBase<UserProfileDTO>
    {
        public string Role { get; set; }
        
        public string RoleId { get; set; }
        
        public string Givenname {
            get
            {
                return $"{this.FirstName} {this.MiddleName ?? string.Empty} {this.LastNamePaternal ?? string.Empty} {this.LastNameMaternal ?? string.Empty}".Replace("  ", " ");
            }
        }
        
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
        
        public string LastNamePaternal { get; set; }
        
        public string LastNameMaternal { get; set; }
        
        public string Sub { get; set; }
        
        public bool Active { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string LastLoginDate { get; set; }
        
        public string LicenseNumber { get; set; }
    }
}
