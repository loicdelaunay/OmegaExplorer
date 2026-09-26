using Microsoft.IdentityModel.Tokens;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OmegaExplorer.Server.Services.Jwt;

/// <summary>
/// Utility and helper for all things around the JSON Web Token
/// </summary>
public static class AuthorizationJwt
{
    /// <summary>
    /// Create a Web Token for an user and link it to the current application
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string GenerateJwtToken(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        var key = Encoding.ASCII.GetBytes(ConfigurationManager.Configuration.Jwt.Key);

        // Check key size
        if (key.Length < 64)
        {
            Log.Logger.Error($"JWT key must be greater than 512 bytes (64 octets), currently is {key.Length}.", EnumLogSeverity.Error);
            throw new InvalidOperationException($"JWT key must be greater than 512 bytes (64 octets), currently is {key.Length}.");
        }

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Issuer = ConfigurationManager.Configuration.Jwt.Issuer,
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("id", user.Id.ToString()),
                }),
            Expires = DateTime.Now.AddSeconds(ConfigurationManager.Configuration.Jwt.DurationInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                        SecurityAlgorithms.HmacSha512Signature)
        };

        try
        {
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to generate json web token", EnumLogSeverity.Error);
            throw;
        }
    }

    /// <summary>
    ///     Read a json token
    /// </summary>
    /// <param name="token"> </param>
    /// <returns> </returns>
    public static JwtSecurityToken ReadJwtToken(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        var key = Encoding.ASCII.GetBytes(ConfigurationManager.Configuration.Jwt.Key);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        return jwtToken;
    }

    /// <summary>
    ///     Revoke a jwt token immediate
    /// </summary>
    public static void RevokeJsonWebToken()
    {
        Log.Logger.Information("Not implemented revoke json web token", EnumLogSeverity.Error);
    }
}