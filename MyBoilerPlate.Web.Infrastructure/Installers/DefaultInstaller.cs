using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;
using MyBoilerPlate.Resources;
using System.Globalization;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Web.Services;
using MyBoilerPlate.Business.Handlers;
using MyBoilerPlate.Web.Infrastructure;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Resources;
using Core.Common.Contracts;

namespace MyBoilerPlate.Web.Infrastructure.Installers
{
    public class DefaultInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Set the AppSettings information
            var appSettingValues = new AppSettings();
            var emailSettingValues = new EmailSetting();

            configuration.GetSection(nameof(AppSettings)).Bind(appSettingValues);
            configuration.GetSection(nameof(EmailSetting)).Bind(emailSettingValues);

            services.AddSingleton<AppSettings>(appSettingValues);
            services.AddSingleton<EmailSetting>(emailSettingValues);

            var authorizerSetting = new AuthorizerSetting();
            configuration.GetSection("Authorizer").Bind(authorizerSetting);

            if(!services.Any(x => x.ServiceType == typeof(AuthorizerSetting)))
                services.AddSingleton(authorizerSetting);

            //Handlers and Resources
            services.AddSingleton<IMessagesResourceHandler, MessagesResourceHandler>();
            services.AddSingleton<IMessageHandler, MessageHandler>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IRequestProvider, RequestProvider>();

            //User Profile
            services.AddScoped<IUserProfile, UserProfile>();
        }
    }
}
