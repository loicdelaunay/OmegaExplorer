namespace OmegaExplorer.Client.Utilities.API.Models;

public class ResultInitialize
{
    public EnumInitializeResult Result { get; set; }

    public string ServerVersion { get; set; }

    public string ToolkitVersion { get; set; }

    public Exception Exception { get; set; }

    public enum EnumInitializeResult
    {
        Success,
        SuccessButBadVersion,
        Error,
    }
}