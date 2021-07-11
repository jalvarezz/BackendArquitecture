
namespace Core.Common.Contracts
{
    public interface IGatewayFactory
    {
        T GetGateway<T>(string name) where T : IGateway;
    }
}
