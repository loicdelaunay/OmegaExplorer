using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Brands;

[ApiController]
[Route("api/brand")]
public class BrandController : ControllerCustom
{
    private readonly BrandDataProvider _brandDataProvider;
    private readonly BrandService _brandService;
    private readonly ILogger<BrandController> _logger;

    public BrandController(AuthenticationService authenticationService, ILogger<BrandController> logger,
        BrandService brandService, BrandDataProvider brandDataProvider) : base(authenticationService)
    {
        _brandService = brandService;
        _brandDataProvider = brandDataProvider;

        _logger = logger;
    }

    #region Get

    [HttpGet]
    [Route("get/all", Name = nameof(GetAllBrands))]
    public ActionResult<IEnumerable<IDynamicItemBrand>> GetAllBrands()
    {
        try
        {
            var brands = _brandDataProvider.GetAll();

            return Ok(brands);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all brands");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}