using AutoMapper;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapSpaceshipModule : IMappingAction<SpaceshipModule, ResponseSpaceshipModule>
{
    private readonly SpaceshipModuleDataProvider _spaceshipModuleDataProvider;

    public AfterMapSpaceshipModule(SpaceshipModuleDataProvider spaceshipModuleDataProvider)
    {
        _spaceshipModuleDataProvider = spaceshipModuleDataProvider;
    }

    public void Process(SpaceshipModule src, ResponseSpaceshipModule dest, ResolutionContext ctx)
    {
        var data = _spaceshipModuleDataProvider.GetByIndex(src.Index);
        if (data == null)
        {
            return;
        }

        dest.Data = ctx.Mapper.Map<ResponseDynamicItemSpaceshipModule>(data);
        dest.Name = data.Name;
    }
}