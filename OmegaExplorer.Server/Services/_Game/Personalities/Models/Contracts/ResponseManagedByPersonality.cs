using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;

namespace OmegaExplorer.Server.Services._Game.Personalities.Models.Contracts;

public class ResponseManagedByPersonality : ResponseMetadata
{
    public ResponsePersonality Personality { get; set; }
}