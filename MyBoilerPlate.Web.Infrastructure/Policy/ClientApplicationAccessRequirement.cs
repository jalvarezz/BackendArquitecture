using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBoilerPlate.Web.Infrastructure.Policy
{
    public class ClientApplicationAccessRequirement : IAuthorizationRequirement
    {
        public ClientApplicationAccessRequirement() { }
    }
}
