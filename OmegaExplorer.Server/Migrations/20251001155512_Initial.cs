using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmegaExplorer.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CycleStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CycleCount = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CycleStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagedByPersonalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonalityId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagedByPersonalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orderables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orderables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    DateLastConnection = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CurrentTechnologyResearchIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    Preferences = table.Column<string>(type: "TEXT", nullable: false),
                    AccessLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Origin_IndexSpecies = table.Column<int>(type: "INTEGER", nullable: true),
                    Origin_IndexHistory = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Galaxies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    UniverseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galaxies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Galaxies_Universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "Universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlueprintSpaceships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlueprintSpaceships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlueprintSpaceships_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventItemIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TargetType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ManagedByPersonalityId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Experience = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    PortraitIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personalities_ManagedByPersonalities_ManagedByPersonalityId",
                        column: x => x.ManagedByPersonalityId,
                        principalTable: "ManagedByPersonalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Personalities_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestObjectiveProgresses = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    Balance = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Choices = table.Column<string>(type: "TEXT", nullable: false),
                    IsResolved = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyKnowledges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TechnologyIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Knowledge = table.Column<int>(type: "INTEGER", nullable: false),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologyKnowledges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologyKnowledges_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlueprintSpaceshipModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BlueprintSpaceshipId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlueprintSpaceshipModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlueprintSpaceshipModules_BlueprintSpaceships_BlueprintSpaceshipId",
                        column: x => x.BlueprintSpaceshipId,
                        principalTable: "BlueprintSpaceships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionInBattleSpaceship",
                columns: table => new
                {
                    ActionsAsTargetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionInBattleSpaceship", x => new { x.ActionsAsTargetId, x.TargetsId });
                });

            migrationBuilder.CreateTable(
                name: "BattleActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BattleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActorSpaceshipId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Turn = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Cycle = table.Column<int>(type: "INTEGER", nullable: false),
                    Result_DiceActor = table.Column<int>(type: "INTEGER", nullable: true),
                    Result_DiceTarget = table.Column<int>(type: "INTEGER", nullable: true),
                    Result_Dodge = table.Column<bool>(type: "INTEGER", nullable: true),
                    Result_Absorb = table.Column<bool>(type: "INTEGER", nullable: true),
                    Result_Bounce = table.Column<bool>(type: "INTEGER", nullable: true),
                    Result_Critical = table.Column<bool>(type: "INTEGER", nullable: true),
                    Result_Value = table.Column<int>(type: "INTEGER", nullable: true),
                    Result_ModuleIds = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleResultRewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IndexResource = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    BattleResultId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleResultRewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BattleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerWinnerIds = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerLoserIds = table.Column<string>(type: "TEXT", nullable: false),
                    FactionWinners = table.Column<string>(type: "TEXT", nullable: false),
                    FactionLosers = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartAtCycle = table.Column<long>(type: "INTEGER", nullable: false),
                    EndAtCycle = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    SystemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEdition = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastAccess = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeletion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberAccess = table.Column<int>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisableReason = table.Column<string>(type: "TEXT", nullable: true),
                    CycleWhenRemove = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TypeInstability = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderableId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetRaw = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    TargetLocationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TargetSpaceshipId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Orderables_OrderableId",
                        column: x => x.OrderableId,
                        principalTable: "Orderables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpaceshipModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SpaceshipId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    IndexBrand = table.Column<int>(type: "INTEGER", nullable: true),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceshipModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spaceships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DestroyedQuantityDamageToRepair = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnerType = table.Column<int>(type: "INTEGER", nullable: false),
                    Faction = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    BattleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Species = table.Column<string>(type: "TEXT", nullable: false),
                    AutoBattle = table.Column<bool>(type: "INTEGER", nullable: false),
                    RawBlueprint = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spaceships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spaceships_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Spaceships_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpatialLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UniverseId = table.Column<Guid>(type: "TEXT", nullable: true),
                    GalaxyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    StarClusterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Position_X = table.Column<int>(type: "INTEGER", nullable: false),
                    Position_Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpatialLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpatialLocations_Galaxies_GalaxyId",
                        column: x => x.GalaxyId,
                        principalTable: "Galaxies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpatialLocations_Universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "Universes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpatialObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SpeedInUniverse = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedInGalaxy = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedInStarCluster = table.Column<int>(type: "INTEGER", nullable: false),
                    SpatialLocationId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpatialObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpatialObjects_Orderables_Id",
                        column: x => x.Id,
                        principalTable: "Orderables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpatialObjects_SpatialLocations_SpatialLocationId",
                        column: x => x.SpatialLocationId,
                        principalTable: "SpatialLocations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StarClusters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarClusters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarClusters_SpatialObjects_Id",
                        column: x => x.Id,
                        principalTable: "SpatialObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StarSystems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanetType = table.Column<int>(type: "INTEGER", nullable: false),
                    Modifiers = table.Column<string>(type: "TEXT", nullable: false),
                    Species = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarSystems_SpatialObjects_Id",
                        column: x => x.Id,
                        principalTable: "SpatialObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StarSystems_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stars_StarSystems_Id",
                        column: x => x.Id,
                        principalTable: "StarSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionInBattleSpaceship_TargetsId",
                table: "ActionInBattleSpaceship",
                column: "TargetsId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleActions_ActorSpaceshipId",
                table: "BattleActions",
                column: "ActorSpaceshipId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleActions_BattleId",
                table: "BattleActions",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleResultRewards_BattleResultId",
                table: "BattleResultRewards",
                column: "BattleResultId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleResults_BattleId",
                table: "BattleResults",
                column: "BattleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintSpaceshipModules_BlueprintSpaceshipId",
                table: "BlueprintSpaceshipModules",
                column: "BlueprintSpaceshipId");

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintSpaceships_OwnerId",
                table: "BlueprintSpaceships",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_SystemId",
                table: "Buildings",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Galaxies_UniverseId",
                table: "Galaxies",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderableId",
                table: "Orders",
                column: "OrderableId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OwnerUserId",
                table: "Orders",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TargetLocationId",
                table: "Orders",
                column: "TargetLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TargetSpaceshipId",
                table: "Orders",
                column: "TargetSpaceshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Personalities_ManagedByPersonalityId",
                table: "Personalities",
                column: "ManagedByPersonalityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personalities_OwnerId",
                table: "Personalities",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestProgresses_UserId",
                table: "QuestProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_UserId",
                table: "Resources",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioProgresses_UserId",
                table: "ScenarioProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceshipModules_SpaceshipId",
                table: "SpaceshipModules",
                column: "SpaceshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Spaceships_BattleId",
                table: "Spaceships",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Spaceships_OwnerId",
                table: "Spaceships",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SpatialLocations_GalaxyId",
                table: "SpatialLocations",
                column: "GalaxyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpatialLocations_StarClusterId",
                table: "SpatialLocations",
                column: "StarClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_SpatialLocations_UniverseId",
                table: "SpatialLocations",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_SpatialObjects_SpatialLocationId",
                table: "SpatialObjects",
                column: "SpatialLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StarSystems_OwnerId",
                table: "StarSystems",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologyKnowledges_UserId",
                table: "TechnologyKnowledges",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionInBattleSpaceship_BattleActions_ActionsAsTargetId",
                table: "ActionInBattleSpaceship",
                column: "ActionsAsTargetId",
                principalTable: "BattleActions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionInBattleSpaceship_Spaceships_TargetsId",
                table: "ActionInBattleSpaceship",
                column: "TargetsId",
                principalTable: "Spaceships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleActions_Battles_BattleId",
                table: "BattleActions",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleActions_Spaceships_ActorSpaceshipId",
                table: "BattleActions",
                column: "ActorSpaceshipId",
                principalTable: "Spaceships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleResultRewards_BattleResults_BattleResultId",
                table: "BattleResultRewards",
                column: "BattleResultId",
                principalTable: "BattleResults",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleResults_Battles_BattleId",
                table: "BattleResults",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_SpatialObjects_Id",
                table: "Battles",
                column: "Id",
                principalTable: "SpatialObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_StarSystems_SystemId",
                table: "Buildings",
                column: "SystemId",
                principalTable: "StarSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instabilities_StarSystems_Id",
                table: "Instabilities",
                column: "Id",
                principalTable: "StarSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Spaceships_TargetSpaceshipId",
                table: "Orders",
                column: "TargetSpaceshipId",
                principalTable: "Spaceships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_SpatialLocations_TargetLocationId",
                table: "Orders",
                column: "TargetLocationId",
                principalTable: "SpatialLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Planets_StarSystems_Id",
                table: "Planets",
                column: "Id",
                principalTable: "StarSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpaceshipModules_Spaceships_SpaceshipId",
                table: "SpaceshipModules",
                column: "SpaceshipId",
                principalTable: "Spaceships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Spaceships_SpatialObjects_Id",
                table: "Spaceships",
                column: "Id",
                principalTable: "SpatialObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpatialLocations_StarClusters_StarClusterId",
                table: "SpatialLocations",
                column: "StarClusterId",
                principalTable: "StarClusters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarClusters_SpatialObjects_Id",
                table: "StarClusters");

            migrationBuilder.DropTable(
                name: "ActionInBattleSpaceship");

            migrationBuilder.DropTable(
                name: "BattleResultRewards");

            migrationBuilder.DropTable(
                name: "BlueprintSpaceshipModules");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "CycleStates");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Instabilities");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Personalities");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "QuestProgresses");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "ScenarioProgresses");

            migrationBuilder.DropTable(
                name: "SpaceshipModules");

            migrationBuilder.DropTable(
                name: "Stars");

            migrationBuilder.DropTable(
                name: "TechnologyKnowledges");

            migrationBuilder.DropTable(
                name: "BattleActions");

            migrationBuilder.DropTable(
                name: "BattleResults");

            migrationBuilder.DropTable(
                name: "BlueprintSpaceships");

            migrationBuilder.DropTable(
                name: "ManagedByPersonalities");

            migrationBuilder.DropTable(
                name: "StarSystems");

            migrationBuilder.DropTable(
                name: "Spaceships");

            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SpatialObjects");

            migrationBuilder.DropTable(
                name: "Orderables");

            migrationBuilder.DropTable(
                name: "SpatialLocations");

            migrationBuilder.DropTable(
                name: "Galaxies");

            migrationBuilder.DropTable(
                name: "StarClusters");

            migrationBuilder.DropTable(
                name: "Universes");
        }
    }
}
