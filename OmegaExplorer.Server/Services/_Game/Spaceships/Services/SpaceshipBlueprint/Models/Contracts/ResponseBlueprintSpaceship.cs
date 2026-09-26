using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;

public class ResponseBlueprintSpaceship : ResponseMetadata
{
    public ResponseUser? Owner { get; set; }

    public Spaceship.SpaceshipSize Size { get; set; }

    //Server feed it from ModuleInfos
    public List<ResponseSpaceshipModule> Modules { get; set; } = new();

    public string Thumbnail { get; set; }
}