using AutoMapper;
using OmegaExplorer.Server.Services._Game.Spaceships.Providers;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapSpaceshipBlueprint : IMappingAction<BlueprintSpaceship, ResponseBlueprintSpaceship>
{
    private readonly SpaceshipThumbnailProvider _spaceshipThumbnailProvider;

    public AfterMapSpaceshipBlueprint(SpaceshipThumbnailProvider spaceshipThumbnailProvider)
    {
        _spaceshipThumbnailProvider = spaceshipThumbnailProvider;
    }

    public void Process(BlueprintSpaceship src, ResponseBlueprintSpaceship dest, ResolutionContext ctx)
    {
        // Transformation of blueprint modules into generic modules
        foreach (var blueprintModule in src.Modules)
        {
            SpaceshipModule spaceshipModule = new(blueprintModule)
            {
                Spaceship = null // Avoid circular reference
            };
            var spaceshipModuleMapped = ctx.Mapper.Map<ResponseSpaceshipModule>(spaceshipModule);
            dest.Modules.Add(spaceshipModuleMapped);
        }

        try
        {
            dest.Thumbnail = _spaceshipThumbnailProvider.GetThumbnailPath(src).Result;
        }
        catch (Exception e)
        {
            Log.Error(e, $"Error getting thumbnail for blueprint spaceship");
        }
    }
}