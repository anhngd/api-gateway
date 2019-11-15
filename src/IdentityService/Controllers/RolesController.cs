using System.Threading;
using System.Threading.Tasks;
using IdentityService.Commands.RoleCommands;
using IdentityService.Constants;
using IdentityService.ViewModels.AccountViewModels;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IdentityService.Controllers
{
    //[Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RolesController : ControllerBase
    {
        /// <summary>
        /// Show ALL Role 
        /// </summary>
        /// <returns>A 200 OK response containing a list of all customer accounts, a 400 Bad Request if the sort, limit, page request
        /// parameters are invalid or a 404 Not Found if a page with the specified page number was not found.
        /// </returns>
        [HttpGet("/api/roles", Name = RolesControllerRoute.GetRole)]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of all customer accounts.", typeof(ListAccount))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetRole(
            [FromServices] IGetRoleCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync();
        /// <summary>
        /// Update your account Role. 
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="model">The information to update account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated account, a 400 Bad Request if the information is invalid or a
        /// or a 404 Not Found if a account was not found.</returns>
        //[Authorize]
        [HttpPut("/api/users/roles", Name = RolesControllerRoute.PutRole)]
        [SwaggerResponse(StatusCodes.Status200OK, "Your account was updated.", typeof(User))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The information is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A account could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> PutRole(
            [FromServices] IPutUserRoleCommand command,
            [FromBody] UpdateUserRole model,
            CancellationToken cancellationToken) => command.ExecuteAsync(model, cancellationToken);

    }
}