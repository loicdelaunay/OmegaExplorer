using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Species;

public class SpeciesDataProvider : GameDataProvider<IDynamicItemSpecies>
{
    public SpeciesDataProvider(ILogger<SpeciesDataProvider> logger) : base(logger)
    {
    }
}