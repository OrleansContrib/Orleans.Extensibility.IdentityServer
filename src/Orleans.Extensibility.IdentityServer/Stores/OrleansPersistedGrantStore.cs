using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

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

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId) => _clusterClient.GetGrain<ISubjectGrantCollectionGrain>(subjectId).GetAllGrants();

        public Task<PersistedGrant> GetAsync(string key) => _clusterClient.GetGrain<IPersistedGrantGrain>(key).GetData();

        public Task RemoveAllAsync(string subjectId, string clientId) => _clusterClient.GetGrain<ISubjectGrantCollectionGrain>(subjectId).RemoveAllGrants(clientId);

        public Task RemoveAllAsync(string subjectId, string clientId, string type) => _clusterClient.GetGrain<ISubjectGrantCollectionGrain>(subjectId).RemoveAllGrants(clientId, type);

        public Task RemoveAsync(string key) => _clusterClient.GetGrain<IPersistedGrantGrain>(key).Remove();

        public Task StoreAsync(PersistedGrant grant) => _clusterClient.GetGrain<ISubjectGrantCollectionGrain>(grant.SubjectId).CreateGrant(grant);
    }
}