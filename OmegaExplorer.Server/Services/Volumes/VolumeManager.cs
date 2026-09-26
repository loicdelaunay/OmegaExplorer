using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services.ProjectFiles;

namespace OmegaExplorer.Server.Services.Volumes;

public static class VolumeManager
{
    public static DirectoryInfo GetSpaceshipThumbnailDirectory()
    {
        var res = FileProjectManager.GetStaticDirectory("/Spaceship/Thumbnail/");

        res.CreateIfNotExist();

        return res;
    }

    public static FileInfo GetLogDatabaseFile()
    {
        var baseDirectory = AppContext.BaseDirectory;

        var res = Path.Combine(baseDirectory, "logs.db");

        FileInfo file = new(res);

        return file;
    }

    public static FileInfo GetMiniProfilerDatabaseFile()
    {
        var baseDirectory = AppContext.BaseDirectory;

        var res = Path.Combine(baseDirectory, "miniprofiler.db");

        FileInfo file = new(res);

        return file;
    }
}