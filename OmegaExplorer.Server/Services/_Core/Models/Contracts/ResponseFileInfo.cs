namespace OmegaExplorer.Server.Services._Core.Models.Contracts;

public class ResponseFileInfo
{
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Extension { get; set; }
    public long Length { get; set; }
    public string? Data { get; set; }
}