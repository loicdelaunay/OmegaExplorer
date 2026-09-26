using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Species.Data;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Celebrities.Data._10001_11000_Specials;

public class DynamicItemCelebrity_10001_Dream : IDynamicItemCelebrity
{
    public const int INDEX = 10001;
    public static readonly DynamicItemCelebrity_10001_Dream Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Dream";

    public string? Description { get; set; } =
        "Dream is an entrepreneurial space exploration game where players take on the role of a visionary founder who has launched numerous starship brands. Build, innovate, and dominate the galactic market by creating cutting-edge spacecraft that redefine the future of interstellar travel. Your journey is not just about exploration but shaping the very fabric of the cosmos through groundbreaking designs and strategic business acumen.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Origin;

    public bool IsReal { get; set; } = true;

    public EnumCelebrityRole Role { get; set; } =
        EnumCelebrityRole.Designer | EnumCelebrityRole.Founder | EnumCelebrityRole.Helper;

    public IDynamicItemSpecies? Species { get; set; } = DynamicItemSpecies_0_Humanity.Instance;
}