using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the user element from the server
/// </summary>
public class DynamicDriverBrand : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new();
        List<CardDefinitionBrand> resultDefinitions = new();
        List<IDynamicItemBrand>? resultApi = await ApiManager.Client.GetAllBrandsAsync();

        foreach (IDynamicItemBrand responseElement in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionBrand(responseElement));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
