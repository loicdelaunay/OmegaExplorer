namespace OmegaExplorer.Client.Models;

public class Pagination
{
    /// <summary>
    /// Current page selected
    /// </summary>
    public int SelectedPage { get; set; } = 0;

    /// <summary>
    /// Total page available
    /// </summary>
    public int TotalPages { get; set; } = 1;

    /// <summary>
    /// Number of page to show 
    /// </summary>
    public int OptionPageSize { get; set; } = 50;
}