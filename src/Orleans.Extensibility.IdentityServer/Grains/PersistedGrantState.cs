using System;
using System.Collections.Generic;

namespace Orleans.Extensibility.IdentityServer.Grains
{
    internal class PersistedGrantState
    {
        public ISubjectGrantCollectionGrain Collection { get; set; }
        public PersistedGrant Grant { get; set; }
    }

    public class PersistedGrant
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Expiration { get; set; }
        public string Data { get; set; }
    }

    internal class SubjectGrantCollectionState
    {
        public Dictionary<string, IPersistedGrantGrain> ByClientId { get; set; } = new Dictionary<string, IPersistedGrantGrain>();
        public Dictionary<Tuple<string, string>, IPersistedGrantGrain> ByClientIdAndType { get; set; } = new Dictionary<Tuple<string, string>, IPersistedGrantGrain>();
    }
}