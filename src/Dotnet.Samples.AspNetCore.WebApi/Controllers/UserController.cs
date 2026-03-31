using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Samples.AspNetCore.WebApi.Controllers


[ApiController]
[Route("users")]
[Produces("application/json")]
public class UserController(
        IPlayerService playerService,
        ILogger<UserController> logger,
        IValidator<PlayerRequestModel> validator
    ) : ControllerBase
{

    [HttpGet("squadNumber/{squadNumber:int}", Name = "RetrieveBySquadNumber")]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetBySquadNumberAsync([FromRoute] int squadNumber)
    {
        var player = await playerService.RetrieveBySquadNumberAsync(squadNumber);
        if (player != null)
        {
            logger.LogInformation(
                "GET /players/squadNumber/{SquadNumber} retrieved: {@Player}",
                squadNumber,
                player
            );
            return TypedResults.Ok(player);
        }
        else
        {
            logger.LogWarning("GET /players/squadNumber/{SquadNumber} not found", squadNumber);
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: NotFoundTitle,
                detail: $"Player with Squad Number '{squadNumber}' was not found.",
                instance: HttpContext?.Request?.Path.ToString()
            );
        }
    }
}

}
