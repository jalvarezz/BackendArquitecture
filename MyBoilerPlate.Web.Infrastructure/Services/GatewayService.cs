using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;
using System.Linq;
using MyBoilerPlate.Gateways.AWS.Contracts;
using MyBoilerPlate.Gateways.Twilio.Contracts;
using MyBoilerPlate.Gateways.Twilio;
using Core.Common.Contracts;
using MyBoilerPlate.Business;
using System.Net.Http;
using System;
using MyBoilerPlate.Gateways.AWS;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class GatewayService
    {
        public static void AddGatewayServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAWSGateway, AWSGateway>();
            services.AddTransient<ITwilioGateway, TwilioGateway>();

            // TODO: Add custom gateways here. Ex.: Whatsapp, CreditCard, etc.
            //       or any other communication with an external service

            // With a factory you can play implementing multiple Creditcard, SMS/Email, Report Gateways
            // This factory uses the name and is generic because it must handle multiple gateway types.
            services.AddTransient<IGatewayFactory, GatewayFactory>();
        }
    }
}
