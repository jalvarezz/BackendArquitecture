using Core.Common.Contracts;
using MyBoilerPlate.Web.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MyBoilerPlate.Web.Services
{

    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _EmailSetting;
        private readonly IDataRepositoryFactory _DataRepositoryFactory;

        public EmailSender(EmailSetting emailSetting, IDataRepositoryFactory dataRepositoryFactory)
        {
            _EmailSetting = emailSetting;
            _DataRepositoryFactory = dataRepositoryFactory;
        }
    }
}