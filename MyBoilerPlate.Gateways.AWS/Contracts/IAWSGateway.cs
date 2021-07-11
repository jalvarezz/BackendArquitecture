using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBoilerPlate.Gateways.AWS.Contracts
{
    public interface IAWSGateway : IEmailGateway, ISmsGateway
    {
    }
}
