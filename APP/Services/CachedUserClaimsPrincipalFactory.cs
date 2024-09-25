using CodeData_Connection.Areas.Identity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CodeData_Connection.Services
{
    public class CachedUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly IMemoryCache _cache;

        public CachedUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor, IMemoryCache cache) : base(userManager, optionsAccessor)
        {
            _cache = cache;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            return await _cache.GetOrCreateAsync($"UserIdentity-{user.Id}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                return await base.CreateAsync(user);
            });
        }
    }
}
