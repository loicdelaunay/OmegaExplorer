#region

using Microsoft.AspNetCore.Mvc;
using System.Net;

#endregion

namespace OmegaExplorer.Server.Services.Servers.Models.Classes;

/// <summary>
///     Return a formatted exception to the client
/// </summary>
public class HttpResponseException : ActionResult
{
    /// <summary>
    /// </summary>
    /// <param name="message"> </param>
    /// <param name="code"> </param>
    public HttpResponseException(string message, int code)
    {
        Message = message;
        Code = code;
    }

    /// <summary>
    /// </summary>
    /// <param name="message"> </param>
    /// <param name="code"> </param>
    public HttpResponseException(string message)
    {
        Message = message;
        Code = (int)HttpStatusCode.Forbidden;
    }

    /// <summary>
    /// </summary>
    /// <param name="code"> </param>
    public HttpResponseException(int code)
    {
        Message = "no details";
        Code = code;
    }

    /// <summary>
    ///     Details of the exception
    /// </summary>
    public string Message { get; set; }


    /// <summary>
    ///     Exception code
    /// </summary>
    public int Code { get; set; }
}