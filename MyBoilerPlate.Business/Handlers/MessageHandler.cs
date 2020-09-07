using Core.Common;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBoilerPlate.Business.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IMessagesResourceHandler _ResourceHandler;

        public MessageHandler(IMessagesResourceHandler resourceHandler)
        {
            this._ResourceHandler = resourceHandler;
        }

        public IEnumerable<KeyValueDTO<string>> GetMessages(params string[] messageCodes)
        {
            //Validate input parameter
            if (messageCodes == null || messageCodes.Length == 0)
                throw new ArgumentException("No se pudo obtener los mensajes de validación");

            var data = messageCodes.Select(
                key => new KeyValueDTO<string>
                {
                    Id = key,
                    Name = _ResourceHandler.GetString(key),
                });

            return data;
        }

        public KeyValueDTO<string> GetMessage(string messageCode)
        {
            //Validate input parameter
            if (messageCode == null || messageCode.Length == 0)
                throw new ArgumentException("No se pudo obtener los mensajes de validación");

            var data = new KeyValueDTO<string>
            {
                Id = messageCode,
                Name = _ResourceHandler.GetString(messageCode),
            };

            return data;
        }

        public KeyValueDTO<string, string> GetMessageWithKey(string messageCode)
        {
            //Validate input parameter
            if(messageCode == null || messageCode.Length == 0)
                throw new ArgumentException("No se pudo obtener los mensajes de validación");

            var data = new KeyValueDTO<string, string>
            {
                Id = messageCode,
                Name = _ResourceHandler.GetString(messageCode),
            };

            return data;
        }
    }
}
