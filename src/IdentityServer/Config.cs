using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                 new ApiResource()
                {
                    Name = "api_admin",
                    DisplayName = "Admin API",
                    Description = "Admin API Access",
                    UserClaims = new List<string> {"role"},
                    Scopes = new List<Scope>
                    {
                        new Scope("admin:root"),
                        new Scope("admin:admin"),
                        new Scope("admin:user"),
                    }
                },
                new ApiResource()
                {
                    Name = "account_api",
                    DisplayName = "Account API",
                    Description = "Account API Access",
                    UserClaims = new List<string> {"role"},
                    Scopes = new List<Scope>
                    {
                        new Scope("account:root"),
                        new Scope("account:admin"),
                        new Scope("account:user"),
                    }
                },
                new ApiResource()
                {
                    Name = "trip_api",
                    DisplayName = "Trip API",
                    Description = "Trip API Access",
                    UserClaims = new List<string> {"role"},
                    //ApiSecrets =  new List<Secret>
                    //{
                    //    new Secret("e79ff478fe924cd09663e384fc30bee2".Sha256())
                    //},
                    Scopes = new List<Scope>
                    {
                        new Scope("admin:trip:root"),
                        new Scope("admin:trip:admin"),
                        new Scope("admin:trip:user"),
                    }
                },
                new ApiResource()
                {
                    Name = "catalog_api",
                    DisplayName = "Catalog API",
                    Description = "Catalog API Access",
                    UserClaims = new List<string> {"role"},
                    Scopes = new List<Scope>
                    {
                        new Scope("admin:catalog:root"),
                        new Scope("admin:catalog:admin"),
                        new Scope("admin:catalog:user"),
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "tripbricks.admin",
                    ClientName = "Tripbricks Administration",
                    ClientUri = "https://admin.tripbricks.com",

                    AllowedGrantTypes = new[] { "implicit", "password"},
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("44F71137B3B448D7B14C698B42B1FFDF".Sha256()),
                        new Secret("5666867FFEFA4561B30D10AC5AE10427".Sha256()),
                        new Secret("F5AF8ABE42CA44D3896BCE1C17FB443E".Sha256()),
                        new Secret("59035AD08FFE4CC6B7467657A06CF34C".Sha256())
                    },

                    RedirectUris =
                    {
                        "https://admin.tripbricks.com/auth/authorized",
                        "https://admin.tripbricks.com/auth/authorized",
                        "https://admin.tripbricks.com/auth/callback",
                        "https://admin.tripbricks.com/auth/callback"
                    },

                    PostLogoutRedirectUris = { "https://admin.tripbricks.com/auth/authorized" },
                    AllowedCorsOrigins = { "https://admin.tripbricks.com" },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "role",
                        "admin:account:root",
                        "admin:account:admin",
                        "admin:trip:root",
                        "admin:trip:admin",
                        "admin:catalog:root",
                        "admin:catalog:admin",
                        "offline_access"
                    }
                },
                // admin.tripbricks.com test
                new Client
                {
                    ClientId = "tripbricks.admin.dev",
                    ClientName = "Tripbricks Administration (dev mode)",
                    ClientUri = "http://localhost:3300",

                    AllowedGrantTypes = new[] { "implicit", "password"},
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("44F71137B3B448D7B14C698B42B1FFDF".Sha256()),
                        new Secret("5666867FFEFA4561B30D10AC5AE10427".Sha256()),
                        new Secret("F5AF8ABE42CA44D3896BCE1C17FB443E".Sha256()),
                        new Secret("59035AD08FFE4CC6B7467657A06CF34C".Sha256())
                    },

                    RedirectUris =
                    {
                        "http://localhost:3300/auth/authorized",
                        "http://localhost:3300/auth/authorized",
                        "http://localhost:3300/auth/callback",
                        "http://localhost:3300/auth/callback"
                    },

                    PostLogoutRedirectUris = { "http://localhost:3300/auth/authorized" },
                    AllowedCorsOrigins = { "http://localhost:3300" },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "role",
                        "offline_access",
                        "account:root",
                        "account:admin",
                        "account:user",
                        "admin:trip:root",
                        "admin:trip:admin",
                        "admin:trip:onlineuser",
                        "admin:catalog:root",
                        "admin:catalog:admin",
                        "admin:root",
                        "admin:admin",
                        "admin:user"
                    }
                },
                new Client
                {
                    ClientId = "tripbricks.admin.dev1",
                    ClientName = "Tripbricks Administration (dev mode)",
                    ClientUri = "http://localhost:3301",

                    AllowedGrantTypes = new[] { "implicit", "password"},
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("44F71137B3B448D7B14C698B42B1FFDF".Sha256()),
                        new Secret("5666867FFEFA4561B30D10AC5AE10427".Sha256()),
                        new Secret("F5AF8ABE42CA44D3896BCE1C17FB443E".Sha256()),
                        new Secret("59035AD08FFE4CC6B7467657A06CF34C".Sha256())
                    },

                    RedirectUris =
                    {
                        "http://localhost:3301/auth/authorized",
                        "http://localhost:3301/auth/authorized",
                        "http://localhost:3301/auth/callback",
                        "http://localhost:3301/auth/callback"
                    },

                    PostLogoutRedirectUris = { "http://localhost:3301/auth/authorized" },
                    AllowedCorsOrigins = { "http://localhost:3301" },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "role",
                        "admin:account:root",
                        "admin:account:admin",
                        "admin:account:onlineuser",
                        "admin:trip:root",
                        "admin:trip:admin",
                        "admin:trip:onlineuser",
                        "admin:catalog:root",
                        "admin:catalog:admin",
                        "admin:catalog:onlineuser",
                        "offline_access"
                    }
                },
                new Client
                {
                    ClientId = "tripbricks.dev",
                    ClientName = "Tripbricks (dev mode)",
                    ClientUri = "http://localhost:3000",

                    AllowedGrantTypes = new[] { "implicit", "password"},
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("44F71137B3B448D7B14C698B42B1FFDF".Sha256()),
                        new Secret("5666867FFEFA4561B30D10AC5AE10427".Sha256()),
                        new Secret("F5AF8ABE42CA44D3896BCE1C17FB443E".Sha256()),
                        new Secret("59035AD08FFE4CC6B7467657A06CF34C".Sha256())
                    },

                    RedirectUris =
                    {
                        "http://localhost:3000/auth/authorized",
                        "http://localhost:3000/auth/authorized",
                        "http://localhost:3000/auth/callback",
                        "http://localhost:3000/auth/callback"
                    },

                    PostLogoutRedirectUris = { "http://localhost:3000/auth/authorized" },
                    AllowedCorsOrigins = { "http://localhost:3000" },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "role",
                        "offline_access",
                        "onlineuser",
                    }
                },
            };
        }
    }
}