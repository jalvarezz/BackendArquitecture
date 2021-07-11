using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBoilerPlate.Gateways.NeverBounce.Contracts
{
    public interface INeverBounceGateway
    {
        Task<string> CheckAsync(string email);

        Task<List<string>> CheckAsync(List<string> emails);
    }
}
