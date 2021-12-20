using MyBoilerPlate.Gateways.ReportServer.Contracts;
using MyBoilerPlate.Gateways.ReportServer.Enums;
using MyBoilerPlate.Gateways.ReportServer.Execution;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace MyBoilerPlate.Gateways.ReportServer
{
    public class ReportServerGateway : IReportServerGateway
    {
        private readonly string _ServiceUrl;
        private readonly string _ExecutionUrl;
        private readonly string _UserName;
        private readonly string _Password;
        private readonly string _RootPath;

        private readonly string _DataSourceConnectString;
        private readonly string _DataSourceUserName;
        private readonly string _DataSourcePassword;
        private readonly string _DataSourceExtension;

        public ReportServerGateway(ReportServerSettings settings)
        {
            _ServiceUrl = settings.ServiceUrl;
            _ExecutionUrl = settings.ExecutionUrl;
            _UserName = settings.UserName;
            _Password = settings.Password;
            _RootPath = settings.RootPath;

            _DataSourceConnectString = settings.DataSource.ConnectString;
            _DataSourceUserName = settings.DataSource.UserName;
            _DataSourcePassword = settings.DataSource.Password;
            _DataSourceExtension = settings.DataSource.Extension;
        }

        public Responses.RenderResponse RenderReport(string reportPath,
            Dictionary<string, string> parameters,
            string dataSourceName,
            bool overrideCredentials = true,
            ReportFormats format = ReportFormats.PDF)
        {
            //Set the format
            string reportFormat = null;

            switch(format)
            {
                case ReportFormats.XML:
                    reportFormat = "XML";
                    break;
                case ReportFormats.NULL:
                    reportFormat = "NULL";
                    break;
                case ReportFormats.CSV:
                    reportFormat = "CSV";
                    break;
                case ReportFormats.IMAGE:
                    reportFormat = "IMAGE";
                    break;
                case ReportFormats.PDF:
                    reportFormat = "PDF";
                    break;
                case ReportFormats.HTML40:
                    reportFormat = "HTML4.0";
                    break;
                case ReportFormats.HTML32:
                    reportFormat = "HTML3.2";
                    break;
                case ReportFormats.MHTML:
                    reportFormat = "MHTML";
                    break;
                case ReportFormats.EXCEL:
                    reportFormat = "EXCEL";
                    break;
                case ReportFormats.Word:
                    reportFormat = "Word";
                    break;
                default:
                    reportFormat = "PDF";
                    break;
            }

            //Create the communication channel
            var channelFactory = CreateReportExecutionServiceFactory();
            var client = channelFactory.CreateChannel();

            //Setup the parameters
            Execution.ParameterValue[] reportParameters = null;

            if(parameters != null)
            {
                reportParameters = new Execution.ParameterValue[parameters.Count];

                for(int i = 0; i < reportParameters.Length; i++)
                {
                    reportParameters[i] = new Execution.ParameterValue();
                    reportParameters[i].Name = parameters.Keys.ElementAt(i);
                    reportParameters[i].Value = parameters.Values.ElementAt(i);
                }
            }
            else
                reportParameters = new Execution.ParameterValue[0];

            var loadedReport = client.LoadReport(new LoadReportRequest
            {
                Report = reportPath
            });


            if(overrideCredentials) /* °\(^▿^)/°  (ノ°ο°)ノ */
            {
                //**These lines are for setting the datasource of the report before rendering it**
                ReportServer.Execution.DataSourceCredentials datasetCredential = new ReportServer.Execution.DataSourceCredentials();
                datasetCredential.DataSourceName = dataSourceName;
                datasetCredential.UserName = _DataSourceUserName;
                datasetCredential.Password = _DataSourcePassword;

                ReportServer.Execution.DataSourceCredentials[] credentials = new ReportServer.Execution.DataSourceCredentials[1];
                credentials[0] = datasetCredential;

                client.SetExecutionCredentials(new SetExecutionCredentialsRequest(loadedReport.ExecutionHeader, new Execution.TrustedUserHeader(), credentials));

            }

            //Just set the parameters if there are some
            if(reportParameters.Length > 0)
                try
                {
                    client.SetExecutionParameters(new SetExecutionParametersRequest
                    {
                        Parameters = reportParameters,
                        ExecutionHeader = loadedReport.ExecutionHeader,
                        ParameterLanguage = "en-us"
                    });
                }catch(Exception e)
                {
                    if(e.Message.Contains("dataSource"))
                    {
                        reportParameters = reportParameters.Where(r => r.Name != "dataSource").ToArray();
                        client.SetExecutionParameters(new SetExecutionParametersRequest
                        {
                            Parameters = reportParameters,
                            ExecutionHeader = loadedReport.ExecutionHeader,
                            ParameterLanguage = "en-us"
                        });
                    }
                }

            //Build the render request
            var request = new RenderRequest()
            {
                Format = reportFormat,
                ExecutionHeader = loadedReport.ExecutionHeader
            };
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-ES");
            //Render the report
            var renderedReport = client.Render(request);
            //Close de communication channel
            channelFactory.Close();

            var result = new Responses.RenderResponse();

            result.Content = renderedReport.Result;
            result.ContentType = renderedReport.MimeType;
            result.Extension = renderedReport.Extension;
            return result;
        }

        private ChannelFactory<IReportExecutionService> CreateReportExecutionServiceFactory()
        {

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.AllowCookies = true;

            var reportExecutionEndpoint = new EndpointAddress(new Uri(_ExecutionUrl));
            ChannelFactory<IReportExecutionService> ReportExecutionServiceFactory = new ChannelFactory<IReportExecutionService>(binding, reportExecutionEndpoint);

            //Remove the default credentials
            var defaultCredentials = ReportExecutionServiceFactory.Endpoint.EndpointBehaviors.First() as ClientCredentials;
            ReportExecutionServiceFactory.Endpoint.EndpointBehaviors.Remove(defaultCredentials);

            //Create the credentials
            ClientCredentials credentials = new ClientCredentials();

            credentials.Windows.ClientCredential.UserName = _UserName;
            credentials.Windows.ClientCredential.Password = _Password;

            credentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

            ReportExecutionServiceFactory.Endpoint.EndpointBehaviors.Add(credentials);

            return ReportExecutionServiceFactory;
        }
    }
}
