
using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Orleans.Extensibility.IdentityServer.Grains;

namespace Orleans.Extensibility.IdentityServer.Stores
{
    public class OrleansClientStore : IClientStore
    {
        private readonly IClusterClient _clusterClient;

        public OrleansClientStore(IClusterClient clusterClient)
        {
            if (clusterClient == null) throw new ArgumentNullException(nameof(clusterClient));
            _clusterClient = clusterClient;
        }

        public Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            return _clusterClient.GetGrain<IClientGrain>(clientId).GetClientData();
        }
    }
}