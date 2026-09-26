using AutoMapper;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.StarSystems.Models.Resolver;

public class StarSystemTypeResolver : IValueResolver<StarSystem,
    ResponseStarSystem, EnumStarSystemType>
{
    public EnumStarSystemType Resolve(StarSystem source,
        ResponseStarSystem destination, EnumStarSystemType destMember, ResolutionContext context)
    {
        try
        {
            switch (source)
            {
                case Star:
                    return EnumStarSystemType.Star;
                case Instability:
                    return EnumStarSystemType.Instability;
                case Planet:
                    return EnumStarSystemType.Planet;
                default:
                    return EnumStarSystemType.Unknown; // Default case
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}