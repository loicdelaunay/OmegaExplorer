using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OmegaExplorer.Server.Services.Servers;

/// <summary>
/// Controller about the backend
/// </summary>
[Route("/api/server")]
public class ServerController : ControllerBase
{
    /*/// <summary>
    /// Redirect / to /swagger to have the swagger documentation
    /// into the main page
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [Route("")]
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult Index()
    {
        return Redirect("/swagger");
    }*/

    /// <summary>
    /// Test if the server is alive
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [Route("/ping", Name = "Ping")]
    public ActionResult Ping()
    {
        return Ok("Pong");
    }
}