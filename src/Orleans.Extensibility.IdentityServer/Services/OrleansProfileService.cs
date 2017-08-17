using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Orleans.Extensibility.IdentityServer.Grains;

namespace Orleans.Extensibility.IdentityServer.Services
{
    public class OrleansProfileService : IProfileService
    {
        private readonly IClusterClient _clusterClient;

        public OrleansProfileService(IClusterClient clusterClient)
        {
            if (clusterClient == null) throw new ArgumentNullException(nameof(clusterClient));
            _clusterClient = clusterClient;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var profileGrain = _clusterClient.GetGrain<IUserProfileGrain>(sub);
            var profile = await profileGrain.GetProfileData();
            if (profile == null) throw new InvalidOperationException("Profile not found.");

            var claims = new List<Claim>();
            context.IssuedClaims = profile.Claims.Select(c => new Claim(c.Key, c.Value)).ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var profileGrain = _clusterClient.GetGrain<IUserProfileGrain>(sub);
            var profile = await profileGrain.GetProfileData();
            context.IsActive = profile != null;
        }
    }
}