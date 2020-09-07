using System;

namespace MyBoilerPlate.Web.Infrastructure.Settings
{
    public class AppSettings
    {
        public string PolicyCORS { get; set; }
        public string ActiveDirectoryPath { get; set; }
        public string DomainName { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
