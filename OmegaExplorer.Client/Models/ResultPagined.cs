using OmegaExplorer.Client.Models.Card;

namespace OmegaExplorer.Client.Models;

public class ResultPagined
{
    /// <summary>
    /// Current pagination
    /// </summary>
    public Pagination Pagination { get; set; }

    /// <summary>
    /// All results
    /// </summary>
    public IEnumerable<CardDefinition> Data { get; set; }
}