#region

using System.Security.Claims;

#endregion

namespace OmegaExplorer.Server.Extensions;

public static class ClaimPrincipaleExtension
{
    /// <summary>
    ///     Return email from the GWToken
    /// </summary>
    /// <param name="source"> </param>
    /// <returns> </returns>
    public static string GetEmail(this ClaimsPrincipal source)
    {
        string? res = source.FindFirst(ClaimTypes.Email)?.Value;
        return res;
    }
}