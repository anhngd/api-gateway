using System;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.Commands.AccountCommands;
using IdentityService.Commands.ActivityHistoryCommands;
using IdentityService.Commands.AdminCommands;
using IdentityService.Commands.RoleCommands;
using IdentityService.Commands.UserCommands;
using IdentityService.Mappers.AccountMappers;
using IdentityService.Mappers.AdminMappers;
using IdentityService.Mappers.SignUpMappers;
using IdentityService.Mappers.UserMappers;
using IdentityService.Repositories;
using IdentityService.Services;
using IdentityService.ViewModels.AccountViewModels;
using IdentityService.ViewModels.AdminViewModels;
using IdentityService.ViewModels.SignUpViewModels;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods add project services.
    /// </summary>
    /// <remarks>
    /// AddScoped - Only one instance is ever created and returned.
    /// AddScoped - A new instance is created and returned for each request/response cycle.
    /// AddTransient - A new instance is created and returned each time.
    /// </remarks>
    public static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services
                // ADD Service for Library
                .AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>()
                .AddScoped<IDisposable, UserManager<ApplicationUser>>()
                // AccountCommands
                .AddScoped<IGetAllAccountCommand, GetAllAccountCommand>()
                .AddScoped<IPostAccountCommand, PostAccountCommand>()
                .AddScoped<IGetMyAccountCommand, GetMyAccountCommand>()
                .AddScoped<IPutMyAccountCommand, PutMyAccountCommand>()
                .AddScoped<IGetAccountCommand, GetAccountCommand>()
                .AddScoped<IPutAccountCommand, PutAccountCommand>()
                .AddScoped<IDeleteAccountCommand, DeleteAccountCommand>()
                .AddScoped<IPostAccountNoteCommand, PostAccountNoteCommand>()
                .AddScoped<IPostAccountStatusCommand, PostAccountStatusCommand>()
                .AddScoped<IPutAccountUserCommand, PutAccountUserCommand>()
                .AddScoped<IDeleteAccountUserCommand, DeleteAccountUserCommand>()
                .AddScoped<IPutAccountLockCommand, PutAccountLockCommand>()
                .AddScoped<IPutAccountUnLockCommand, PutAccountUnLockCommand>()
                // AdminCommands
                .AddScoped<IGetAllAdminCommand, GetAllAdminCommand>()
                .AddScoped<IPostAdminCommand, PostAdminCommand>()
                .AddScoped<IGetAdminCommand, GetAdminCommand>()
                .AddScoped<IPutAdminCommand, PutAdminCommand>()
                .AddScoped<IDeleteAdminCommand, DeleteAdminCommand>()
                .AddScoped<IPutAdminGroupCommand, PutAdminGroupCommand>()
                .AddScoped<IPutAdminPermissionCommand, PutAdminPermissionCommand>()
                .AddScoped<IPutAdminUserCommand, PutAdminUserCommand>()
                .AddScoped<IDeleteAdminUserCommand, DeleteAdminUserCommand>()
                .AddScoped<IPutAdminLockCommand, PutAdminLockCommand>()
                .AddScoped<IPutAdminUnLockCommand, PutAdminUnLockCommand>()
                // RoleCommands //
                .AddScoped<IGetRoleCommand, GetRoleCommand>()
                .AddScoped<IPutUserRoleCommand, PutUserRoleCommand>()

                //// EmailTemplateCommands
                //.AddScoped<IGetConfirmNewAccountCommand, GetConfirmNewAccountCommand>()
                //.AddScoped<IPutConfirmNewAccountCommand, PutConfirmNewAccountCommand>()
                //.AddScoped<IGetResetPasswordCommand, GetResetPasswordCommand>()
                //.AddScoped<IPutResetPasswordCommand, PutResetPasswordCommand>()
                // SignUpCommands
                .AddScoped<IPostSignUpCommand, PostSignUpCommand>()
                .AddScoped<IPostConfirmEmailCommand, PostConfirmEmailCommand>()
                // UserCommands
                .AddScoped<IGetAllUserCommand, GetAllUserCommand>()
                .AddScoped<IPostUserCommand, PostUserCommand>()
                .AddScoped<IGetMyUserCommand, GetMyUserCommand>()
                .AddScoped<IPutMyUserCommand, PutMyUserCommand>()
                .AddScoped<IPutMyPassUserCommand, PutMyPassUserCommand>()
                .AddScoped<IGetUserCommand, GetUserCommand>()
                .AddScoped<IPutUserCommand, PutUserCommand>()
                .AddScoped<IDeleteUserCommand, DeleteUserCommand>()
                .AddScoped<IPutPassUserCommand, PutPassUserCommand>()
                .AddScoped<IPutLockUserCommand, PutLockUserCommand>()
                .AddScoped<IPutUnLockUserCommand, PutUnLockUserCommand>()

                .AddScoped<IPostForgotPasswordCommand, PostForgotPasswordCommand>()
                .AddScoped<IGetResetPasswordCommand, GetResetPasswordCommand>()
                .AddScoped<IPostResetPasswordCommand, PostResetPasswordCommand>()
                // LoginHistoryCommands
                .AddScoped<IGetAllActivityHistoryCommand, GetAllActivityHistoryCommand>()
                .AddScoped<IGetMyActivityHistoryCommand, GetMyActivityHistoryCommand>()
                .AddScoped<IGetActivityHistoryByIdCommand, GetActivityHistoryByIdCommand>()
                .AddScoped<IGetActivityHistoryByUserIdCommand, GetActivityHistoryByUserIdCommand>()
                .AddScoped<IPostActivityHistoryCommand, PostActivityHistoryCommand>();
                // END AddProjectCommands

        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services
                // AccountMapper
                .AddScoped<IMapper<ApplicationUser, Account>, AccountToAccountMapper>()
                .AddScoped<IMapper<CreateAccount, ApplicationUser>, AccountToAccountMapper>()
                .AddScoped<IMapper<UpdateAccount, ApplicationUser>, AccountToAccountMapper>()
                // AdminMapper
                .AddScoped<IMapper<ApplicationUser, Admin>, AdminToAdminMapper>()
                .AddScoped<IMapper<CreateAdmin, ApplicationUser>, AdminToAdminMapper>()
                .AddScoped<IMapper<UpdateAdmin, ApplicationUser>, AdminToAdminMapper>()
                // UserMapper
                .AddScoped<IMapper<ApplicationUser, User>, UserToUserMapper>()
                .AddScoped<IMapper<CreateUser, ApplicationUser>, UserToUserMapper>()
                .AddScoped<IMapper<UpdateUser, ApplicationUser>, UserToUserMapper>()
                // SignUpMapper
                .AddScoped<IMapper<SignUp, ApplicationUser>, SignUpToUserMapper>();

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddScoped<IUserRepository, UserRepository>();

        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddScoped<IClockService, ClockService>();
    }
}
