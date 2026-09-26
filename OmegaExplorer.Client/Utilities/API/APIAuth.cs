using OmegaExplorer.Client.Components;
using OmegaExplorer.Client.Managers;
using OmegaExplorer.Client.Utilities.Enums;
using OmegaExplorer.Toolkit.API;
using Serilog;
using System.Net;

namespace OmegaExplorer.Client.Utilities.API;

/// <summary>
///     Api auth manager
/// </summary>
public static class ApiAuth
{
    #region Properties

    /// <summary>
    ///     The user connected
    /// </summary>
    public static ResponseUser? UserConnected { get; set; }

    /// <summary>
    ///     Security token saved in memory
    /// </summary>
    public static string Token { get; set; }

    /// <summary>
    ///     When api is connecting
    /// </summary>
    public static bool IsConnecting { get; set; }

    /// <summary>
    /// Check if ui client try to manage auth token to let
    /// timer to refresh token
    /// </summary>
    public static bool IsClientConnecting { get; set; }

    public static bool IsClientWaiting = true;

    /// <summary>
    ///     If user is connected
    /// </summary>
    public static bool IsConnected => UserConnected != null;

    public static bool IsAdmin
    {
        get { return UserConnected?.AccessLevel == EnumUserAccessLevel.Admin; }
    }

    #endregion

    #region Events

    /// <summary>
    ///     When the user is connected
    /// </summary>
    public static Action<ResponseUser>? OnConnected { get; set; }

    /// <summary>
    ///     When user is calling the disconnect function
    /// </summary>
    public static Action? OnDisconnected { get; set; }

    public static Action? OnConnectionStateChanged { get; set; }

    #endregion

    /// <summary>
    ///     The Connect function is used to connect a user to the server.
    ///     &lt;para&gt;It takes two parameters: email and password.&lt;/para&gt;
    ///     &lt;para&gt;The function returns an EnumResultAuthConnect value, which can be Success, NotFound or Failed.&lt;/para
    ///     &gt;
    /// </summary>
    /// <param name="string email"> Email of the user</param>
    /// <param name="string password"> The password of the user</param>
    /// <returns> An enumresultauthconnect enum</returns>
    public static async Task<EnumResultAuthConnect> Connect(string email, string password)
    {
        if (ApiManager.Client == null) throw new Exception("Api not initialized");
        IsConnecting = true;

        try
        {
            RequestAuthLogin authRequest = new RequestAuthLogin
            {
                Email = email,
                Password = password
            };

            try
            {
                ResponseUser? res = await ApiManager.Client.LoginAsync(authRequest);

                if (res == null)
                {
                    return EnumResultAuthConnect.NotFound;
                }

                await SetUserAsActive(user: res);

                ConnectStateChanged();
                return EnumResultAuthConnect.Success;
            }
            catch (SwaggerException swaggerException)
            {
                if (swaggerException.StatusCode == (int)HttpStatusCode.Forbidden) return EnumResultAuthConnect.PasswordIncorrect;

                if (swaggerException.StatusCode == (int)HttpStatusCode.Unauthorized) return EnumResultAuthConnect.Disabled;

                ConnectStateChanged();
                return EnumResultAuthConnect.Unknown;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                ConnectStateChanged();
                return EnumResultAuthConnect.Failed;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error not able to connect : " + e.Message);

            ConnectStateChanged();
            return EnumResultAuthConnect.Failed;
        }
    }

    public static async Task<EnumResultAuthConnect> Connect(string token)
    {
        if (ApiManager.Client == null) throw new Exception("Api not initialized");
        IsConnecting = true;

        RequestAuthLoginWithToken req = new RequestAuthLoginWithToken
        {
            Token = token
        };

        try
        {
            ResponseUser? res = await ApiManager.Client.LoginWithTokenAsync(req);

            if (res == null)
            {
                return EnumResultAuthConnect.NotFound;
            }

            await SetUserAsActive(user: res);

            ConnectStateChanged();
            return EnumResultAuthConnect.Success;
        }
        catch (SwaggerException swaggerException)
        {
            if (swaggerException.StatusCode == (int)HttpStatusCode.Forbidden) return EnumResultAuthConnect.PasswordIncorrect;

            if (swaggerException.StatusCode == (int)HttpStatusCode.Unauthorized) return EnumResultAuthConnect.Disabled;

            ConnectStateChanged();
            return EnumResultAuthConnect.Unknown;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            ConnectStateChanged();
            return EnumResultAuthConnect.Failed;
        }
    }

    public static async Task SetUserAsActive(ResponseUser user)
    {
        UserConnected = user;
        ApiManager.SetJwtToken(user.Token);

        //Save token in secured memory
        string? token = ApiAuth.UserConnected.Token;

        //Save in secured storage
        UserSettings? userSettings = await UserSettingsManager.GetUserSettings();
        userSettings.Token = token;
        UserSettingsManager.SetUserSettings(userSettings);
    }

    private static void ConnectStateChanged()
    {
        IsConnecting = false;
        OnConnected?.Invoke(UserConnected);
        OnConnectionStateChanged?.Invoke();
    }

    /// <summary>
    /// Disconnect the user from the API.
    /// <!> <see cref="CheckConnection" redirect on event OnDisconnected event/>
    /// </summary>
    public static async Task Disconnect()
    {
        UserConnected = null;

        //Clear token in secured memory
        var userSettings = await UserSettingsManager.GetUserSettings();

        if (userSettings != null)
        {
            userSettings.Token = null;
            UserSettingsManager.SetUserSettings(userSettings);
        }

        OnDisconnected?.Invoke();
    }

    public static async Task AwaitConnection()
    {
        if (IsConnected)
        {
            return;
        }

        Log.Logger.Information($"[{nameof(ApiAuth)}] : awaiting connection...");

        //30 seconds 300*100 ms
        int maxLoop = 100;

        while (IsClientWaiting && maxLoop > 0)
        {
            maxLoop--;
            await Task.Delay(300);
        }

        while (IsClientConnecting && maxLoop > 0)
        {
            maxLoop--;
            await Task.Delay(300);
        }

        while (IsConnecting && maxLoop > 0)
        {
            maxLoop--;
            await Task.Delay(300);
        }

        if (maxLoop == 0)
        {
            Log.Logger.Error($"[{nameof(ApiAuth)}] : connection timeout ! Check if component {nameof(CheckConnection)} is correctly set in the page.");
        }
    }

    public static async Task RefreshUser()
    {
        if (UserConnected == null) return;

        RequestAuthLoginWithToken req = new RequestAuthLoginWithToken
        {
            Token = UserConnected.Token
        };

        ResponseUser? res = await ApiManager.Client.LoginWithTokenAsync(req);

        if (res == null)
        {
            await Disconnect();
            return;
        }

        await SetUserAsActive(user: res);
    }
}