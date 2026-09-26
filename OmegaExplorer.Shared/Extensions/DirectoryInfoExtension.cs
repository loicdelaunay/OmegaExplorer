namespace OmegaExplorer.Server.Extensions;

using System.IO;

/// <summary>
///     Extend directory info methods
/// </summary>
public static class DirectoryInfoExtension
{
    /// <summary>
    ///     Get a specific file into a folder
    /// </summary>
    /// <param name="source"> </param>
    /// <param name="filename"> </param>
    /// <returns> </returns>
    public static FileInfo? GetFile(this DirectoryInfo source, string filename,
                                    GetBehaviour behaviour = GetBehaviour.Default)
    {
        try
        {
            string path = Path.Combine(path1: source.FullName, path2: filename);
            FileInfo file = new FileInfo(fileName: path);

            switch (behaviour)
            {
                case GetBehaviour.CreateIfNotExist when !file.Exists:
                    file.Create();
                    break;
                case GetBehaviour.ErrorIfNotExist when !file.Exists:
                    throw new Exception(message: "File at " + path + " not found");
                case GetBehaviour.NullIfNotExist when !file.Exists:
                    return null;
                case GetBehaviour.Default:
                    break;
            }

            return file;
        }
        catch (Exception e)
        {
            throw new Exception(message: "Not able to get directory " + e.Message);
        }
    }

    public static string GetFilePath(this DirectoryInfo source, string filename)
    {
        return Path.Combine(source.FullName, filename);
    }

    public static bool IsFileExist(this DirectoryInfo source, string filename)
    {
        return source.GetFiles().Any(x => x.Name == filename);
    }

    public static List<FileInfo> GetFilesRecursively(this DirectoryInfo source, string searchPattern = "*.*")
    {
        List<FileInfo> files = new List<FileInfo>();

        files.AddRange(collection: source.GetFiles(searchPattern: searchPattern));

        foreach (DirectoryInfo directory in source.GetDirectories())
        {
            files.AddRange(collection: directory.GetFilesRecursively(searchPattern: searchPattern));
        }

        return files;
    }

    public enum GetBehaviour
    {
        /// <summary>
        ///     Return a directory info even if not exist
        /// </summary>
        Default,

        /// <summary>
        ///     Return null if directory not exist
        /// </summary>
        NullIfNotExist,

        /// <summary>
        ///     Throw exception if folder not exist
        /// </summary>
        ErrorIfNotExist,

        /// <summary>
        ///     Create directory if not existing
        /// </summary>
        CreateIfNotExist
    }

    /// <summary> The CreateIfNotExist function creates a directory if it does not exist.</summary>
    /// <param name="this DirectoryInfo source">
    ///     /// the source.
    /// </param>
    /// <returns> The directoryinfo object that was passed in.</returns>
    public static void CreateIfNotExist(this DirectoryInfo source)
    {
        if (!source.Exists) source.Create();
    }

    /// <summary>
    ///     Get a specific folder into a folder
    /// </summary>
    /// <param name="source"> </param>
    /// <param name="folderName"> </param>
    /// <param name="behaviour"> </param>
    /// <returns> </returns>
    public static DirectoryInfo GetDirectory(this DirectoryInfo source, string folderName,
                                             GetBehaviour behaviour = GetBehaviour.NullIfNotExist)
    {
        try
        {
            string path = Path.Combine(path1: source.FullName, path2: folderName);
            DirectoryInfo directory = new DirectoryInfo(path: path);

            switch (behaviour)
            {
                case GetBehaviour.CreateIfNotExist when !directory.Exists:
                    directory.Create();
                    break;
                case GetBehaviour.ErrorIfNotExist when !directory.Exists:
                    throw new Exception(message: "Directory at " + path + " not found");
                case GetBehaviour.NullIfNotExist when !directory.Exists:
                    return null;
                case GetBehaviour.Default:
                    break;
            }

            return directory;
        }
        catch (Exception e)
        {
            throw new Exception(message: "Not able to get directory " + e.Message);
        }
    }

    public static void Copy(this DirectoryInfo source, DirectoryInfo target, bool overrideFiles = false)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(source.FullName, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(source.FullName, target.FullName));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(source.FullName, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(source.FullName, target.FullName), overrideFiles);
        }
    }

    public static bool IsEmpty(this DirectoryInfo source)
    {
        bool haveFile = source.GetFiles().Length > 0;
        bool haveFolder = source.GetDirectories().Length > 0;

        return !haveFile && !haveFolder;
    }
}