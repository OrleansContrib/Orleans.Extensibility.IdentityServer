using AutoMapper;
using Orleans.Extensibility.IdentityServer.Grains;

namespace Orleans.Extensibility.IdentityServer.Mappers
{
    internal static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg =>cfg.AddProfile<PersistedGrantMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        internal static IdentityServer4.Models.PersistedGrant ToModel(this PersistedGrant token)
        {
            return token == null ? null : Mapper.Map<IdentityServer4.Models.PersistedGrant>(token);
        }

        internal static PersistedGrant ToEntity(this IdentityServer4.Models.PersistedGrant token)
        {
            return token == null ? null : Mapper.Map<PersistedGrant>(token);
        }

        internal static void UpdateEntity(this PersistedGrant token, PersistedGrant target)
        {
            Mapper.Map(token, target);
        }
    }
}