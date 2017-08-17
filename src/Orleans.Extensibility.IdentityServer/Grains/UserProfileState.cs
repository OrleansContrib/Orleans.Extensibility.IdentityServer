using System;
using System.Collections.Generic;

namespace Orleans.Extensibility.IdentityServer.Grains
{
    internal class UserProfileState
    {
        public UserProfile Profile { get; set; }
    }

    public class UserProfile
    {
        public string SubjectId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    }
}