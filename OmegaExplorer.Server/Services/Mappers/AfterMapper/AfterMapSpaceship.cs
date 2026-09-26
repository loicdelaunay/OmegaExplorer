using AutoMapper;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Providers;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapSpaceship : IMappingAction<Spaceship, ResponseSpaceship>
{
    private readonly SpaceshipThumbnailProvider _spaceshipThumbnailProvider;

    public AfterMapSpaceship(SpaceshipThumbnailProvider spaceshipThumbnailProvider)
    {
        _spaceshipThumbnailProvider = spaceshipThumbnailProvider;
    }

    public void Process(Spaceship src, ResponseSpaceship dest, ResolutionContext ctx)
    {
        dest.Thumbnail = _spaceshipThumbnailProvider.GetThumbnailPath(src).Result;
    }
}