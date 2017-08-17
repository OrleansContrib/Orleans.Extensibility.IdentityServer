using System;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.Extensibility.IdentityServer.Grains
{
    internal class UserProfileGrain : Grain<UserProfileState>, IUserProfileGrain
    {
        public Task Create(string email, string username)
        {
            if (State.Profile != null) throw new InvalidOperationException("Profile already exist.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            State.Profile = new UserProfile { SubjectId = this.GetPrimaryKeyString(), Email = email, UserName = username };
            return WriteStateAsync();
        }

        public Task<UserProfile> GetProfileData() => Task.FromResult(State.Profile);

        public Task SetClaim(string claim, string value)
        {
            if (string.IsNullOrWhiteSpace(claim)) throw new ArgumentNullException(nameof(claim));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (State.Profile == null) throw new InvalidOperationException("Profile doesn't exist.");

            State.Profile.Claims[claim] = value;
            return WriteStateAsync();
        }

        public async Task RemoveClaim(string claim)
        {
            if (string.IsNullOrWhiteSpace(claim)) throw new ArgumentNullException(nameof(claim));
            if (State.Profile == null) throw new InvalidOperationException("Profile doesn't exist.");

            if (State.Profile.Claims.Remove(claim))
            {
                await WriteStateAsync();
            }
        }
    }
}