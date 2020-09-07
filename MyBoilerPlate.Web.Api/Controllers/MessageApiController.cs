using System.Collections.Generic;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyBoilerPlate.Web.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/messages")]
    [ApiController]
    public class MessageApiController : ControllerBase
    {
        private readonly IMessageHandler _MessageHandler;

        public MessageApiController(IMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }

        [HttpGet]
        public IEnumerable<KeyValueDTO<string>> GetValidationMessages([FromQuery]string[] messageCodes)
        {
            return _MessageHandler.GetMessages(messageCodes);
        }
    }
}