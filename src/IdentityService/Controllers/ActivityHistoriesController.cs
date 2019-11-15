using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer4.AccessTokenValidation;
using IdentityService.Commands.ActivityHistoryCommands;
using IdentityService.Constants;
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
    public class ActivityHistoriesController : ControllerBase
    {
        /// <summary>
        /// Get all activity histories. [Role Admin].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 200 OK response containing all activity histories
        /// or a 404 Not Found if activity histories with id was not found.
        /// </returns>
        [HttpGet("/api/activities/all", Name = ActivityHistoriesControllerRoute.GetAllActivityHistory)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "List login's history of all user.", typeof(List<IdentityServer.Models.ActivityHistory>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found.")]
        public Task<IActionResult> GetAllActivityHistory(
            [FromServices] IGetAllActivityHistoryCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync();

        /// <summary>
        /// Get my activity history. [Role Admin].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 200 OK response containing the your activity histories 
        /// or a 404 Not Found if you not login or activity histories is empty.
        /// </returns>
        [HttpGet("/api/activities/my", Name = ActivityHistoriesControllerRoute.GetMyActivityHistory)]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, "Your activity history.", typeof(List<IdentityServer.Models.ActivityHistory>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found.")]
        public Task<IActionResult> GetMyActivityHistory(
            [FromServices] IGetMyActivityHistoryCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync();

        /// <summary>
        /// Get activity history by Id. [Role Admin].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="id">Id of account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 200 OK response containing the activity history getted by id 
        /// or a 404 Not Found if activity history with id was not found.
        /// </returns>
        [HttpGet("/api/activities/{id}", Name = ActivityHistoriesControllerRoute.GetActivityHistoryById)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "Activity history getted by id.", typeof(IdentityServer.Models.ActivityHistory))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found.")]
        public Task<IActionResult> GetActivityHistoryById(
            [FromServices] IGetActivityHistoryByIdCommand command,
            int id,
            CancellationToken cancellationToken) => command.ExecuteAsync(id, cancellationToken);

        /// <summary>
        /// Get list activity histories by userId. [Role Admin].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="userId">Id of account.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 200 OK response containing the account 
        /// or a 404 Not Found if a account with id was not found.
        /// </returns>
        [HttpGet("/api/activities", Name = ActivityHistoriesControllerRoute.GetActivityHistoryByUserId)]
        [Authorize(ApplicationPolicies.Root, Roles = RoleNames.Root)]
        [SwaggerResponse(StatusCodes.Status200OK, "List activity histories getted by userId.", typeof(List<IdentityServer.Models.ActivityHistory>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found.")]
        public Task<IActionResult> GetActivityHistoryByUserId(
            [FromServices] IGetActivityHistoryByUserIdCommand command,
            string userId,
            CancellationToken cancellationToken) => command.ExecuteAsync(userId);

        /// <summary>
        /// Create new activity history type login if you login. [Role Admin].
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>
        /// A 201 Created response containing the newly created customer account
        /// or a 404 Not Found if the information of account is invalid.
        /// </returns>
        [HttpPost("/api/activities", Name = ActivityHistoriesControllerRoute.PostActivityHistory)]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status201Created, "The activity histories was created.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Information of account is invalid.")]
        public Task<IActionResult> PostActivityHistory(
            [FromServices] IPostActivityHistoryCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync();
    }
}
