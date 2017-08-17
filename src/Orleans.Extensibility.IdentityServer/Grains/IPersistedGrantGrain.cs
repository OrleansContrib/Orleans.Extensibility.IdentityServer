using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Extensibility.IdentityServer.Grains
{
    //Key == Grant Key
    internal interface IPersistedGrantGrain : IGrainWithStringKey
    {
        Task Create(ISubjectGrantCollectionGrain collection, PersistedGrant grant);
        Task<PersistedGrant> GetData();
        Task Remove();
    }

    //Key == SubjectId
    public interface ISubjectGrantCollectionGrain : IGrainWithStringKey
    {
        Task CreateGrant(PersistedGrant grant);
        Task RemoveAllGrants();
        Task RemoveAllGrants(string clientId);
        Task RemoveAllGrants(string clientId, string type);
        Task RemoveGrant(PersistedGrant grant);
        Task<IEnumerable<PersistedGrant>> GetAllGrants();
    }
}