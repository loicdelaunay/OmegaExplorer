using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game.Limits.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;
using OmegaExplorer.Shared.Enums;
using System.Net;

namespace OmegaExplorer.Server.Services.Servers;

public static class StatusCodeGenerator
{
    /// <summary>
    ///     Return a not connected object
    /// </summary>
    /// <returns> </returns>
    public static ObjectResult NotConnected()
    {
        return new ObjectResult("Not connected, please login")
        {
            StatusCode = (int?)HttpStatusCode.Forbidden
        };
    }

    public static ObjectResult NotFound(string objectName)
    {
        return new ObjectResult($"Object {objectName} not found")
        {
            StatusCode = (int?)HttpStatusCode.NotFound
        };
    }

    public static ObjectResult Forbidden(string message = "method not allowed")
    {
        return new ObjectResult($"Forbidden : {message}")
        {
            StatusCode = (int?)HttpStatusCode.Forbidden
        };
    }

    /// <summary>
    ///     Return a not connected object
    /// </summary>
    /// <returns> </returns>
    public static ObjectResult Exception(Exception e)
    {
        return new ObjectResult("Internal server exception " + e.Message)
        {
            StatusCode = (int?)HttpStatusCode.InternalServerError
        };
    }

    public static ObjectResult OmegaExplorerError(EnumCustomStatusCode code)
    {
        return new ObjectResult($"OmegaExplorer error : {code}")
        {
            StatusCode = (int?)HttpStatusCode.InternalServerError
        };
    }

    public static ActionResult<ResponseBlueprintSpaceship> BadRequest(string requestIsNull)
    {
        return new ObjectResult($"OmegaExplorer bad request : {requestIsNull}")
        {
            StatusCode = (int?)HttpStatusCode.BadRequest
        };
    }

    public static ActionResult LimitReached(string message, Limit limit)
    {
        return new ObjectResult($"OmegaExplorer limit reached : {message}")
        {
            //TODO return custom error code
            StatusCode = (int?)HttpStatusCode.BadRequest
        };
    }
}