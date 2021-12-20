namespace MyBoilerPlate.Gateways.ReportServer
{
    public class ReportServerSettings
    {
        public string ServiceUrl { get; set; }
        public string ExecutionUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string RootPath { get; set; }
        public ReportServerDataSource DataSource { get; set; }
    }

    public class ReportServerDataSource
    {
        public string ConnectString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Extension { get; set; }
    }
}
