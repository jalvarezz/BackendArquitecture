using Microsoft.AspNetCore.Authorization;
using MyBoilerPlate.Web.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure.Policy
{
    public static class AuthorizationOptionsExtensions
    {
        public static void SetEndpointPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.ClientApplicationAccess, policy =>
            {
                policy.Requirements.Add(new ClientApplicationAccessRequirement());
            });

            options.AddPolicy(PolicyNames.WebApi, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && x.Value == ScopeNames.WebApi);
                });
            });

            options.AddPolicy(PolicyNames.AllDevices, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" &&
                        (x.Value == ScopeNames.WebApi ||
                         x.Value == ScopeNames.Pwa ||
                         x.Value == ScopeNames.Kiosk ||
                         x.Value == ScopeNames.Mobile ||
                         x.Value == ScopeNames.Display));
                });
            });


            options.AddPolicy(PolicyNames.Device, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" &&
                        (x.Value == ScopeNames.Pwa || x.Value == ScopeNames.Kiosk || x.Value == ScopeNames.Display || x.Value == ScopeNames.Mobile));
                });
            });

            options.AddPolicy(PolicyNames.WebPwa, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && (
                        x.Value == ScopeNames.WebApi ||
                        x.Value == ScopeNames.Pwa));
                });
            });

            options.AddPolicy(PolicyNames.WebPwaMobile, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && (
                        x.Value == ScopeNames.WebApi ||
                        x.Value == ScopeNames.Pwa ||
                        x.Value == ScopeNames.Mobile));
                });
            });

            options.AddPolicy(PolicyNames.WebKiosk, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && (
                        x.Value == ScopeNames.WebApi ||
                        x.Value == ScopeNames.Kiosk));
                });
            });

            options.AddPolicy(PolicyNames.WebDisplay, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && (
                        x.Value == ScopeNames.WebApi ||
                        x.Value == ScopeNames.Display));
                });
            });

            options.AddPolicy(PolicyNames.WebKioskPwa, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && (
                        x.Value == ScopeNames.WebApi ||
                        x.Value == ScopeNames.Kiosk ||
                        x.Value == ScopeNames.Pwa));
                });
            });

            options.AddPolicy(PolicyNames.KioskPwa, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => (x.Type == "scope" && (
                        x.Value == ScopeNames.Kiosk ||
                        x.Value == ScopeNames.Pwa)));
                });
            });

            options.AddPolicy(PolicyNames.Pwa, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && x.Value == ScopeNames.Pwa);
                });
            });

            options.AddPolicy(PolicyNames.Kiosk, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && x.Value == ScopeNames.Kiosk);
                });
            });

            options.AddPolicy(PolicyNames.Display, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && x.Value == ScopeNames.Display);
                });
            });

            options.AddPolicy(PolicyNames.Mobile, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && x.Value == ScopeNames.Mobile);
                });
            });
        }
    }
}
