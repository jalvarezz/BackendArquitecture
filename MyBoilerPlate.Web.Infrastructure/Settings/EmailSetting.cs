namespace MyBoilerPlate.Web.Infrastructure.Settings
{
    public class EmailSetting
    {
        public string SmtpClient { get; set; } 

        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public int Timeout { get; set; }
        public string MailAddressFrom { get; set; }
        public string MailUsername { get; set; }
        public string MailPassword { get; set; }
    }
}
