using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Buildings.Data._0_100Administration;

public class DynamicItemBuildingAdministration : IDynamicItemBuilding
{
    public const int INDEX = 1;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Administration";

    public string? Description { get; set; } =
        """
        The administration of the city is a complex and intricate web of bureaucracy and politics. 
        The city is divided into districts, each with its own council, and the city as a whole is governed by a council 
        of the most powerful and influential individuals. The administration is responsible for the day-to-day running 
        of the city, and for ensuring that the city is safe and prosperous. The administration is also responsible for the 
        city's defences, and for ensuring that the city is well-defended against any potential threats. The administration 
        is also responsible for the city's laws, and for ensuring that the city is a safe and just place to live. 
        The administration is also responsible for the city's economy, and for ensuring that the city is prosperous 
        and wealthy. The administration is also responsible for the city's infrastructure, and for ensuring that the city 
        is well-maintained and functional. The administration is also responsible for the city's trade, and for ensuring 
        that the city is well-connected to the rest of the world. The administration is also responsible for the city's culture,
         and for ensuring that the city is a vibrant and diverse place to live. The administration is also responsible for 
         the city's education, and for ensuring that the city's citizens are well-educated and knowledgeable. 
         The administration is also responsible for the city's health, and for ensuring that the city's citizens are healthy 
         and well-cared for. The administration is also responsible for the city's welfare, and for ensuring that the city's 
         citizens are well-looked after and supported. The administration is also responsible for the city's justice, 
         and for ensuring that the city's laws are upheld and that justice is served. The administration is also responsible
          for the city's security, and for ensuring that the city is safe and well-protected. The administration is also 
          responsible for the city's diplomacy, and for ensuring that the city is well-regarded and respected by other 
          cities and nations. The administration is also responsible for the city's environment, and for ensuring that the 
          city is clean and healthy. The administration is also responsible for the city's religion, and for ensuring that the 
          city's citizens are free to worship as they please. The administration is also responsible for the city's 
          entertainment, and for ensuring that the city is a fun and exciting place to live. 
          The administration is also responsible for the city's arts, and for ensuring that the city is a creative and 
          inspiring place to live.
        """;

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public int Size { get; set; } = 4;

    public EnumStarSystemType StarSystemCompatibility { get; set; } = EnumStarSystemType.Planet;

    public List<ModifierResource> Cost { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_0_Credit.INDEX,
            Amount = 1000
        }
    };

    public List<ModifierResource> Modifiers { get; set; } = new();

    public List<EnumBuildingAction> Actions { get; set; } = new();

    public IDynamicItemBrand? Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
}