using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer
{
    public class DefaultDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public DefaultDataProtectorTokenProvider(
            IDataProtectionProvider dataProtectionProvider, 
            IOptions<DefaultDataProtectorTokenProviderOptions> options) 
            : base(dataProtectionProvider, options)
        {
        }
    }

    public class DefaultDataProtectorTokenProviderOptions : DataProtectionTokenProviderOptions { }
}
