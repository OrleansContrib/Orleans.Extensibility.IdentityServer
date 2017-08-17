
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Orleans.Extensibility.IdentityServer.Grains
{
    public interface IClientGrain : IGrainWithStringKey
    {
        Task Create(IdentityServer4.Models.Client client);
        Task<IdentityServer4.Models.Client> GetClientData();
    }
}