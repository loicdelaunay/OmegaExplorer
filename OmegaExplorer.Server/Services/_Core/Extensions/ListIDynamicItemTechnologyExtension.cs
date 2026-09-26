using OmegaExplorer.Server.Services._Game.Technologies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Core.Extensions;

public static class ListIDynamicItemTechnologyExtension
{
    public static void GetTechnologiesWithProgress(this List<IDynamicItemTechnology> allTechnologies, List<TechnologyKnowledge> progress)
    {
        //Get progress to all tech viewed by the user
        foreach (var technology in allTechnologies)
        {
            var tech = progress.FirstOrDefault(x => x.TechnologyIndex == technology.Index);

            if (tech != null)
            {
                technology.Knowledge = tech.Knowledge;
            }
        }

        //Set researchable if not already researched and if all requirements are met
        foreach (var technologyToCheck in allTechnologies)
        {
            if (technologyToCheck.Knowledge != EnumDataKnowledge.Researched && technologyToCheck.Knowledge != EnumDataKnowledge.Unlocked)
            {
                //Check requirements
                if (technologyToCheck.RequiredTechnologies.Any())
                {
                    //If one technology is not researched, the technology is not researchable
                    if (allTechnologies.Any(technology => technologyToCheck.RequiredTechnologies.Exists(tech => tech.Index == technology.Index) && technology.Knowledge != EnumDataKnowledge.Researched))
                    {
                        technologyToCheck.Knowledge = EnumDataKnowledge.Locked;
                        continue;
                    }

                    //If all requirements are met, the technology is researchable
                    technologyToCheck.Knowledge = EnumDataKnowledge.Unlocked;
                }
                else
                {
                    // If no requirements, the technology is researchable
                    technologyToCheck.Knowledge = EnumDataKnowledge.Researched;
                }
            }
        }
    }
}