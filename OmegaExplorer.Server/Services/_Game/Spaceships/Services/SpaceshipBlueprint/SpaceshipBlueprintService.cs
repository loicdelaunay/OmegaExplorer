using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;

public class SpaceshipBlueprintService
{
    public int GetMaxModuleCount(Spaceship.SpaceshipSize size, Guid userId)
    {
        var maxModuleCount = 0;

        switch (size)
        {
            case Spaceship.SpaceshipSize.Cruiser:
                maxModuleCount += 4;
                break;
            case Spaceship.SpaceshipSize.Corvette:
                maxModuleCount += 6;
                break;
            case Spaceship.SpaceshipSize.Battleship:
                maxModuleCount += 10;
                break;
            case Spaceship.SpaceshipSize.Dreadnought:
                maxModuleCount += 14;
                break;
            case Spaceship.SpaceshipSize.Capital:
                maxModuleCount += 25;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(size), size, null);
        }

        // Get user tech progress to add more max module count

        return maxModuleCount;
    }
}