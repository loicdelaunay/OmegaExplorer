namespace OmegaExplorer.Server.Services.Users.Models.Contracts;

public class RequestUpdateUser
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public bool? Disabled { get; set; }
}