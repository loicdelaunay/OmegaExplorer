using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.ProjectFiles.Models.Enums;

namespace OmegaExplorer.Server.Services.ProjectFiles;

public static class FileProjectManager
{
    /// <summary>
    ///     return a file with a relative path
    ///     Ex : Public/Assets/Image.png
    /// </summary>
    /// <param name="path"></param>
    /// <param name="behavior">Behavior of the get project file</param>
    /// <returns></returns>
    public static FileInfo GetProjectFile(string path, EnumGetProjectFileBehavior behavior = EnumGetProjectFileBehavior.ThrowErrorIfNotFound)
    {
        var pathResult = Path.Combine(Environment.CurrentDirectory, path);
        FileInfo res = new(pathResult);

        Log.Logger.Information($"Get project file at : {pathResult}", EnumLogSeverity.Information);

        switch (behavior)
        {
            case EnumGetProjectFileBehavior.ThrowErrorIfNotFound:
                if (!res.Exists)
                {
                    throw new FileNotFoundException($"File not found at {pathResult}");
                }
                break;
            case EnumGetProjectFileBehavior.IgnoreIfNotFound:
                break;
            case EnumGetProjectFileBehavior.CreateIfNotFound:
                if (!res.Exists)
                {
                    res.Create();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(behavior), behavior, null);
        }
        return res;
    }

    /// <summary>
    ///     return a project folder with a relative path
    ///     Ex : Public/Assets/
    /// </summary>
    /// <param name="path"></param>
    /// <param name="createIfNotExist"></param>
    /// <returns></returns>
    public static DirectoryInfo GetProjectDirectory(string path, bool createIfNotExist = false)
    {
        var pathResult = Path.Combine(Environment.CurrentDirectory, path);
        DirectoryInfo res = new(pathResult);

        Log.Logger.Information($"Get project directory at : {pathResult} | exist ? {res.Exists} => create if not exist ? {createIfNotExist}",
                       EnumLogSeverity.Information);

        if (!res.Exists && createIfNotExist)
        {
            res.Create();
        }

        return res;
    }

    public static DirectoryInfo GetVolumeDirectory(string path, bool createIfNotExist = true)
    {
        return GetProjectDirectory($"Volume/{path}", createIfNotExist);
    }

    public static DirectoryInfo GetStaticDirectory(string path, bool createIfNotExist = true)
    {
        return GetProjectDirectory($"Static/{path}", createIfNotExist);
    }
}