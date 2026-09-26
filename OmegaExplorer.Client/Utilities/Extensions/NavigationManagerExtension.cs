using Microsoft.AspNetCore.Components;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class NavigationManagerExtension
{
    public static void NavigateToPlanet(this NavigationManager navigationManager, Guid idPlanet)
    {
        navigationManager.NavigateTo($"/planet/{idPlanet}");
    }

    public static void NavigateToPage<T>(this NavigationManager navigationManager) where T : ComponentBase
    {
        RouteAttribute? routeAttribute = typeof(T).GetCustomAttributes(false)
                                                  .OfType<RouteAttribute>()
                                                  .FirstOrDefault();

        if (routeAttribute != null)
        {
            navigationManager.NavigateTo(routeAttribute.Template);
        }
        else
        {
            throw new InvalidOperationException($"No route found for {typeof(T).Name}");
        }
    }
}