// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                // ensure roles
                EnsureRoles(scope.ServiceProvider);

                // ensure root account
                EnsureRoot(scope.ServiceProvider);

                // // ensure test users
                // EnsureTestUsers(scope.ServiceProvider);

            }
        }

        private static void EnsureRoles(IServiceProvider services)
        {
            var roleManager = services.GetService<RoleManager<IdentityRole>>();

            var roles = new[] {
                RoleNames.Root,
                RoleNames.Admin,
                RoleNames.Partner,
                RoleNames.User
            };

            foreach (var roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var identityRole = new IdentityRole {Name = roleName};

                    var roleResult = roleManager.CreateAsync(identityRole).Result;
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Error creating role '{roleName}'");
                    }
                }
                else
                {
                    Console.WriteLine($"Role '{roleName}' already exists");
                }
            }
        }

        private static void EnsureRoot(IServiceProvider services)
        {
            var userMgr = services.GetService<UserManager<ApplicationUser>>();

            var root = userMgr.FindByNameAsync("root").Result;

            // if(root != null) {
            //     var result = userMgr.DeleteAsync(root).Result;
            // }

            if (root == null)
            {
                root = new ApplicationUser
                {
                    UserName = "root",
                    EmailConfirmed = true
                };

                var result = userMgr.CreateAsync(root, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                // result = userMgr.AddClaimsAsync(root, new Claim[]{
                //     new Claim(JwtClaimTypes.Name, "root"),
                //     //new Claim(JwtClaimTypes.GivenName, "Bob"),
                //     //new Claim(JwtClaimTypes.FamilyName, "Smith"),
                //     //new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                //     //new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                //     //new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                //     //new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                //     //new Claim("location", "somewhere")
                // }).Result;

                // if (!result.Succeeded)
                // {
                //     throw new Exception(result.Errors.First().Description);
                // }

                // add to role "Root"
                result = userMgr.AddToRoleAsync(root, RoleNames.Root).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Console.WriteLine("root created");
            }
            else
            {
                Console.WriteLine("root already exists");
            }
        }

        private static void EnsureTestUsers(IServiceProvider services)
        {
            var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
            var alice = userMgr.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    UserName = "alice"
                };
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Console.WriteLine("alice created");
            }
            else
            {
                Console.WriteLine("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob"
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Console.WriteLine("bob created");
            }
            else
            {
                Console.WriteLine("bob already exists");
            }
        }
    }
}
