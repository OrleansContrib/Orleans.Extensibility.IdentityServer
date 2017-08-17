using Orleans.Extensibility.IdentityServer.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdSrvExtensions
    {
        public static IIdentityServerBuilder AddOrleansProfileStore(this IIdentityServerBuilder builder)
        {
            builder.AddProfileService<OrleansProfileService>();

            return builder;
        }
    }
}