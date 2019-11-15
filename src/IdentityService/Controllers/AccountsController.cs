using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer4.AccessTokenValidation;
using IdentityService.Commands.AccountCommands;
using IdentityService.Constants;
using IdentityService.ViewModels.AccountViewModels;
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
    public class AccountsController : ControllerBase
    {
        /// <summary>
        /// Get a paginated list of all customer accounts. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="sort">Specifies sort order.
        /// Available values: _id, email, birth_date, family_name, name, gender, user_name</param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a list of all customer accounts, a 400 Bad Request if the sort, limit, page request
        /// parameters are invalid or a 404 Not Found if a page with the specified page number was not found.
        /// </returns>
        [HttpGet("/api/accounts", Name = AccountsControllerRoute.GetAllAccount)]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of all customer accounts.", typeof(ListAccount))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetAll(
            [FromServices] IGetAllAccountCommand command,
            string sort,
            int limit,
            int page,
            CancellationToken cancellationToken) => command.ExecuteAsync(sort, limit, page);

        /// <summary>
        /// Create a new customer account. [Role Admin].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="model">The information of account to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created customer account or a 400 Bad Request if the information of account is
        /// invalid.</returns>
        [HttpPost("/api/accounts", Name = AccountsControllerRoute.PostAccount)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status201Created, "The customer account was created.", typeof(Account))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information of account is invalid.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Post(
            [FromServices] IPostAccountCommand command,
            [FromBody] CreateAccount model,
            CancellationToken cancellationToken) => command.ExecuteAsync(model);

        /// <summary>
        /// Get the logged-in user's account details. [Any logged-in user]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response a the logged-in user's account details, or a 404 Not Found if account was not found.
        /// </returns>
        [HttpGet("/api/accounts/my", Name = AccountsControllerRoute.GetMyAccount)]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, "A the logged-in user's account details.", typeof(Account))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Account was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetMy(
            [FromServices] IGetMyAccountCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync();

        /// <summary>
        /// Update your account details. [Any logged-in user].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="model">The information of account to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account, a 400 Bad Request if the model of account is invalid or a
        /// or a 404 Not Found if a account was not found.</returns>
        [HttpPut("/api/accounts/my", Name = AccountsControllerRoute.PutMyAccount)]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, "The account was updated.", typeof(Account))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information of account is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account was not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutMy(
            [FromServices] IPutMyAccountCommand command,
            [FromBody] UpdateAccount model,
            CancellationToken cancellationToken) => command.ExecuteAsync(model);

        /// <summary>
        /// Get a customer account by ID. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to get the account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the account or a 404 Not Found if a car with id was not found.</returns>
        [HttpGet("/api/accounts/{id}", Name = AccountsControllerRoute.GetAccount)]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, "The a account by ID.", typeof(Account))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Get(
            [FromServices] IGetAccountCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// Update a customer account by ID. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to update the account.</param>
        /// <param name="model">The information of account to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account, a 400 Bad Request if the information of account is invalid or a
        /// or a 404 Not Found if a account with was not found.</returns>
        [HttpPut("/api/accounts/{id}", Name = AccountsControllerRoute.PutAccount)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Admin, Roles= RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, "Account with id was updated.", typeof(Account))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information of account is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account was not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Put(
            [FromServices] IPutAccountCommand command,
            string id,
            [FromBody] UpdateAccount model,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, model);

        /// <summary>
        /// Delete a customer account by ID. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to delete the account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if the account was deleted or a 404 Not Found if a account with id was not found.</returns>
        [HttpDelete("/api/accounts/{id}", Name = AccountsControllerRoute.DeleteAccount)]
        [Authorize(ApplicationPolicies.Admin, Roles= RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The account with id was deleted!")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Delete(
            [FromServices] IDeleteAccountCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// [NotImplemented] - Add a new note on a customer account. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to add a new note onto an account</param>
        /// <param name="data">New note</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the note was add on a customer account or a 400 Bad Request if the note is
        /// invalid.</returns>
        [HttpPost("/api/accounts/{id}/notes", Name = AccountsControllerRoute.PostAccountNote)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status201Created, "The new note was add on a customer account.", typeof(Account))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The new note is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The account was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PostNote(
            [FromServices] IPostAccountNoteCommand command,
            string id,
            [FromBody] string data,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, data);

        /// <summary>
        /// [NotImplemented] - Update customer account status by ID. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to update an account status</param>
        /// <param name="status">New status</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created account or a 400 Bad Request if the account is
        /// invalid.</returns>
        [HttpPost("/api/accounts/{id}/status", Name = AccountsControllerRoute.PostAccountStatus)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status201Created, "The status was update on a customer account.", typeof(Account))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The status is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The account was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PostStatus(
            [FromServices] IPostAccountStatusCommand command,
            string id,
            [FromBody] string status,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, status);

        /// <summary>
        /// Unlink a system user to a customer account. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to unlink a user to account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if the account was deleted or a 404 Not Found if a account with id was not found.</returns>
        [HttpDelete("/api/accounts/{id}/user", Name = AccountsControllerRoute.DeleteAccountUser)]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The account with id was unlinked!")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> DeleteUser(
            [FromServices] IDeleteAccountUserCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);

        /// <summary>
        /// Link a system user to a customer account. [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id to link the account to a user.</param>
        /// <param name="username">The username of account to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response Link a system user to a customer account success, a 400 Bad Request if the username is invalid or a
        /// or a 404 Not Found if a account with id was not found.</returns>
        [HttpPut("/api/accounts/{id}/user", Name = AccountsControllerRoute.PutAccountUser)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, "Link a system user to a customer account success!")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The username is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutUser(
            [FromServices] IPutAccountUserCommand command,
            string id,
            [FromBody] string username,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, username);

        /// <summary>
        /// Lock an account [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id of account to lock.</param>
        /// <param name="model">The lockrequest of account to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response Lock account success, a 400 Bad Request if the model of lockrequest is invalid
        /// or a 404 Not Found if a account with id was not found.</returns>
        [HttpPut("/api/accounts/{id}/lock", Name = AccountsControllerRoute.PutAccountLock)]
        //[ApiValidationFilter]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, "Lock account success!")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The model of LockRequestAccount is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutLock(
            [FromServices] IPutAccountLockCommand command,
            string id,
            [FromBody] LockRequestAccount model,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, model);

        /// <summary>
        /// Unlock an user account [Role Admin]
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">The id of account to unlock.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response unlock account success
        /// or a 404 Not Found if a account with id was not found.</returns>
        [HttpPut("/api/accounts/{id}/unlock", Name = AccountsControllerRoute.PutAccountUnLock)]
        [Authorize(ApplicationPolicies.Admin, Roles = RoleNames.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, "Unlock account success.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account with id could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutUnLock(
            [FromServices] IPutAccountUnLockCommand command,
            string id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id);
    }
}