using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer4.AccessTokenValidation;
using IdentityService.Commands.AdminCommands;
using IdentityService.Constants;
using IdentityService.ViewModels.AdminViewModels;
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
    public class AdminsController : ControllerBase
    {
        /// <summary>
        /// Get a paginated list of all admin accounts. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="sort">Specifies sort order.
        /// Available values: _id, email, birth_date, family_name, name, gender, user_name</param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a paginated list of all customer admins, a 400 Bad Request if the sort, limit, page request
        /// parameters are invalid or a 404 Not Found if a page with the specified page number was not found.
        /// </returns>
        [HttpGet("/api/admins", Name = AdminsControllerRoute.GetAllAdmin)]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of account.", typeof(ListAdmin))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetAll(
            [FromServices] IGetAllAdminCommand command,
            string sort,
            int limit,
            int page,
            CancellationToken cancellationToken) => command.ExecuteAsync(sort, limit, page);

        /// <summary>
        /// Create a new admin account. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="model">The information of admin account to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created admin account or a 400 Bad Request if the information of admin account is
        /// invalid.</returns>
        [HttpPost("/api/admins", Name = AdminsControllerRoute.PostAdmin)]
        //[ApiValidationFilter]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status201Created, "The admin account was created.", typeof(Admin))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information of admin account is invalid.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Post(
            [FromServices] IPostAdminCommand command,
            [FromBody] CreateAdmin model,
            CancellationToken cancellationToken) => command.ExecuteAsync(model);

        /// <summary>
        /// Get an admin account by ID. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to get the admin account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the admin account or a 404 Not Found if a account with id was not found.</returns>
        [HttpGet("/api/admins/{id}", Name = AdminsControllerRoute.GetAdmin)]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "The a admin account by ID.", typeof(Admin))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A admin account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Get(
            [FromServices] IGetAdminCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// Update an admin account by ID. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to update the admin account.</param>
        /// <param name="model">The information of admin account to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated admin, a 400 Bad Request if the information of admin account is invalid or a
        /// or a 404 Not Found if a admin account with was not found.</returns>
        [HttpPut("/api/admins/{id}", Name = AdminsControllerRoute.PutAdmin)]
        //[ApiValidationFilter]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "The admin account was updated.", typeof(Admin))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information of admin account is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A admin account was not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Put(
            [FromServices] IPutAdminCommand command,
            string id,
            [FromBody] UpdateAdmin model,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, model);

        /// <summary>
        /// Delete an admin account by ID. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to delete the admin account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if the admin account was deleted or a 404 Not Found if a admin account with id was not found.</returns>
        [HttpDelete("/api/admins/{id}", Name = AdminsControllerRoute.DeleteAdmin)]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The admin account with id was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A admin account with id was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Delete(
            [FromServices] IDeleteAdminCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// Unlink an admin account from a user account. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to unlink the admin account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if Unlink admin account success or a 404 Not Found if a account with id was not found.</returns>
        [HttpDelete("/api/admins/{id}/user", Name = AdminsControllerRoute.DeleteAdminUser)]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Unlink admin account success.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A admin account with id was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> DeleteUser(
            [FromServices] IDeleteAdminUserCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// [NotImplemented] - Update an admin account's groups by ID. [Role Root]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id update group admin account</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the group was updated or a 400 Bad Request if the group is invalid.</returns>
        [HttpPut("/api/admins/{id}/groups", Name = AdminsControllerRoute.PutAdminGroup)]
        //[ApiValidationFilter]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status201Created, "The group of account was updated.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The new note is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The account was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutGroup(
            [FromServices] IPutAdminGroupCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// [NotImplemented] - Update customer admin status by ID. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to update permission of account</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created admin or a 400 Bad Request if the admin is
        /// invalid.</returns>
        [HttpPut("/api/admins/{id}/permissions", Name = AdminsControllerRoute.PutAdminPermission)]
        //[ApiValidationFilter]
        //[Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status201Created, "The permission was update on an account.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The permission is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The account was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutPermission(
            [FromServices] IPutAdminPermissionCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// Link a system user to a customer admin. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to link the admin to a user.</param>
        /// <param name="username">The username of admin to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated admin, a 400 Bad Request if the username of admin is invalid or a
        /// or a 404 Not Found if a admin with id was not found.</returns>
        [HttpPut("/api/admins/{id}/user", Name = AdminsControllerRoute.PutAdminUser)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "Link a system user to a customer admin success.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The username is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutUser(
            [FromServices] IPutAdminUserCommand command,
            string id,
            [FromBody] string username,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// Lock an admin [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id of admin to lock.</param>
        /// <param name="model">The lockrequest of admin to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated admin, a 400 Bad Request if the lockrequest of admin is invalid or a
        /// or a 404 Not Found if a admin with id was not found.</returns>
        [HttpPut("/api/admins/{id}/lock", Name = AdminsControllerRoute.PutAdminLock)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "The username of admin was updated.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The username is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A admin with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutLock(
            [FromServices] IPutAdminLockCommand command,
            string id,
            [FromBody] LockRequestAdmin model,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, model);

        /// <summary>
        /// Unlock an user admin [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id of admin to unlock.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated admin, a 400 Bad Request if the lockrequest of admin is invalid or a
        /// or a 404 Not Found if a admin with id was not found.</returns>
        [HttpPut("/api/admins/{id}/unlock", Name = AdminsControllerRoute.PutAdminUnLock)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "The username of admin was updated.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A admin with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutUnLock(
            [FromServices] IPutAdminUnLockCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);
    }
}