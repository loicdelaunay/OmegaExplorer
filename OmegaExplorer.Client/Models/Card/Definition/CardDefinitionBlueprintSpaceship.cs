#region

using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Configuration;
using OmegaExplorer.Toolkit.API;

#endregion

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverBlueprintSpaceship))]
public class CardDefinitionBlueprintSpaceship : CardDefinition
{
    public CardDefinitionBlueprintSpaceship(ResponseBlueprintSpaceship blueprintSpaceship)
    {
        Value = blueprintSpaceship;
        BlueprintBlueprintSpaceship = blueprintSpaceship;

        Title = blueprintSpaceship.Name;
        Subtitle = string.Empty;

        Owner = blueprintSpaceship.Owner;

        ImageHeader = GetThumbnailUrl();
        ImageMode = EnumImageMode.Img;

        Contents.Add(new CardContent("Url", blueprintSpaceship.Thumbnail));
    }

    public string GetThumbnailUrl()
    {
        string target = ConfigurationApplication.ConfigurationApi.Url + "/Static/Spaceship/Thumbnail/" + BlueprintBlueprintSpaceship.Thumbnail + ".png";
        Console.WriteLine("Target is : " + target);
        return target;
    }

    public ResponseBlueprintSpaceship BlueprintBlueprintSpaceship { get; set; }
}