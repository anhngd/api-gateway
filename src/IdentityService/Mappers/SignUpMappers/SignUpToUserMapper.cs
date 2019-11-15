using System;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.ViewModels.SignUpViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Mappers.SignUpMappers
{
    public class SignUpToUserMapper :
        IMapper<SignUp, ApplicationUser>
    {
        private readonly IUrlHelper _urlHelper;

        public SignUpToUserMapper(IUrlHelper urlHelper) => _urlHelper = urlHelper;

        public void Map(SignUp source, ApplicationUser destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            
            destination.Email = source.Email;
            destination.FamilyName = source.FamilyName;
            destination.GivenName = source.GivenName;
            destination.UserName = source.Email;
        }
    }
}
