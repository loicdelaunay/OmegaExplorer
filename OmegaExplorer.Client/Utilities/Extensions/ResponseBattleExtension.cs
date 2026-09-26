using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseBattleExtension
{
    public static int GetDuration(this ResponseBattle responseBattle)
    {
        return (int)(responseBattle.StartAtCycle - responseBattle.CurrentCycle);
    }

    public static string GetUrl(this ResponseBattle responseBattle)
    {
        return $"/battle/{responseBattle.Id}";
    }
}
