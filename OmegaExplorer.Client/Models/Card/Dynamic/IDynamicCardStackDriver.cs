using OmegaExplorer.Client.Managers;
using OmegaExplorer.Shared.Filters;

namespace OmegaExplorer.Client.Models.Card.Dynamic;

public interface IDynamicCardStackDriver
{
    /// <summary>
    /// Get elements paged with server system
    /// Please register it in <see cref="DynamicCardStackDriverManager"/>
    /// </summary>
    /// <param name="pagination"> </param>
    /// <returns> </returns>
    public Task<ResultPagined?> Get(Pagination pagination, DynamicDriverFilter filters);
}
