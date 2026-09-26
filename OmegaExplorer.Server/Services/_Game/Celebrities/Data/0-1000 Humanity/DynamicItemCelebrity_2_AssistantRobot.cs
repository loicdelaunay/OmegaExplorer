using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Species.Data;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Celebrities.Data._0_1000_Humanity;

public class DynamicItemCelebrity_2_AssistantRobot : IDynamicItemCelebrity
{
    public const int INDEX = 2;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Assistant robot";

    public string? Description { get; set; } = "";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Origin;

    public bool IsReal { get; set; } = true;

    public EnumCelebrityRole Role { get; set; } =
        EnumCelebrityRole.Designer | EnumCelebrityRole.Founder | EnumCelebrityRole.Helper;

    public IDynamicItemSpecies? Species { get; set; } = DynamicItemSpecies_0_Humanity.Instance;
}