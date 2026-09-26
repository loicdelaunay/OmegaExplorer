using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Species.Data;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Celebrities.Data._0_1000_Humanity;

public class DynamicItemCelebrity_1_SuperGeneral : DynamicItemBase<DynamicItemCelebrity_1_SuperGeneral>,
    IDynamicItemCelebrity
{
    public override string Name { get; set; } = "Super General";

    public override string? Description { get; set; } = "";
    public override EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public override EnumRarity Rarity { get; set; } = EnumRarity.Origin;

    public bool IsReal { get; set; } = true;

    public EnumCelebrityRole Role { get; set; } =
        EnumCelebrityRole.Designer | EnumCelebrityRole.Founder | EnumCelebrityRole.Helper;

    public IDynamicItemSpecies? Species { get; set; } = DynamicItemSpecies_0_Humanity.Instance;

    protected override int GetIndex()
    {
        return 1;
    }
}