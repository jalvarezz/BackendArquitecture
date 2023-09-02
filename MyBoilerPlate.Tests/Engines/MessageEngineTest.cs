using Core.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Tests;
using MyBoilerPlate.Web.Infrastructure.Services;
using MyBoilerPlate.Tests.Installers;

namespace EduEsp.Tests.Engines
{
    [TestClass]
    public class ValidationMessageHandlerTest : TestBase
    {
        private IMessagesResourceHandler _ResourceHandler;
        private IMessageHandler _MessageHandler;

        [TestInitialize]
        public void Setup()
        {
            this.Initialize((services, configuration) =>
            {
                services.AddDefaultServices(configuration);
            });

            _ResourceHandler = ObjectContainer.GetService<IMessagesResourceHandler>();
            _MessageHandler = ObjectContainer.GetService<IMessageHandler>();
        }

        [TestMethod]
        public void ValidationMessageEngineTest_GetMessages()
        {
            // Arrange
            string[] messages = new string[1] { "M000001" };

            // Act  
            var data = _MessageHandler.GetMessages(messages);

            // Assert
            Assert.IsTrue(data.Any());
        }
    }
}
