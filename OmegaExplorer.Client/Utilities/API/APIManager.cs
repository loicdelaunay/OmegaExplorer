#region

using OmegaExplorer.Client.Utilities.API.Models;
using OmegaExplorer.Toolkit.API;
using Serilog;
using System.Net.Http.Headers;

#endregion

namespace OmegaExplorer.Client.Utilities.API;

public static class ApiManager
{
    private static HttpClient? _httpClient;
    private static string _token = string.Empty;
    private static OmegaExplorerAPI _client;
    private static bool _isInitialized;
    private static string _urlServer;


    public static OmegaExplorerAPI Client
    {
        get
        {
            if (_client == null || !_isInitialized) throw new Exception("Api Manager not initialized, please call APIManager.Initialize()");

            return _client;
        }
    }

    public static async Task<ResultInitialize> Initialize(string url)
    {
        ResultInitialize res = new ResultInitialize();

        Console.WriteLine($"[{nameof(ApiManager)}] : Initializing API Manager targeting : {url}");

        try
        {
            _client = new OmegaExplorerAPI(url, GetHttpClient());
            _urlServer = url;

            _isInitialized = true;
        }
        catch (Exception e)
        {
            res.Exception = e;
            res.Result = ResultInitialize.EnumInitializeResult.Error;

            Console.WriteLine("API initialization failed");
            return res;
        }

        Console.WriteLine($"[{nameof(ApiManager)}] : API initialized");
        return res;
    }

    public static void SetJwtToken(string token)
    {
        _token = token;

        GenerateHttpClient();

        // Set the client with the good jwt token to request server
        _client = new OmegaExplorerAPI(_urlServer, GetHttpClient());
    }

    public static void ClearJwtToken()
    {
        //Rebuild http client with no auth
        _token = string.Empty;
        GenerateHttpClient();

        //Rebuild the client with new http client
        _client = new OmegaExplorerAPI(_urlServer, GetHttpClient());
        Console.WriteLine("Jwt token cleared from http client");
    }

    public static HttpClient GetHttpClient(int timeOutInSeconds = 120, bool forceGenerateNewHttpClient = false, bool setAsDefault = true)
    {
        HttpClient res;

        if (_httpClient == null && !forceGenerateNewHttpClient)
            res = GenerateHttpClient(timeOutInSeconds: timeOutInSeconds, setAsDefault: setAsDefault);
        else
            res = _httpClient;

        return res;
    }

    private static HttpClient GenerateHttpClient(bool ignoreServerCertificateValidation = true, int timeOutInSeconds = 120, bool setAsDefault = true)
    {
        HttpClientHandler clientHandler = new HttpClientHandler();

        try
        {
            if (ignoreServerCertificateValidation) clientHandler.ServerCertificateCustomValidationCallback += delegate { return true; };
        }
        catch (Exception e)
        {
            // ignored
        }

        HttpClient httpClient = new HttpClient(clientHandler);
        httpClient.Timeout = TimeSpan.FromSeconds(timeOutInSeconds);

        if (!string.IsNullOrEmpty(_token)) httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        if (setAsDefault) _httpClient = httpClient;

        return httpClient;
    }

    public static string GetJwtToken()
    {
        return _token;
    }
}
