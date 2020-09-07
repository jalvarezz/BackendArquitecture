using AutoMapper;
using Core.Common;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Web.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace MyBoilerPlate.Web.Infrastructure.AutoMapper
{
    public class AutoMapperCommonProfileConfiguration : Profile
    {
        public AutoMapperCommonProfileConfiguration()
        : this("GlobalProfile")
        {
        }
        protected AutoMapperCommonProfileConfiguration(string profileName)
        : base(profileName)
        {
            this.RecognizePrefixes("Config");

            //NOTE: Only map the types that are common in all your apis
        }
    }
}
