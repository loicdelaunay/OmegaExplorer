using OmegaExplorer.Client.Enums;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseSpaceshipModuleExtension
{
    public static bool IsModuleNextTo(this ResponseSpaceshipModule module, IEnumerable<ResponseSpaceshipModule> modules, Position position)
    {
        int positionX = module.X;
        int positionY = module.Y;

        switch (position)
        {
            case Position.Top:
                positionY++;
                break;
            case Position.Right:
                positionX--;
                break;
            case Position.Bottom:
                positionY--;
                break;
            case Position.Left:
                positionX++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(position), position, null);
        }

        bool exist = modules.Any(m => m.X == positionX && m.Y == positionY);

        return exist;
    }
}