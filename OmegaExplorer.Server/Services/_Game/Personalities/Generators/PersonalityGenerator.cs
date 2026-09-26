using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services.ProjectFiles;

namespace OmegaExplorer.Server.Services._Game.Personalities.Generators;

public class PersonalityGenerator : IInitializable
{
    private PersonalityGeneratorContent _personalityGeneratorContent = new();
    public bool IsInitialized { get; set; }

    public void Initialize()
    {
        var file = FileProjectManager.GetProjectFile("Game/Personality/_data/personality.naming.json");

        _personalityGeneratorContent = file.ReadJson<PersonalityGeneratorContent>();
    }

    public void Uninitialize()
    {
    }
}