using Core.Common.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyBoilerPlate.Web.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure.Events
{
    public class BoilerPlateJwtBearerEvents : JwtBearerEvents
    {
        public async override Task TokenValidated(TokenValidatedContext context)
        {
            var identity = context.Principal.Identity as ClaimsIdentity;

            // TODO: Load user profile claims here. Remember to use cache for this!

            var claims = new List<Claim>();

            claims.Add(new Claim("profileprop1", "something"));
            claims.Add(new Claim("profileprop2", "something"));

            // add claims to the identity
            identity.AddClaims(claims);
        }
    }
}
