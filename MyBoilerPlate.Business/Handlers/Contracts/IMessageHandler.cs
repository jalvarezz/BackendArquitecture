using System.Collections.Generic;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;

namespace MyBoilerPlate.Business.Contracts
{
    public interface IMessageHandler
    {
        IEnumerable<KeyValueDTO<string>> GetMessages(params string[] messageCodes);

        KeyValueDTO<string> GetMessage(string messageCode);

        KeyValueDTO<string, string> GetMessageWithKey(string messageCode);
    }
}
