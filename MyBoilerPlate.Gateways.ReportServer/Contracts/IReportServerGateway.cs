using MyBoilerPlate.Gateways.ReportServer.Enums;
using MyBoilerPlate.Gateways.ReportServer.Responses;
using System.Collections.Generic;

namespace MyBoilerPlate.Gateways.ReportServer.Contracts
{
    public interface IReportServerGateway
    {
        RenderResponse RenderReport(string reportPath, Dictionary<string, string> parameters, string dataSourceName, bool overrideCredentials = true, ReportFormats format = ReportFormats.PDF);
    }
}
