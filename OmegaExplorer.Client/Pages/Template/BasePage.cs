using Microsoft.AspNetCore.Components;
using OmegaExplorer.Client.I18nText;
using Toolbelt.Blazor.I18nText;

public abstract class BasePage : ComponentBase
{
    // Injected I18nText service
    [Inject]
    protected I18nText I18NText { get; set; }

    // Shared text model
    protected Translation Translation { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        // Initialize the text table using the I18nText service
        Translation = await I18NText.GetTextTableAsync<Translation>(this);

        // Call the page-specific initialization method
        await OnInitializedPageAsync();
    }

    // Virtual method for additional initialization in derived pages
    protected virtual Task OnInitializedPageAsync() => Task.CompletedTask;
}