using Newtonsoft.Json;
using OmegaExplorer.Server.Utilities.Serializer;
using System.IO;

namespace OmegaExplorer.Server.Extensions;

/// <summary>
///     Expand file info methods
/// </summary>
public static class FileInfoExtension
{
    public static void CreateAndDispose(this FileInfo source, bool overwrite = false)
    {
        if (overwrite)
        {
            source.DeleteIfExist();
        }

        FileStream fs = source.Create();
        fs.Dispose();
    }

    /// <summary>
    ///     Read all text into this file
    /// </summary>
    /// <param name="source"> </param>
    /// <returns> </returns>
    public static string ReadAllText(this FileInfo source)
    {
        string res = File.ReadAllText(source.FullName);
        return res;
    }

    public static string NameWithoutExtension(this FileInfo source)
    {
        return Path.GetFileNameWithoutExtension(source.Name);
    }

    public static byte[] GetBytes(this FileInfo source)
    {
        FileStream fs = File.OpenRead(path: source.FullName);
        byte[] data = new byte[fs.Length];
        int br = fs.Read(buffer: data, offset: 0, count: data.Length);

        if (br != fs.Length)
        {
            throw new IOException(message: source.FullName);
        }

        fs.Dispose();
        return data;
    }

    /// <summary>
    ///     Write plan text into a file
    /// </summary>
    /// <param name="source"> </param>
    /// <param name="data"> </param>
    public static void WriteText(this FileInfo source, string data)
    {
        File.WriteAllText(source.FullName, data);
    }

    public static void AppendText(this FileInfo source, string data)
    {
        File.AppendAllText(source.FullName, data);
    }

    /// <summary>
    ///     Delete file if exist else ignore
    /// </summary>
    /// <param name="source"> </param>
    public static void DeleteIfExist(this FileInfo source)
    {
        if (source.Exists)
        {
            source.Delete();
        }
    }

    /// <summary>
    ///     Read a json and return object
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="source"> </param>
    /// <param name="writeDefaultIfNotExisting">if file not exist write default T</param>
    /// <returns> </returns>
    public static T ReadJson<T>(this FileInfo source, bool writeDefaultIfNotExisting = false,
        object defaultObjectToWrite = null) where T : new()
    {
        if (!source.Exists)
        {
            if (writeDefaultIfNotExisting)
            {
                source.CreateAndDispose();
                if (defaultObjectToWrite == null)
                {
                    source.WriteJson(new T());
                }
                else
                {
                    source.WriteJson(defaultObjectToWrite);
                }
            }
            else
            {
                return default;
            }
        }

        T result = JsonSerializerManager.Deserialize<T>(toDeserialize: source.ReadAllText());

        return result;
    }

    /// <summary>
    ///     Write an object an serialize it into the file
    /// </summary>
    /// <param name="source"> </param>
    /// <param name="data"> </param>
    public static void WriteJson(this FileInfo source, object data)
    {
        source.WriteText(JsonSerializerManager.Serialize(data));
    }

    public static void AppendJson(this FileInfo source, object data)
    {
        string dateRaw = JsonConvert.SerializeObject(data);
        File.AppendAllText(source.FullName, dateRaw);
    }

    public static void Rename(this FileInfo source, string newName)
    {
        // Get the file's directory
        string? directory = source.DirectoryName;

        // Create the new full path
        if (directory != null)
        {
            string newPath = Path.Combine(path1: directory, path2: newName);

            // Rename the file
            File.Move(sourceFileName: source.FullName, destFileName: newPath);
        }
        else
        {
            throw new Exception(message: "Not able to rename because it's not possible to detect parent");
        }
    }
}