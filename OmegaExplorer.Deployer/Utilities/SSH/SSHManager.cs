using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace OmegaExplore.Deployer.Utilities.SSH;

public static class SSHManager
{
    private static readonly string[] dontCopyFiles =
    {
        "config.json",
        "appsettings.json",
        "database.sqlite",
        "database.sqlite-shm",
        "database.sqlite-wal",
    };

    private static readonly string[] dontCopyFolder =
    {
        "volume",
        "logs"
    };

    public static void CleanDirectories(SftpClient sftpClient, string remoteDirectory)
    {
        if (!sftpClient.Exists(remoteDirectory))
        {
            Console.WriteLine($"Directory {remoteDirectory} does not exist.");
            return;
        }

        foreach (ISftpFile file in sftpClient.ListDirectory(remoteDirectory))
        {
            try
            {
                //Ignore folder 
                if (dontCopyFolder.Contains(file.Name.ToLower()))
                {
                    continue;
                }

                //Ignore files
                if (dontCopyFiles.Contains(file.Name.ToLower()))
                {
                    continue;
                }

                if (file.Name == "." || file.Name == "..")
                {
                    continue;
                }

                if (file.IsDirectory)
                {
                    //Not used while no subfolder to check to ignore
                    CleanDirectories(sftpClient, file.FullName);
                }
                else
                {
                    Console.WriteLine($"Deleting file {file.FullName}");
                    sftpClient.DeleteFile(file.FullName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while deleting {file.FullName} : {e.Message}");
                throw;
            }
        }
    }

    public static void DeleteDatabase(SftpClient sftpClient, string remoteDirectory)
    {
        string[] target = new[]
        {
            "database.sqlite",
            "database.sqlite-shm",
            "database.sqlite-wal"
        };

        foreach (ISftpFile file in sftpClient.ListDirectory(remoteDirectory))
        {
            if (target.Contains(file.Name))
            {
                Console.WriteLine($"Deleting file {file.FullName}");
                sftpClient.DeleteFile(file.FullName);
            }
        }
    }

    public static void SyncDirectories(SftpClient sftpClient, DirectoryInfo localDirectory, string remoteDirectory)
    {
        if (!sftpClient.Exists(remoteDirectory))
        {
            Console.WriteLine($"Creating directory {remoteDirectory}...");
            sftpClient.CreateDirectory(remoteDirectory);
        }

        foreach (FileInfo localFile in localDirectory.GetFiles())
        {
            if (dontCopyFiles.Contains(localFile.Name.ToLower()))
            {
                Console.WriteLine($"Skipping file {localFile.Name}");
                continue;
            }

            string remoteFileName = remoteDirectory + localFile.Name;
            using (FileStream fileStream = new FileStream(localFile.FullName, FileMode.Open))
            {
                Console.WriteLine($"Uploading {localFile.FullName} to {remoteFileName}");
                sftpClient.UploadFile(fileStream, remoteFileName);
            }
        }

        foreach (DirectoryInfo localSubDir in localDirectory.GetDirectories())
        {
            if (dontCopyFolder.Contains(localSubDir.Name.ToLower()))
            {
                Console.WriteLine($"Skipping folder {localSubDir.Name}");
                continue;
            }

            string remoteSubDir = remoteDirectory + localSubDir.Name + "/";
            SyncDirectories(sftpClient, localSubDir, remoteSubDir);
        }
    }
}