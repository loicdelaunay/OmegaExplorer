using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseOrderExtension
{
    public static string ToStringHumanized(this ResponseOrder order)
    {
        string? target = string.Empty;

        switch (order.Type)
        {
            case OrderType.Attack:
                target = order.TargetSpaceship?.Name ?? order.TargetRaw;
                break;
            case OrderType.Defend:
                target = order.TargetSpaceship?.Name ?? order.TargetRaw;
                break;
            case OrderType.Move:
                target = order.TargetLocation?.ToStringFormated(simple: true) ?? order.TargetRaw;
                break;
            case OrderType.Wait:
                break;
            case OrderType.Colonize:
                target = order.TargetLocation?.ToStringFormated(simple: true) ?? order.TargetRaw;
                break;
            default:
                target = order.TargetRaw;
                break;
        }

        string res = $"{order.Type} {target}";

        return res;
    }
}
