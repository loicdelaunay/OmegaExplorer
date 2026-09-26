using Microsoft.JSInterop;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Configuration;
using OmegaExplorer.Toolkit.API;
using Serilog;
using System.Net;

namespace OmegaExplorer.Client.Services;

public class JavascriptProvider
{
    private readonly IJSRuntime _jsRuntime;

    public JavascriptProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    ///     Activate additive functions from native javascript to element with specific class
    ///     class is : '.diagram-canvas'
    /// </summary>
    public void DiagramExtensionEnable()
    {
        _jsRuntime.InvokeVoidAsync("updateDiagramExtension");
    }

    public async Task SaveAsFile(string fileName, string content, string type = "application/json")
    {
        await _jsRuntime.InvokeVoidAsync("saveAsFile", fileName, type, content);
    }

    public async Task PushLog(string message, string style)
    {
        await _jsRuntime.InvokeVoidAsync("customLogging.logStyled",
                                         message,
                                         style);
    }

    public async Task TriggerConfetti(string gainExplosion)
    {
        await _jsRuntime.InvokeVoidAsync("triggerConfetti", "gain-explosion");
    }

    public async Task LoginGoogle()
    {
        try
        {
            string clientId = ConfigurationApplication.ConfigurationApi.GoogleClientId;

            // Call JavaScript function to trigger Google Sign-In
            string idToken = await _jsRuntime.InvokeAsync<string>("googleSignIn", clientId);

            // Send the ID token to the API for validation
            RequestAuthGoogleLogin req = new RequestAuthGoogleLogin()
            {
                TokenId = idToken
            };

            ResponseUser? user = await ApiManager.Client.GoogleLoginAsync(req);
            await ApiAuth.SetUserAsActive(user);

            Log.Logger.Information($"[JavascriptProvider] : user is {user?.Email}");
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error login google");
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}