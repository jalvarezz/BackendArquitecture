﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;
using System.Linq;
using MyBoilerPlate.Gateways.NeverBounce;
using MyBoilerPlate.Gateways.AWS.Contracts;
using MyBoilerPlate.Gateways.NeverBounce.Contracts;
using MyBoilerPlate.Gateways.Twilio.Contracts;
using MyBoilerPlate.Gateways.Twilio;
using Core.Common.Contracts;
using MyBoilerPlate.Business;
using System.Net.Http;
using System;
using MyBoilerPlate.Gateways.AWS;

namespace MyBoilerPlate.Web.Infrastructure.Installers
{
    public class GatewayInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var neverBounceSettings = new NeverBounceSetting();
            configuration.GetSection(nameof(NeverBounceSetting)).Bind(neverBounceSettings);

            if (!services.Any(x => x.ServiceType == typeof(NeverBounceSetting)))
                services.AddSingleton(neverBounceSettings);

            services.AddTransient<INeverBounceGateway, NeverBounceGateway>();
            services.AddTransient<IAWSGateway, AWSGateway>();
            services.AddTransient<ITwilioGateway, TwilioGateway>();

            services.AddTransient<IGatewayFactory, GatewayFactory>();

            services.AddHttpClient("bounce", c =>
            {
                ConfigureBounceHttpClient(c, neverBounceSettings);
            });
        }

        private void ConfigureBounceHttpClient(HttpClient client, NeverBounceSetting settings)
        {
            client.BaseAddress = new Uri($"{settings.Host}/{settings.Version}/");
        }
    }
}
