using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.Extensibility.IdentityServer.Grains
{
    internal class PersistedGrantGrain : Grain<PersistedGrantState>, IPersistedGrantGrain
    {
        public Task Create(ISubjectGrantCollectionGrain collection, PersistedGrant grant)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (grant == null) throw new ArgumentNullException(nameof(grant));

            State.Collection = collection;
            State.Grant = grant;
            return WriteStateAsync();
        }

        public async Task Remove()
        {
            await State.Collection.RemoveGrant(this.State.Grant);
            State.Collection = null;
            State.Grant = null;
            await ClearStateAsync();
            DeactivateOnIdle();
        }

        Task<PersistedGrant> IPersistedGrantGrain.GetData() => Task.FromResult(State.Grant);
    }

    internal class SubjectGrantCollectionGrain : Grain<SubjectGrantCollectionState>, ISubjectGrantCollectionGrain
    {
        public async Task CreateGrant(PersistedGrant grant)
        {
            var grantGrain = GrainFactory.GetGrain<IPersistedGrantGrain>(grant.Key);
            await grantGrain.Create(this, grant);
            State.ByClientId[grant.ClientId] = grantGrain;
            State.ByClientIdAndType[new Tuple<string, string>(grant.ClientId, grant.Type)] = grantGrain;
            await WriteStateAsync();
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllGrants()
        {
            return (await Task.WhenAll(State.ByClientId.Values.Select(async g => (await g.GetData())))).ToList();
        }

        public async Task RemoveAllGrants()
        {
            await Task.WhenAll(State.ByClientId.Values.Select(async g =>
                {
                    var grant = await g.GetData();
                    if (grant != null)
                    {
                        State.ByClientId.Remove(grant.ClientId);
                        State.ByClientIdAndType.Remove(new Tuple<string, string>(grant.ClientId, grant.Type));
                    }
                    await g.Remove();
                }));
            await WriteStateAsync();
        }

        public async Task RemoveAllGrants(string clientId)
        {
            var grantGrain = State.ByClientId[clientId];
            if (grantGrain != null)
            {
                var grant = await grantGrain.GetData();
                if (grant != null)
                {
                    State.ByClientIdAndType.Remove(new Tuple<string, string>(clientId, grant.Type));
                }
                State.ByClientId.Remove(clientId);
                await grantGrain.Remove();
                await WriteStateAsync();
            }
        }

        public async Task RemoveAllGrants(string clientId, string type)
        {
            var key = new Tuple<string, string>(clientId, type);
            var grantGrain = State.ByClientIdAndType[key];
            if (grantGrain != null)
            {
                State.ByClientIdAndType.Remove(key);
                await grantGrain.Remove();
            }

            State.ByClientId.Remove(clientId);
            await WriteStateAsync();
        }

        public Task RemoveGrant(PersistedGrant grant)
        {
            State.ByClientId.Remove(grant.ClientId);
            State.ByClientIdAndType.Remove(new Tuple<string, string>(grant.ClientId, grant.Type));
            return WriteStateAsync();
        }
    }
}