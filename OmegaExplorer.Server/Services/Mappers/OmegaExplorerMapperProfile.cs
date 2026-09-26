using AutoMapper;
using OmegaExplorer.Server.Services._Core.Models.Contracts;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Entities;
using OmegaExplorer.Server.Services._Game.Effects;
using OmegaExplorer.Server.Services._Game.Effects.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Events.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.Events.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Contracts;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Interfaces;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Entities;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Orders.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Orders.Models.Entities;
using OmegaExplorer.Server.Services._Game.Origins.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Origins.Models.Entities;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;
using OmegaExplorer.Server.Services._Game.Quests.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Quests.Models.Entities;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Interfaces;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Resolver;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Universes.Models.Contracts.Responses;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services.Mappers.AfterMapper;
using OmegaExplorer.Server.Services.Users.Models.Classes;
using OmegaExplorer.Server.Services.Users.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services.Mappers;

public class OmegaExplorerMapperProfile : Profile
{
    public OmegaExplorerMapperProfile()
    {
        // Star systems and related
        CreateMap<Star, ResponseStarSystem>()
            .ForMember(dest => dest.StarSystemType, opt => opt.MapFrom(src => EnumStarSystemType.Star))
            .AfterMap<AfterMapFeedSpeciesDataForStarSystem>();

        CreateMap<ModifierResource, ResponseModifierResource>();

        CreateMap<Instability, ResponseStarSystem>()
            .ForMember(dest => dest.StarSystemType, opt => opt.MapFrom(src => EnumStarSystemType.Instability))
            .AfterMap<AfterMapFeedSpeciesDataForStarSystem>();

        CreateMap<Planet, ResponseStarSystem>()
            .ForMember(dest => dest.StarSystemType, opt => opt.MapFrom(src => EnumStarSystemType.Planet))
            .AfterMap<AfterMapFeedSpeciesDataForStarSystem>();

        CreateMap<StarSystem, ResponseStarSystem>()
            .ForMember(dest => dest.StarSystemType, opt => opt.MapFrom<StarSystemTypeResolver>())
            .AfterMap<AfterMapFeedSpeciesDataForStarSystem>();

        // Orders
        CreateMap<Order, ResponseOrder>();
        CreateMap<Orderable, ResponseOrderable>();

        // Math / vectors
        CreateMap<Vector2, ResponseVector2>();

        // User
        CreateMap<User, ResponseUser>();
        CreateMap<UserPreferences, ResponseUserPreferences>();
        CreateMap<UserScenarioProgress, ResponseUserScenarioProgress>()
            .AfterMap<AfterMapUserScenarioProgress>();

        // Spatial
        CreateMap<SpatialLocation, ResponseSpatialLocation>();
        CreateMap<SpatialTravel, ResponseSpatialTravel>();
        CreateMap<SpatialObject, ResponseSpatialObject>();
        CreateMap<SpatialDistance, ResponseSpatialDistance>();
        CreateMap<SpatialObject, ResponseSpatialObject>();

        // Universe/Galaxy/Clusters
        CreateMap<Galaxy, ResponseGalaxy>();
        CreateMap<StarCluster, ResponseStarCluster>();
        CreateMap<Universe, ResponseUniverse>();

        // Battles
        CreateMap<Battle, ResponseBattle>();
        CreateMap<ActionInBattle, ResponseActionInBattle>()
            .AfterMap<AfterMapActionInBattle>();
        CreateMap<ActionInBattleResult, ResponseActionInBattleResult>();
        CreateMap<BattleResult, ResponseBattleResult>();
        CreateMap<BattleRewardResult, ResponseBattleRewardResult>();

        // Quests
        CreateMap<QuestObjectiveProgress, ResponseQuestObjectiveProgress>();
        CreateMap<QuestProgress, ResponseQuestProgress>()
            .AfterMap<AfterMapQuestProgress>();
        CreateMap<IDynamicItemQuest, ResponseDynamicItemQuest>();

        // Tech / Knowledge
        CreateMap<TechnologyKnowledge, ResponseTechnologyKnowledge>()
            .AfterMap<AfterMapTechnologyKnowledge>();

        // Buildings / Spaceships / Modules
        CreateMap<Building, ResponseBuilding>()
            .AfterMap<AfterMapBuilding>();

        CreateMap<Spaceship, ResponseSpaceship>()
            .AfterMap<AfterMapSpaceship>();

        CreateMap<IDynamicItemSpaceshipModule, ResponseDynamicItemSpaceshipModule>();
        CreateMap<SpaceshipModuleModifier, ResponseSpaceshipModuleModifier>();

        CreateMap<SpaceshipModule, ResponseSpaceshipModule>()
            .AfterMap<AfterMapSpaceshipModule>();

        CreateMap<BlueprintSpaceship, ResponseBlueprintSpaceship>()
            // Modules are ignored to be transformed later from blueprint to generic modules
            .ForMember(dest => dest.Modules, opt => opt.Ignore())
            .AfterMap<AfterMapSpaceshipBlueprint>();

        CreateMap<BlueprintSpaceshipModule, ResponseBlueprintSpaceshipModule>();

        // Effects
        CreateMap<Effect, ResponseEffect>();

        // Dynamic items (resources / events / skills / scenarios / choices)
        CreateMap<IDynamicItemGameResource, ResponseDynamicItemGameResource>();
        CreateMap<IDynamicItemEvent, ResponseDynamicItemEvent>();
        CreateMap<IDynamicItemSpaceshipSkill, ResponseDynamicItemSpaceshipSkill>();
        CreateMap<IDynamicItemScenario, ResponseDynamicItemScenario>();
        CreateMap<IDynamicItemScenarioChoice, ResponseDynamicItemScenarioChoice>();
        CreateMap<IDynamicItemEventChoice, ResponseDynamicItemEventChoice>();

        // Events
        CreateMap<Event, ResponseEvent>()
            .AfterMap<AfterMapEventToResponseEvent>();

        // Misc
        CreateMap<FileInfo, ResponseFileInfo>();
        CreateMap<DirectoryInfo, ResponseDirectoryInfo>();
        CreateMap<Personality, ResponsePersonality>();
        CreateMap<Origin, ResponseOrigin>();
        CreateMap<GameResource, ResponseGameResource>();
        CreateMap<ScenarioDialog, ResponseScenarioDialog>();
    }
}