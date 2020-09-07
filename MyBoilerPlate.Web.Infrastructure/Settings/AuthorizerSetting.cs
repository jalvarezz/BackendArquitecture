namespace MyBoilerPlate.Web.Infrastructure.Settings
{
    public class AuthorizerSetting
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
        public int CacheDuration { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
        public string Tenant { get; set; }
    }
}
