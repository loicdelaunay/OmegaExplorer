using System.ComponentModel.DataAnnotations;

namespace OmegaExplorer.Server.Services.Users.Models.Classes;

public class UserPreferences
{
    [MaxLength(10)]
    public string Language { get; set; } = "en";
}