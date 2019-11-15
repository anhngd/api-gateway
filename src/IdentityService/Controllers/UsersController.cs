using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer4.AccessTokenValidation;
using IdentityService.Commands.UserCommands;
using IdentityService.Constants;
using IdentityService.ViewModels;
using IdentityService.ViewModels.SignUpViewModels;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IdentityService.Controllers
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        ///  [Anonymous user] Register new account.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="signup">The information to register new account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 Ok response containing the newly created account or a 400 Bad Request if the information is invalid.</returns>
        [AllowAnonymous]
        [HttpPost("/api/signup", Name = SignUpsControllerRoute.PostSignUp)]
        [SwaggerResponse(StatusCodes.Status200OK, "Account was created.", typeof(User))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information is invalid.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Post(
            [FromServices] IPostSignUpCommand command,
            [FromBody] SignUp signup,
            CancellationToken cancellationToken) => command.ExecuteAsync(signup, cancellationToken);

        /// <summary>
        /// [Anonymous user] account.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="uid">ID of account to confirm.</param>
        /// <param name="token">Token of account to confirm, follow link in email.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 Ok response verify success.</returns>
        [AllowAnonymous]
        [HttpGet("/api/confirm", Name = SignUpsControllerRoute.PostConfirmEmail)]
        [SwaggerResponse(StatusCodes.Status200OK, "Confirm email successfully!")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "uid or token is invalid.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> ConfirmEmail(
            [FromServices] IPostConfirmEmailCommand command,
            string uid,
            string token,
            CancellationToken cancellationToken) => command.ExecuteAsync(uid, token, cancellationToken);

        /// <summary>
        /// Get a paginated list of all customer accounts. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="sort">Specifies sort order.
        /// Available values: _id, email, birth_date, family_name, name, gender, user_name
        /// Default value : _id</param>
        /// <param name="limit">Default value : 20</param>
        /// <param name="page">Default value : 1</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the account or a 404 Not Found if a account with id was not found.</returns>
        [HttpGet("/api/users", Name = UsersControllerRoute.GetAllUser)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "A paginated list of all customer accounts", typeof(ListUser))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A paginated list of all customer accounts could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetAll(
            [FromServices] IGetAllUserCommand command,
            string sort,
            int limit,
            int page,
            CancellationToken cancellationToken) => command.ExecuteAsync(sort, limit, page);

        /// <summary>
        /// Create a new customer account. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="model">The information of account to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response if created account or a 400 Bad Request if the infomation is invalid.</returns>
        [HttpPost("/api/users", Name = UsersControllerRoute.PostUser)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status201Created, "The account was created.", typeof(User))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information is invalid.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Post(
            [FromServices] IPostUserCommand command,
            [FromBody] CreateUser model,
            CancellationToken cancellationToken) => command.ExecuteAsync(model);

        /// <summary>
        /// Get the logged-in user's account details. [Any Role]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the account or a 404 Not Found if a account was not found.</returns>
        [HttpGet("/api/users/my", Name = UsersControllerRoute.GetMyUser)]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, "The account with the specified unique identifier.", typeof(User))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetMy(
            [FromServices] IGetMyUserCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync(cancellationToken);

        /// <summary>
        /// Update your account details. [Any Role]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="model">The information to update account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account, a 400 Bad Request if the information is invalid or a
        /// or a 404 Not Found if a account was not found.</returns>
        [Authorize]
        [HttpPut("/api/users/my", Name = UsersControllerRoute.PutMyUser)]
        [SwaggerResponse(StatusCodes.Status200OK, "Your account was updated.", typeof(User))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutMy(
            [FromServices] IPutMyUserCommand command,
            [FromBody] UpdateUser model,
            CancellationToken cancellationToken) => command.ExecuteAsync(model, cancellationToken);

        /// <summary>
        /// Update the logged-in user's password. [Any Role]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="model">The information to update password.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account, a 400 Bad Request if the information is invalid or a
        /// or a 404 Not Found if a account was not found.</returns>
        [HttpPut("/api/users/my/password", Name = UsersControllerRoute.PutMyPassUser)]
        //[ApiValidationFilter]
        [Authorize] // Every roles
        [SwaggerResponse(StatusCodes.Status200OK, "The password of account was updated.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutMyPass(
            [FromServices] IPutMyPassUserCommand command,
            [FromBody] UpdatePassword model,
            CancellationToken cancellationToken) => command.ExecuteAsync(model, cancellationToken);

        /// <summary>
        /// Get a customer account by ID. [Role Root].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to get the account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the account or a 404 Not Found if a account with id was not found.</returns>
        [HttpGet("/api/users/{id}", Name = UsersControllerRoute.GetUser)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "The account with id.", typeof(User))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Get(
            [FromServices] IGetUserCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, cancellationToken);

        /// <summary>
        /// Update a customer account by ID. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to update the account.</param>
        /// <param name="model">The information to update account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account, a 400 Bad Request if the information is invalid or a
        /// or a 404 Not Found if a account was not found.</returns>
        [HttpPut("/api/users/{id}", Name = UsersControllerRoute.PutUser)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "The account was updated.", typeof(User))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Put(
            [FromServices] IPutUserCommand command,
            string id,
            [FromBody] UpdateUser model,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, model, cancellationToken);

        /// <summary>
        /// Delete a customer account by ID. [Role Root].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to delete the account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if the account was deleted or a 404 Not Found if a account with id was not found.</returns>
        [HttpDelete("/api/users/{id}", Name = UsersControllerRoute.DeleteUser)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The account with id was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id was not found.")]
        public Task<IActionResult> Delete(
            [FromServices] IDeleteUserCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, cancellationToken);

        /// <summary>
        /// Update a user password. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to update.</param>
        /// <param name="model">The information to update password.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account, a 400 Bad Request if the information is invalid or a
        /// or a 404 Not Found if a account was not found.</returns>
        [HttpPut("/api/users/{id}/password", Name = UsersControllerRoute.PutPassUser)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "The password of account was updated.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutPass(
            [FromServices] IPutPassUserCommand command,
            string id,
            [FromBody] UpdatePassword model,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, model, cancellationToken);

        /// <summary>
        /// Lock an account [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id of account to lock.</param>
        /// <param name="model">The information of lockrequest.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response if lock account success, a 400 Bad Request if the information of lockrequest is invalid or a
        /// or a 404 Not Found if a account was not found.</returns>
        [HttpPut("/api/users/{id}/lock", Name = UsersControllerRoute.PutLockUser)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "Lock account success!")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information of lockrequest is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutLock(
            [FromServices] IPutLockUserCommand command,
            string id,
            [FromBody] LockRequestUser model,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, model, cancellationToken);

        /// <summary>
        /// Unlock an user account [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id of account to unlock.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account,
        /// or a 404 Not Found if a account was not found.</returns>
        [HttpPut("/api/users/{id}/unlock", Name = UsersControllerRoute.PutUnLockUser)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "Unlock account success!")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Account with id was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutUnLock(
            [FromServices] IPutUnLockUserCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, cancellationToken);

        /// <summary>
        /// Send a request reset password to your email if you forgot your password.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="email">The email of your account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 200 OK response if An confirm email was sent,
        /// a 404 Not Found if not found account with email.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("/api/users/forgot", Name = UsersControllerRoute.PostForgotPassword)]
        [SwaggerResponse(StatusCodes.Status200OK, "The confirm email was sent!")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found account with email!")]
        public Task<IActionResult> PostForgotPassword(
            [FromServices] IPostForgotPasswordCommand command,
            string email,
            CancellationToken cancellationToken) => command.ExecuteAsync(email, cancellationToken);

        /// <summary>
        /// Verify email to reset password
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="userId">The id of account.</param>
        /// <param name="token">The token to confirm.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 200 OK response containing the account,
        /// a 400 Bad Request if email verification failed.
        /// a 404 Not Found if a account with id was not found.
        /// </returns>
        [HttpGet("/api/users/reset", Name = UsersControllerRoute.GetResetPassword)]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, "Verify success.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Email verification failed.Please try again in a few minutes!")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found account with id")]
        public Task<IActionResult> GetResetPassword(
            [FromServices] IGetResetPasswordCommand command,
            string userId,
            string token,
            CancellationToken cancellationToken) => command.ExecuteAsync(userId, token);

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="resetPassword">The new password.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 200 OK response containing the account,
        /// a 400 Bad Request if token verification failed.
        /// a 404 Not Found if a account with id was not found.
        /// </returns>
        [HttpPost("/api/users/reset", Name = UsersControllerRoute.PostResetPassword)]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, "Password is update.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Token verification failed.Please try again in a few minutes!")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found account with id")]
        public Task<IActionResult> PostResetPassword(
            [FromServices] IPostResetPasswordCommand command,
            [FromBody] ResetPassword resetPassword,
            CancellationToken cancellationToken) => command.ExecuteAsync(resetPassword, cancellationToken);
    }
}
