using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Species.Models.Contracts;

public class ResponseSpeciesAmount
{
    public IDynamicItemSpecies Species { get; set; }

    public long Amount { get; set; }
}