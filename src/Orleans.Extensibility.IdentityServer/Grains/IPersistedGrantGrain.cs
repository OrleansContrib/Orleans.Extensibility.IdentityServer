using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Extensibility.IdentityServer
{
    //Key == Grant Key
    internal interface IPersistedGrantGrain : IGrainWithStringKey
    {
        Task Create(ISubjectGrantCollectionGrain collection, IdentityServer4.Models.PersistedGrant grant);
        Task<IdentityServer4.Models.PersistedGrant> GetData();
        Task Remove();
    }

    //Key == SubjectId
    public interface ISubjectGrantCollectionGrain : IGrainWithStringKey
    {
        Task CreateGrant(IdentityServer4.Models.PersistedGrant grant);
        Task RemoveAllGrants();
        Task RemoveAllGrants(string clientId);
        Task RemoveAllGrants(string clientId, string type);
        Task RemoveGrant(IdentityServer4.Models.PersistedGrant grant);
        Task<IEnumerable<IdentityServer4.Models.PersistedGrant>> GetAllGrants();
    }
}