using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverIDynamicItemScenario))]
public class CardDefinitionIDynamicItemScenario : CardDefinition
{
    public IDynamicItemScenario Scenario { get; set; }

    public CardDefinitionIDynamicItemScenario(IDynamicItemScenario scenario)
    {
        Scenario = scenario;
        Value = scenario;

        Title = scenario.Name;
        Subtitle = "Scenario";

        ImageHeader = ResourceImage.MESSAGE;
        ImageMode = EnumImageMode.Img;
    }
}