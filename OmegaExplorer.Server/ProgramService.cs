using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Game.Battles;
using OmegaExplorer.Server.Services._Game.Brands;
using OmegaExplorer.Server.Services._Game.Buildings;
using OmegaExplorer.Server.Services._Game.Celebrities;
using OmegaExplorer.Server.Services._Game.Cycles;
using OmegaExplorer.Server.Services._Game.Cycles.Hubs;
using OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess;
using OmegaExplorer.Server.Services._Game.Effects;
using OmegaExplorer.Server.Services._Game.Events;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice;
using OmegaExplorer.Server.Services._Game.Galaxies;
using OmegaExplorer.Server.Services._Game.GameResources;
using OmegaExplorer.Server.Services._Game.Limits;
using OmegaExplorer.Server.Services._Game.Maps;
using OmegaExplorer.Server.Services._Game.Modificators;
using OmegaExplorer.Server.Services._Game.Notifications;
using OmegaExplorer.Server.Services._Game.Orders;
using OmegaExplorer.Server.Services._Game.Origins;
using OmegaExplorer.Server.Services._Game.Origins.Modules.History;
using OmegaExplorer.Server.Services._Game.Personalities;
using OmegaExplorer.Server.Services._Game.Quests;
using OmegaExplorer.Server.Services._Game.Recompenses;
using OmegaExplorer.Server.Services._Game.Scenarios;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.Spaceships.Providers;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill;
using OmegaExplorer.Server.Services._Game.SpatialObjects;
using OmegaExplorer.Server.Services._Game.Species;
using OmegaExplorer.Server.Services._Game.StarClusters;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services._Game.Technologies;
using OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge;
using OmegaExplorer.Server.Services._Game.Universes;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Changelogs;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Securities;
using OmegaExplorer.Server.Services.Users;
using System.Reflection;

namespace OmegaExplorer.Server;

public static class ProgramService
{
    public static void ConfigureDatabaseServices(WebApplicationBuilder webApplicationBuilder)
    {
        Log.Logger.Information("Starting a virtual database SQLite", EnumLogSeverity.Information);

        // Mask The SQL request logs of EF Core
        webApplicationBuilder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

        webApplicationBuilder.Services.AddDbContextPool<DatabaseContext>(dbContextOptions =>
            dbContextOptions.UseSqlite("Filename=database.sqlite",
                    options =>
                    {
                        options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    })
                .UseExceptionProcessor()
        //.EnableSensitiveDataLogging()
        );

        Log.Logger.Success($"Database configured", EnumLogSeverity.Information);
    }

    public static void ConfigureServicesFeatures(WebApplicationBuilder webApplicationBuilder)
    {
        Log.Logger.Information("Configuring features services...");

        webApplicationBuilder.Services.AddScoped<AuthenticationService>();
        webApplicationBuilder.Services.AddScoped<SecurityService>();
        webApplicationBuilder.Services.AddScoped<UserService>();

        Log.Logger.Success("Features configured !", EnumLogSeverity.Information);
    }

    public static void ConfigureServicesGameFeatures(WebApplicationBuilder webApplicationBuilder)
    {
        Log.Logger.Information("Configuring features services...");

        // Generators first to be used by hosted services
        webApplicationBuilder.Services.AddSingleton<GalaxyGeneratorService>();
        webApplicationBuilder.Services.AddHostedService(sp => sp.GetRequiredService<GalaxyGeneratorService>());

        webApplicationBuilder.Services.AddSingleton<UniverseHostedService>();
        webApplicationBuilder.Services.AddHostedService(sp => sp.GetRequiredService<UniverseHostedService>());
        webApplicationBuilder.Services.AddSingleton<CycleBackgroundService>();
        webApplicationBuilder.Services.AddHostedService(sp => sp.GetRequiredService<CycleBackgroundService>());
        webApplicationBuilder.Services.AddSingleton<MapHostedService>();
        webApplicationBuilder.Services.AddHostedService(sp => sp.GetRequiredService<MapHostedService>());

        webApplicationBuilder.Services.AddScoped<LimitService>();
        webApplicationBuilder.Services.AddScoped<BattleService>();
        webApplicationBuilder.Services.AddScoped<OrderService>();
        webApplicationBuilder.Services.AddScoped<OrderExecuteService>();
        webApplicationBuilder.Services.AddScoped<SpaceshipService>();
        webApplicationBuilder.Services.AddScoped<BrandService>();
        webApplicationBuilder.Services.AddScoped<MapService>();
        webApplicationBuilder.Services.AddScoped<ChangelogService>();
        webApplicationBuilder.Services.AddScoped<SpaceshipSkillService>();
        webApplicationBuilder.Services.AddScoped<BuildingService>();
        webApplicationBuilder.Services.AddScoped<GameResourceService>();
        webApplicationBuilder.Services.AddScoped<CelebrityService>();
        webApplicationBuilder.Services.AddScoped<ScenarioService>();
        webApplicationBuilder.Services.AddScoped<EffectService>();
        webApplicationBuilder.Services.AddScoped<EventChoiceService>();
        webApplicationBuilder.Services.AddScoped<GalaxyService>();
        webApplicationBuilder.Services.AddScoped<ModificatorService>();
        webApplicationBuilder.Services.AddScoped<NotificationService>();
        webApplicationBuilder.Services.AddScoped<OriginHistoryService>();
        webApplicationBuilder.Services.AddScoped<PersonalityService>();
        webApplicationBuilder.Services.AddScoped<QuestService>();
        webApplicationBuilder.Services.AddScoped<RecompenseService>();
        webApplicationBuilder.Services.AddScoped<ScenarioChoiceService>();
        webApplicationBuilder.Services.AddScoped<SpaceshipBlueprintService>();
        webApplicationBuilder.Services.AddScoped<SpaceshipModuleService>();
        webApplicationBuilder.Services.AddScoped<SpaceshipService>();
        webApplicationBuilder.Services.AddScoped<SpatialTravelService>();
        webApplicationBuilder.Services.AddScoped<SpeciesService>();
        webApplicationBuilder.Services.AddScoped<StarClusterService>();
        webApplicationBuilder.Services.AddScoped<StarSystemService>();
        webApplicationBuilder.Services.AddScoped<TechnologyService>();
        webApplicationBuilder.Services.AddScoped<EventService>();
        webApplicationBuilder.Services.AddScoped<UniverseService>();
        webApplicationBuilder.Services.AddScoped<MapServiceGenerator>();
        webApplicationBuilder.Services.AddScoped<SpatialLocationService>();

        #region SINGLETON DATA PROVIDER

        webApplicationBuilder.Services.AddSingleton<BrandDataProvider>();
        webApplicationBuilder.Services.AddSingleton<SpaceshipSkillDataProvider>();
        webApplicationBuilder.Services.AddSingleton<BuildingDataProvider>();
        webApplicationBuilder.Services.AddSingleton<GameResourceDataProvider>();
        webApplicationBuilder.Services.AddSingleton<SpeciesDataProvider>();
        webApplicationBuilder.Services.AddSingleton<TechnologyDataProvider>();
        webApplicationBuilder.Services.AddSingleton<ScenarioDataProvider>();
        webApplicationBuilder.Services.AddSingleton<CelebrityDataProvider>();
        webApplicationBuilder.Services.AddSingleton<SpaceshipBlueprintDataProvider>();
        webApplicationBuilder.Services.AddSingleton<EventChoiceDataProvider>();
        webApplicationBuilder.Services.AddSingleton<OriginHistoryDataProvider>();
        webApplicationBuilder.Services.AddSingleton<SpaceshipModuleDataProvider>();
        webApplicationBuilder.Services.AddSingleton<QuestDataProvider>();
        webApplicationBuilder.Services.AddSingleton<EventDataProvider>();

        #endregion

        webApplicationBuilder.Services.AddSingleton<SpaceshipThumbnailProvider>();

        webApplicationBuilder.Services.AddSingleton<CycleService>();
        webApplicationBuilder.Services.AddSingleton<CycleTaskService>();

        webApplicationBuilder.Services.AddSingleton<CycleHub>();

        Log.Logger.Success("Features configured !", EnumLogSeverity.Information);
    }

    public static void ConfigureRepositories(WebApplicationBuilder webApplicationBuilder)
    {
        Log.Logger.Information("Configuring repositories services...");

        webApplicationBuilder.Services.AddScoped<AuthenticationRepository>();
        webApplicationBuilder.Services.AddScoped<UserRepository>();
        webApplicationBuilder.Services.AddScoped<SpacialObjectRepository>();
        webApplicationBuilder.Services.AddScoped<QuestRepository>();
        webApplicationBuilder.Services.AddScoped<ScenarioRepository>();
        webApplicationBuilder.Services.AddScoped<SpaceshipRepository>();
        webApplicationBuilder.Services.AddScoped<StarSystemRepository>();
        webApplicationBuilder.Services.AddScoped<GameResourceRepository>();
        webApplicationBuilder.Services.AddScoped<EventRepository>();
        webApplicationBuilder.Services.AddScoped<TechnologyKnowledgeRepository>();
        webApplicationBuilder.Services.AddScoped<UniverseRepository>();
        webApplicationBuilder.Services.AddScoped<BattleRepository>();
        webApplicationBuilder.Services.AddScoped<GalaxyRepository>();
        webApplicationBuilder.Services.AddScoped<BuildingRepository>();
        webApplicationBuilder.Services.AddScoped<PersonalityRepository>();
        webApplicationBuilder.Services.AddScoped<OriginRepository>();
        webApplicationBuilder.Services.AddScoped<UserRepository>();
        webApplicationBuilder.Services.AddScoped<StarClusterRepository>();
        webApplicationBuilder.Services.AddScoped<SpaceshipBlueprintRepository>();
        webApplicationBuilder.Services.AddScoped<SpeciesRepository>();

        Log.Logger.Success("Repositories configured !", EnumLogSeverity.Information);
    }

    /// <summary>
    /// Force a list of service to start at the application start
    /// </summary>
    public static void ServiceDefaultStart()
    {


    }
}