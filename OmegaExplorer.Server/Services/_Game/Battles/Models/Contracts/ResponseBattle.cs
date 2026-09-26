using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

public class ResponseBattle : ResponseSpatialObject
{
    public List<ResponseSpaceship> Spaceships { get; set; } = new();

    public List<ResponseActionInBattle> Actions { get; set; } = new();

    public long StartAtCycle { get; set; }

    public long EndAtCycle { get; set; }

    public int CurrentCycle { get; set; }

    public ResponseBattleResult? Result { get; set; }
}