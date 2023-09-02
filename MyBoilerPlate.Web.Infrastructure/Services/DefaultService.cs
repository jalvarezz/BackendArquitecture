using Core.Common;
using Core.Common.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Handlers;
using MyBoilerPlate.Resources;
using MyBoilerPlate.Web.Infrastructure.Services.Contracts;
using MyBoilerPlate.Web.Infrastructure.Settings;
using System.Linq;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class DefaultService
    {
        public static void AddDefaultServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Set the AppSettings information
            var appSettingValues = new AppSettings();
            var emailSettingValues = new EmailSetting();

            configuration.GetSection(nameof(AppSettings)).Bind(appSettingValues);
            configuration.GetSection(nameof(EmailSetting)).Bind(emailSettingValues);

            services.AddSingleton(appSettingValues);
            services.AddSingleton(emailSettingValues);

            var authorizerSetting = new AuthorizerSetting();
            configuration.GetSection("Authorizer").Bind(authorizerSetting);

            if (!services.Any(x => x.ServiceType == typeof(AuthorizerSetting)))
                services.AddSingleton(authorizerSetting);

            //Handlers and Resources
            services.AddSingleton<IMessagesResourceHandler, MessagesResourceHandler>();
            services.AddSingleton<IMessageHandler, MessageHandler>();

            //User Profile
            services.AddScoped<IUserProfile, UserProfile>();
        }
    }
}
