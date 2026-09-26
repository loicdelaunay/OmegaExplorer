#region

using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Users;

#endregion

namespace OmegaExplorer.Server.Services.Jwt.Middlewares;

/// <summary>
///     Intercept request to add jwt token and link user to it
/// </summary>
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;


    /// <summary>
    ///     Constructor of the middleware
    /// </summary>
    /// <param name="next"> </param>
    public JwtMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Invoke method of the middleware
    /// </summary>
    /// <param name="context"> </param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            var authorizationHeader = context.Request.Headers["Authorization"];
            var listValues = authorizationHeader.ToList();
            var token = listValues.FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                await AttachUserToContext(context, token);
            }

            await _next(context);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Error in JwtMiddleware : {e}", EnumLogSeverity.Error);
            await _next(context);
        }
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var jwtToken = AuthorizationJwt.ReadJwtToken(token);

            var rawUserId = jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (rawUserId != null)
            {
                var userId = Guid.Parse(rawUserId);

                await using var scope = _serviceProvider.CreateAsyncScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();

                var userLinked = await userRepository.GetUserById(userId);
                // attach user to context on successful jwt validation
                context.Items["User"] = userLinked;
            }
        }
        catch (Exception e)
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
            Log.Logger.Information(
                                   $"User jwt attachment failed, raw token is {token} and ip is {context.Connection.RemoteIpAddress} => {context}" +
                                   e, EnumLogSeverity.Error);
        }
    }
}
