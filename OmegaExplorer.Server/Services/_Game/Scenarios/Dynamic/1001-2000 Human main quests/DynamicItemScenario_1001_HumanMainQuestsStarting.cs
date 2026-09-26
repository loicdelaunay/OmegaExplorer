using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Data._0_1000_Humanity;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Recompenses.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Enums;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Data;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Data.Human;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Dynamic._1001_2000_Human_main_quests;

public class DynamicItemScenario_1001_HumanMainQuestsStarting : IDynamicItemScenario
{
    public const int INDEX = 1001;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }


    public string Name { get; set; } = "Main contact";

    public string? Description { get; set; }

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; }

    public List<ScenarioDialog> Dialogs { get; set; } = new()
    {
        new ScenarioDialog
        {
            Index = 1,
            Celebrity = DynamicItemCelebrity_1_SuperGeneral.Instance,
            Message =
                """
                Hello, I am the super general, i manage you has the president of the new colony for the humanity,
                you have to manage the colony and i have to manage you managing the colony ok ? So first i give you the name 
                of you future planet and you have to manage the rest, the name is :
                """,
            ScenarioChoices = new List<IDynamicItemScenarioChoice>
            {
                IDynamicItemScenarioChoice_0_OkMyGeneral.Instance,
                IDynamicItemScenarioChoice_1_NotSureMyGeneral.Instance
            }
        },

        new ScenarioDialog
        {
            Index = 2,
            Celebrity = DynamicItemCelebrity_1_SuperGeneral.Instance,
            Message = "Excellent! I am pleased with your response. Let's move forward with our plans.",
            PreviousChoice = IDynamicItemScenarioChoice_0_OkMyGeneral.Instance,
            Recompenses = new List<Recompense>
            {
                new()
                {
                    Index = DynamicItemSpaceshipBlueprint_2_TsunamiColonizer.INDEX,
                    Amount = 1,
                    Type = Recompense.RecompenseType.Spaceship
                }
            }
        },

        new ScenarioDialog
        {
            Index = 3,
            Celebrity = DynamicItemCelebrity_1_SuperGeneral.Instance,
            Message =
                "I don't care ! It's a yes for me the scenario programmatic of this dialog is not able to manage a NO ! I am pleased with your response. Let's move forward with our plans.",
            PreviousChoice = IDynamicItemScenarioChoice_1_NotSureMyGeneral.Instance,
            Recompenses = new List<Recompense>
            {
                new()
                {
                    Index = DynamicItemSpaceshipBlueprint_2_TsunamiColonizer.INDEX,
                    Amount = 1,
                    Type = Recompense.RecompenseType.Spaceship
                }
            }
        }
    };

    public IDynamicItemCelebrity? MainContact { get; set; } = DynamicItemCelebrity_1_SuperGeneral.Instance;

    public bool Priority { get; set; } = true;

    public Task OnResolved()
    {
        //Give a random planet to the player
        return Task.CompletedTask;
    }

    public EnumScenarioRepetition ScenarioRepetition { get; set; } = EnumScenarioRepetition.Single;

    public EnumScenarioDistribution ScenarioDistribution { get; set; } = EnumScenarioDistribution.Mandatory;
}