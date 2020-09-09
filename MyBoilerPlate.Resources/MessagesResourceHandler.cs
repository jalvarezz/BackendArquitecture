using Core.Common;
using System.Reflection;
using System.Resources;

namespace MyBoilerPlate.Resources
{
    public class MessagesResourceHandler : IMessagesResourceHandler
    {
        private const string _ResourceFilePath = @"MyBoilerPlate.Resources.Messages";
        private readonly ResourceManager _ResourceManager;

        public MessagesResourceHandler()
        {
            this._ResourceManager = new ResourceManager(_ResourceFilePath, Assembly.GetExecutingAssembly());
        }

        public string GetString(string key)
        {
            return _ResourceManager.GetString(key);
        }
    }
}
