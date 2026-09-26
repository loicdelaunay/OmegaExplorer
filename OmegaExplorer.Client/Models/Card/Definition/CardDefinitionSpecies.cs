using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverSpecies))]
public class CardDefinitionSpecies : CardDefinition
{
    public CardDefinitionSpecies(IDynamicItemSpecies species)
    {
        Species = species;

        if (species.Knowledge != EnumDataKnowledge.Unlocked)
        {
            SetLocked("This species is not yet unlocked.");
        }

        Title = species.Name;
        Subtitle = string.Empty;
        Text = species.Description;
        ImageHeader = GetImage();

        Rarity = species.Rarity;
    }

    public IDynamicItemSpecies Species { get; set; }

    private string GetImage()
    {
        return ResourceImage.GetImageSpeciesIllustration(Species.Index);
    }
}
