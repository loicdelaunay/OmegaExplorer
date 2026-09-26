using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;

namespace OmegaExplorer.Server.Services._Game.Brands;

public class BrandDataProvider : GameDataProvider<IDynamicItemBrand>
{
    public BrandDataProvider(ILogger<BrandDataProvider> logger) : base(logger)
    {
    }
}