using System.Security.Cryptography;
using System.Text;

namespace OmegaExplorer.Server.Services.Securities;

public class SecurityService
{
    /// <summary>
    ///     Hash and secure a string
    ///     <remarks> ex : for user password </remarks>
    ///     <remarks> To check if string is correct please use SecurityManager.VerifyString() </remarks>
    /// </summary>
    /// <returns> </returns>
    public string ProtectString(string stringToProtect)
    {
        if (string.IsNullOrEmpty(stringToProtect))
        {
            throw new Exception("Try to protect a null string");
        }

        var res = BCrypt.Net.BCrypt.HashPassword(inputKey: stringToProtect);

        return res;
    }

    /// <summary>
    ///     Check if the string is equal to a hash
    /// </summary>
    /// <param name="stringToVerify"> </param>
    /// <param name="hash"> </param>
    /// <returns> </returns>
    public bool VerifyString(string stringToVerify, string hash)
    {
        if (string.IsNullOrEmpty(value: stringToVerify) || string.IsNullOrEmpty(value: hash))
        {
            throw new Exception(message: "Not able to check password, one value is null");
        }

        var res = BCrypt.Net.BCrypt.Verify(text: stringToVerify, hash: hash);

        return res;
    }

    /// <summary>
    /// generates a random password.    /// </summary>
    /// <param name="length">La longueur du mot de passe à générer. La valeur par défaut est 12.</param>
    /// <returns>Un mot de passe aléatoire composé de lettres majuscules, minuscules, chiffres et caractères spéciaux.</returns>
    public string GenerateRandomPassword(int length = 12)
    {
        // Define the set of characters to be used in the password
        const string VALID_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        StringBuilder result = new(length);

        // Use a generator of cryptographic random numbers
        using (var rng = RandomNumberGenerator.Create())
        {
            var buffer = new byte[sizeof(uint)];

            while (result.Length < length)
            {
                rng.GetBytes(buffer);
                var num = BitConverter.ToUInt32(buffer, 0);
                // Select a random character from Validchars and add it to the result
                result.Append(VALID_CHARS[(int)(num % VALID_CHARS.Length)]);
            }
        }

        return result.ToString();
    }
}