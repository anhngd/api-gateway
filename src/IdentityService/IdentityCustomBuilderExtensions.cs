using System;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService
{
    public static class IdentityCustomBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCtx"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
		public static IServiceCollection AddIdentityManagers<TCtx, TUser, TRole>(
            this IServiceCollection services
        ) where TRole : class where TUser : class
        {
            // Identity services
            services.AddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.AddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.AddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.AddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.AddScoped<IdentityErrorDescriber>();
            services.AddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            // services.AddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<TUser>>();
            services.AddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
            services.AddScoped<UserManager<TUser>>();
            services.AddScoped<SignInManager<TUser>>();
            services.AddScoped<RoleManager<TRole>>();

            AddStores(services, typeof(TUser), typeof(TRole), typeof(TCtx));

            AddDefaultTokenProviders(services, typeof(TUser));

            return services;
        }

        /// <summary>
        /// Adds the default token providers used to generate tokens for reset passwords, change email
        /// and change telephone number operations, and for two factor authentication token generation.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="userType"></param>
        private static void AddDefaultTokenProviders(IServiceCollection services, Type userType)
        {
            var dataProtectionProviderType = typeof(DataProtectorTokenProvider<>).MakeGenericType(userType);
            var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<>).MakeGenericType(userType);
            var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(userType);
            var authenticatorProviderType = typeof(AuthenticatorTokenProvider<>).MakeGenericType(userType);
            AddTokenProvider(services, TokenOptions.DefaultProvider, dataProtectionProviderType, userType);
            AddTokenProvider(services, TokenOptions.DefaultEmailProvider, emailTokenProviderType, userType);
            AddTokenProvider(services, TokenOptions.DefaultPhoneProvider, phoneNumberProviderType, userType);
            AddTokenProvider(services, TokenOptions.DefaultAuthenticatorProvider, authenticatorProviderType, userType);
        }

        private static void AddTokenProvider(IServiceCollection services, string providerName, Type provider, Type userType)
        {
            if (!typeof(IUserTwoFactorTokenProvider<>).MakeGenericType(userType).GetTypeInfo().IsAssignableFrom(provider.GetTypeInfo()))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Type {0} must derive from {1}&lt;{2}&gt;.", provider.Name, "IUserTokenProvider", userType.Name));
            }

            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap[providerName] = new TokenProviderDescriptor(provider);
            });
            services.AddTransient(provider);
        }

        private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type contextType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException("AddEntityFrameworkStores can only be called with a user that derives from IdentityUser<TKey>.");
            }

            var keyType = identityUserType.GenericTypeArguments[0];

            if (roleType != null)
            {
                var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
                if (identityRoleType == null)
                {
                    throw new InvalidOperationException("AddEntityFrameworkStores can only be called with a role that derives from TRole<TKey>.");
                }

                Type userStoreType;
                var identityContext = FindGenericBaseType(contextType, typeof(IdentityDbContext<,,,,,,,>));
                Type roleStoreType;
                if (identityContext == null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(UserStore<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
                    roleStoreType = typeof(RoleStore<,,>).MakeGenericType(roleType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(UserStore<,,,,,,,,>).MakeGenericType(userType, roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[5],
                        identityContext.GenericTypeArguments[7],
                        identityContext.GenericTypeArguments[6]);
                    roleStoreType = typeof(RoleStore<,,,,>).MakeGenericType(roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[6]);
                }
                services.AddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
                services.AddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
            }
            else
            {   // No Roles
                Type userStoreType = null;
                var identityContext = FindGenericBaseType(contextType, typeof(IdentityUserContext<,,,,>));
                if (identityContext == null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(UserOnlyStore<,,>).MakeGenericType(userType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(UserOnlyStore<,,,,,>).MakeGenericType(userType, contextType,
                        identityContext.GenericTypeArguments[1],
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4]);
                }
                services.AddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            }

        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
