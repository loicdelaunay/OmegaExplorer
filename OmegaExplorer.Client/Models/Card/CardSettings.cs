namespace OmegaExplorer.Client.Models.Card;

public class CardSettings
{
    public bool IsCheckable = false;
    public int HeaderImageSizeFactor = 10;

    public bool HeaderImageFluid { get; set; } = true;
    public bool IsLocked { get; set; }

    public string LockedReason { get; set; } = string.Empty;
}
