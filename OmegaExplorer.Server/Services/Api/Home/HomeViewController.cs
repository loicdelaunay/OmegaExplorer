using Microsoft.AspNetCore.Mvc;

namespace OmegaExplorer.Server.Services.Api.Home;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeViewController : Controller

{
    public IActionResult Index()
    {
        //return View("~/Views/Index.cshtml");
        return View("~/Services/Api/Home/View/Index.cshtml");
    }

}