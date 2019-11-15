using System;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.Services;
using IdentityService.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Mappers.AdminMappers
{
    public class AdminToAdminMapper :
        IMapper<ApplicationUser, Admin>,
        IMapper<CreateAdmin, ApplicationUser>,
        IMapper<UpdateAdmin, ApplicationUser>
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IClockService _clockService;

        public AdminToAdminMapper(IUrlHelper urlHelper, IClockService clockService) { 
            _urlHelper = urlHelper;
            _clockService = clockService;

        } 

        public void Map(ApplicationUser source, Admin destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Role = source.Role.Name;
            destination.Address = source.Address;
            destination.BirthDate = source.BirthDate;
            destination.Email = source.Email;
            destination.EmailConfirmed = source.EmailConfirmed;
            destination.FamilyName = source.FamilyName;
            destination.Gender = source.Gender;
            destination.GivenName = source.GivenName;
            destination.Id = source.Id;
            destination.Locale = source.Locale;
            if (source.LockoutEnd != null)
            {
                destination.Lockedout = source.LockoutEnabled;
                destination.LockoutEnd = source.LockoutEnd;
            }
            destination.PhoneNumber = source.PhoneNumber;
            destination.PhoneNumberConfirmed = source.PhoneNumberConfirmed;
            destination.Picture = source.Picture;
            destination.UpdatedAt = source.UpdatedAt;
            destination.UserName = source.UserName;
            destination.ZoneInfo = source.ZoneInfo;
            destination.CreatedAt = source.CreatedAt;
        }

        public void Map(CreateAdmin source, ApplicationUser destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            var now = _clockService.UtcNow;

            if (destination.CreatedAt == null)
            {
                destination.CreatedAt = now;
            }
            destination.UpdatedAt = now;

            destination.FamilyName = source.FamilyName;
            destination.GivenName = source.GivenName;
            destination.Gender = source.Gender;
            destination.Picture = source.Picture;
            destination.BirthDate = source.BirthDate;
            destination.ZoneInfo = source.ZoneInfo;
            destination.Locale = source.Locale;
            destination.Address = source.Address;
            destination.PhoneNumberConfirmed = source.PhoneNumberConfirmed;
            destination.PhoneNumber = source.PhoneNumber;
            destination.EmailConfirmed = source.EmailConfirmed;
            destination.Email = source.Email;
            destination.UserName = source.UserName;
            if (source.LockoutEnd != null)
            {
                destination.LockoutEnabled = source.Lockedout;
                destination.LockoutEnd = source.LockoutEnd;
            }
            //destination.PasswordHash = source.Password;
        }

        public void Map(UpdateAdmin source, ApplicationUser destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            var now = _clockService.UtcNow;

            if (destination.CreatedAt == null)
            {
                destination.CreatedAt = now;
            }
            destination.UpdatedAt = now;

            destination.FamilyName = source.FamilyName;
            destination.GivenName = source.GivenName;
            destination.Gender = source.Gender;
            //destination.Picture = source.Picture;
            destination.BirthDate = source.BirthDate;
            destination.ZoneInfo = source.ZoneInfo;
            destination.Locale = source.Locale;
            destination.Address = source.Address;
            //destination.UpdateAt = source.UpdateAt;
            //destination.PhoneNumberConfirmed = source.PhoneNumberConfirmed;
            destination.PhoneNumber = source.PhoneNumber;
            //destination.EmailConfirmed = source.EmailConfirmed;
            //destination.Email = source.Email;
            //destination.UserName = source.UserName;
            //destination.Id = source.Id;
            //destination.LockoutEnabled = source.Lockedout;
            //destination.LockoutEnd = source.LockoutEnd;
            //destination.PasswordHash = source.Password;
        }
    }
}
