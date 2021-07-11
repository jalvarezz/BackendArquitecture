using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Core.Common
{
    public class ApplicationProfile : IApplicationProfile
    {
        public int? ApplicationId { get; set; }
    }
}
