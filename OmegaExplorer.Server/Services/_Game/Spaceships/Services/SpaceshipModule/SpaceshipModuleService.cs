using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule;

public class SpaceshipModuleService
{
    private readonly SpaceshipModuleDataProvider _spaceshipModuleDataProvider;

    public SpaceshipModuleService(SpaceshipModuleDataProvider spaceshipModuleDataProvider)
    {
        _spaceshipModuleDataProvider = spaceshipModuleDataProvider;
    }

    public async Task<List<IDynamicItemSpaceshipSkill>> GetSkill(Models.Entities.SpaceshipModule spaceshipModule)
    {
        List<IDynamicItemSpaceshipSkill> res = new();

        var moduleData = _spaceshipModuleDataProvider.GetByIndex(spaceshipModule.Index);
        if (moduleData == null) return res;

        res.AddRange(moduleData.Skills);

        return res;
    }


    public async Task AssignModulesToSpaceship(Spaceship spaceship,
        BlueprintSpaceship blueprint)
    {
        var index = 0;
        foreach (var blueprintModule in blueprint.Modules)
        {
            // Detach module from blueprint to avoid circular reference
            blueprint.Modules[index].BlueprintSpaceship = null;

            var spaceshipModule = new Models.Entities.SpaceshipModule(blueprintModule);
            var moduleData = _spaceshipModuleDataProvider.GetByIndex(blueprintModule.Index);

            if (moduleData == null)
                throw new Exception("Spaceship module data not found at index " + blueprintModule.Index);

            // Set max health of the module
            spaceshipModule.Health = moduleData.Health;

            spaceship.Modules.Add(spaceshipModule);

            // Compute speed and other stats link to the ship
            await ComputeStats(spaceship, blueprint.Modules);

            index++;
        }
    }

    public async Task ComputeStats(Spaceship spaceship,
        List<BlueprintSpaceshipModule> blueprintModules)
    {
        foreach (var blueprintModule in blueprintModules)
        {
            var data = _spaceshipModuleDataProvider.GetByIndex(blueprintModule.Index);

            if (data == null) throw new Exception("Spaceship module data not found at index " + blueprintModule.Index);

            if (!data.Modifiers.Any()) continue;

            var speed = data.Modifiers
                .Where(modifier => modifier.ModifierSpaceshipModuleModifierType ==
                                   SpaceshipModuleModifier.SpaceshipModuleModifierType.Velocity)
                .Sum(modifier => modifier.Value);

            spaceship.SpeedInStarCluster = speed;
            spaceship.SpeedInGalaxy = speed;
            spaceship.SpeedInUniverse = speed;
        }
    }
}