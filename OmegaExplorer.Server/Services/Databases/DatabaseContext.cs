using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OmegaExplorer.Server.Services._Core.Comparers;
using OmegaExplorer.Server.Services._Game.Battles.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Entities;
using OmegaExplorer.Server.Services._Game.Cycles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Entities;
using OmegaExplorer.Server.Services._Game.Orders.Models.Entities;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;
using OmegaExplorer.Server.Services._Game.Quests.Models.Entities;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services.Databases.Attributes;
using OmegaExplorer.Server.Services.Jsons.Converter;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.Reflection;

namespace OmegaExplorer.Server.Services.Databases;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orderable>().UseTptMappingStrategy();

        modelBuilder.Entity<SpatialObject>()
                    .ToTable(nameof(SpatialObjects))
                    .HasOne(spatialObject => spatialObject.SpatialLocation)
                    .WithMany()
                    .HasForeignKey(spatialObject => spatialObject.SpatialLocationId)
                    .OnDelete(DeleteBehavior.NoAction);


        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Skip owned types to avoid reconfiguring them as regular entities.
            if (entityType.IsOwned())
            {
                continue;
            }

            foreach (var property in entityType.ClrType.GetProperties())
            {
                if (Attribute.IsDefined(property, typeof(JsonColumnAttribute)))
                {
                    // 1) Creation/instantiation of the JSON converter
                    var propertyBuilder = modelBuilder.Entity(entityType.ClrType).Property(property.Name);
                    var converterType = typeof(JsonValueConverter<>).MakeGenericType(property.PropertyType);
                    var converter = Activator.CreateInstance(converterType) as ValueConverter;
                    propertyBuilder.HasConversion(converter);

                    // Check if the property is a generic List<>
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var elementType = property.PropertyType.GetGenericArguments()[0];
                        // Get the generic method CreateListValueComparer<T> using reflection.
                        var methodInfo = typeof(CreateListValueComparer).GetMethod(nameof(CreateListValueComparer.Process), BindingFlags.Public | BindingFlags.Static);
                        if (methodInfo != null)
                        {
                            var genericMethod = methodInfo.MakeGenericMethod(elementType);
                            var comparer = genericMethod.Invoke(null, null) as ValueComparer;
                            propertyBuilder.Metadata.SetValueComparer(comparer);
                        }
                    }

                    // 2) Define a valuecomompare if it is a dictionary <int, int>
                    if (property.PropertyType == typeof(Dictionary<int, int>))
                    {
                        DictionaryIntIntComparer comparer = new();

                        propertyBuilder.Metadata.SetValueComparer(comparer);
                    }
                }
            }
        }
    }

    public DbSet<User> Users { get; set; }

    public DbSet<GameResource> Resources { get; init; }

    public DbSet<Planet> Planets { get; init; }

    public DbSet<Building> Buildings { get; init; }

    public DbSet<Personality> Personalities { get; init; }

    public DbSet<CycleState> CycleStates { get; init; }

    public DbSet<Order> Orders { get; init; }
    public DbSet<Orderable> Orderables { get; init; }


    public DbSet<Spaceship> Spaceships { get; init; }
    public DbSet<SpaceshipModule> SpaceshipModules { get; init; }


    public DbSet<BlueprintSpaceship> BlueprintSpaceships { get; init; }
    public DbSet<BlueprintSpaceshipModule> BlueprintSpaceshipModules { get; init; }

    public DbSet<SpatialObject> SpatialObjects { get; init; }
    public DbSet<SpatialLocation> SpatialLocations { get; init; }

    public DbSet<Instability> Instabilities { get; init; }
    public DbSet<Universe> Universes { get; set; }
    public DbSet<Galaxy> Galaxies { get; init; }
    public DbSet<StarCluster> StarClusters { get; init; }
    public DbSet<Star> Stars { get; init; }
    public DbSet<StarSystem> StarSystems { get; init; }

    public DbSet<Battle> Battles { get; init; }

    public DbSet<BattleResult> BattleResults { get; init; }

    public DbSet<BattleRewardResult> BattleResultRewards { get; init; }
    public DbSet<ActionInBattle> BattleActions { get; init; }

    public DbSet<Event> Events { get; init; }

    public DbSet<TechnologyKnowledge> TechnologyKnowledges { get; init; }

    public DbSet<QuestProgress> QuestProgresses { get; init; }
    public DbSet<UserScenarioProgress> ScenarioProgresses { get; init; }
}