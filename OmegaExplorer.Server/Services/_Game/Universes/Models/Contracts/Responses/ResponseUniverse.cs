using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Universes.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Universes.Models.Contracts.Responses;

public class ResponseUniverse : ResponseMetadata
{
    public EnumUniverseType Type { get; set; } = EnumUniverseType.Physic;
}