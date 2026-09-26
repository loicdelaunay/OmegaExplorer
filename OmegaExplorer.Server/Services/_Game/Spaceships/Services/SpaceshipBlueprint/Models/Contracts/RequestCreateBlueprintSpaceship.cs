using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services.Validations;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;

public class RequestCreateBlueprintSpaceship : IValid
{
    /// <summary>
    ///     If null create else update
    /// </summary>
    public Guid? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Spaceship.SpaceshipSize Size { get; set; }

    public List<BlueprintSpaceshipModule> Modules { get; set; } = new();

    public Task<NotValidReason?> Validate()
    {
        //TODO check validation

        return Task.FromResult<NotValidReason?>(null);
    }

    public BlueprintSpaceship ToBlueprintSpaceship(Guid ownerId)
    {
        BlueprintSpaceship newBlueprint = new()
        {
            Name = Name,
            Size = Size,
            OwnerId = ownerId
        };

        foreach (var module in Modules) newBlueprint.Modules.Add(module);

        if (!Id.IsNullOrEmpty()) newBlueprint.Id = (Guid)Id;

        return newBlueprint;
    }
}