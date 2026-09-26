using OmegaExplorer.Client.Components;
using Serilog;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class CycleExtension
{
    public static long GetElapsedTurns(long turnStart)
    {
        if (MainLayout.Instance == null)
        {
            Log.Logger.Error($"[{nameof(CycleExtension)}] MainLayout.Instance is null");
            return 0;
        }
        long turnElapsed = MainLayout.Instance.CycleState.CycleCount - turnStart;
        return turnElapsed;
    }
}