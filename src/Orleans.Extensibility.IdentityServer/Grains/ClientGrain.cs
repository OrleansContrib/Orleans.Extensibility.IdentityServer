
using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Orleans.Extensibility.IdentityServer.Mappers;

namespace Orleans.Extensibility.IdentityServer.Grains
{
    internal class ClientGrain : Grain<ClientState>, IClientGrain
    {
        public Task<IdentityServer4.Models.Client> GetClientData() => Task.FromResult(State.Client?.ToModel());

        public Task Create(IdentityServer4.Models.Client client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            State.Client = client.ToEntity();
            return WriteStateAsync();
        }
    }
}