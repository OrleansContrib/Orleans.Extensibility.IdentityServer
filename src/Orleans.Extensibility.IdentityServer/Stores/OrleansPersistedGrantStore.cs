using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Orleans.Extensibility.IdentityServer.Mappers;

namespace Orleans.Extensibility.IdentityServer.Stores
{
    public class OrleansPersistedGrantStore : IPersistedGrantStore
    {
        private readonly IClusterClient _clusterClient;

        public OrleansPersistedGrantStore(IClusterClient clusterClient)
        {
            if (clusterClient == null) throw new ArgumentNullException(nameof(clusterClient));
            _clusterClient = clusterClient;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId) => (await _clusterClient.GetGrain<Grains.ISubjectGrantCollectionGrain>(subjectId).GetAllGrants()).Select(g => g.ToModel());

        public async Task<PersistedGrant> GetAsync(string key) => (await _clusterClient.GetGrain<Grains.IPersistedGrantGrain>(key).GetData()).ToModel();

        public Task RemoveAllAsync(string subjectId, string clientId) => _clusterClient.GetGrain<Grains.ISubjectGrantCollectionGrain>(subjectId).RemoveAllGrants(clientId);

        public Task RemoveAllAsync(string subjectId, string clientId, string type) => _clusterClient.GetGrain<Grains.ISubjectGrantCollectionGrain>(subjectId).RemoveAllGrants(clientId, type);

        public Task RemoveAsync(string key) => _clusterClient.GetGrain<Grains.IPersistedGrantGrain>(key).Remove();

        public Task StoreAsync(PersistedGrant grant) => _clusterClient.GetGrain<Grains.ISubjectGrantCollectionGrain>(grant.SubjectId).CreateGrant(grant.ToEntity());
    }
}