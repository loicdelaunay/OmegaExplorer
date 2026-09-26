using OmegaExplore.Deployer.Utilities.Configuration;
using OmegaExplore.Deployer.Utilities.SSH;
using Renci.SshNet;

namespace OmegaExplore.Deployer;

public static class DeployPipelineManager
{
    private static bool _cleanBeforeDeploy = true;

    public enum ApplyOn
    {
        Api,
        App,
        Both
    }

    public static void Deploy()
    {
        ApplyOn applyOn = ApplyOn.Both;

        Console.WriteLine("Deploy mode : ");
        Console.WriteLine("1.) Both | 2.) Api | 3.) App");
        string? res = Console.ReadLine();
        switch (res)
        {
            case "1":
                applyOn = ApplyOn.Both;
                break;
            case "2":
                applyOn = ApplyOn.Api;
                break;
            case "3":
                applyOn = ApplyOn.App;
                break;
        }

        Console.WriteLine("Clean build and folder mode : ");
        Console.WriteLine("Default.) Yes | 2.) False");
        res = Console.ReadLine();
        switch (res)
        {
            case "2":
                _cleanBeforeDeploy = false;
                break;
        }


        Console.WriteLine("1.) Development | 2.) Integration | 3.) Production");
        res = Console.ReadLine();
        switch (res)
        {
            case "1":
                PathManager.Mode = "development";
                StartDeployment(applyOn);
                break;
            case "2":
                PathManager.Mode = "staging";
                StartDeployment(applyOn);
                break;
            case "3":
                PathManager.Mode = "production";
                StartDeployment(applyOn);
                break;
        }
    }

    public static void Restart()
    {
        Console.WriteLine("Restarting mode : ");
        Console.WriteLine("1.) Development | 2.) Integration | 3.) Production");
        string? res = Console.ReadLine();
        switch (res)
        {
            case "1":
                PathManager.Mode = "development";
                StartServiceOnServer(PathManager.GetApiServiceName());
                StopServiceOnServer(PathManager.GetApiServiceName());
                break;
            case "2":
                PathManager.Mode = "staging";
                StartServiceOnServer(PathManager.GetApiServiceName());
                StopServiceOnServer(PathManager.GetApiServiceName()); break;
            case "3":
                PathManager.Mode = "production";
                StartServiceOnServer(PathManager.GetApiServiceName());
                StopServiceOnServer(PathManager.GetApiServiceName()); break;
        }
    }

    public static void DeleteDatabase()
    {
        Console.WriteLine("Deleting database mode : ");
        Console.WriteLine("1.) No | 2.) Yes");
        string? res = Console.ReadLine();
        switch (res)
        {
            case "1":
                break;
            case "2":
                Console.WriteLine("1.) Development | 2.) Integration | 3.) Production");
                string? res2 = Console.ReadLine();
                switch (res2)
                {
                    case "1":
                        PathManager.Mode = "development";
                        break;
                    case "2":
                        PathManager.Mode = "staging";
                        break;
                    case "3":
                        PathManager.Mode = "production";
                        break;
                }

                break;
        }

        DeleteDatabaseOnServer(PathManager.GetApiDeployFolder());
    }

    private static void StartDeployment(ApplyOn applyOn)
    {
        if (_cleanBeforeDeploy)
        {
            PathManager.GetApiBuiltFolder().Delete(true);
            PathManager.GetAppBuiltFolder().Delete(true);
        }

        if (applyOn is ApplyOn.Both or ApplyOn.Api)
        {
            BuildProject(PathManager.GetApiProjectFolder(), PathManager.GetApiBuiltFolder(), false);
            StopServiceOnServer(PathManager.GetApiServiceName());
            if (_cleanBeforeDeploy)
            {
                CleanDirectoryOnServer(PathManager.GetApiDeployFolder());
            }
            CopyOnServer(PathManager.GetApiBuiltFolder(), PathManager.GetApiDeployFolder());
            AddExecAuthorizationOnServer(PathManager.GetApiDeployExePath());
            StartServiceOnServer(PathManager.GetApiServiceName());
        }

        if (applyOn is ApplyOn.Both or ApplyOn.App)
        {
            BuildProject(PathManager.GetAppProjectFolder(), PathManager.GetAppBuiltFolder(), true);
            StopServiceOnServer(PathManager.GetAppServiceName());

            if (_cleanBeforeDeploy)
            {
                CleanDirectoryOnServer(PathManager.GetAppDeployFolder());
            }

            CopyOnServer(PathManager.GetAppBuiltFolder(), PathManager.GetAppDeployFolder());
            AddExecAuthorizationOnServer(PathManager.GetAppDeployExePath());
            StartServiceOnServer(PathManager.GetAppServiceName());
        }
    }

    private static void BuildProject(DirectoryInfo projectDirectory, DirectoryInfo buildDirectory, bool publishingBlazorWASM)
    {
        try
        {
            if (_cleanBeforeDeploy)
            {
                Console.WriteLine($"Cleaning project {projectDirectory.Name}");
                CommandManager.RunCommand($"cd {projectDirectory.FullName} && dotnet clean --configuration Release");
            }

            Console.WriteLine($"Building project {projectDirectory.Name}");
            if (publishingBlazorWASM)
            {
                CommandManager.RunCommand($"cd {projectDirectory.FullName} && dotnet publish --configuration Release --output {buildDirectory.FullName} --runtime linux-x64 --self-contained true -p:PublishSingleFile=false");
            }
            else
            {
                CommandManager.RunCommand($"cd {projectDirectory.FullName} && dotnet publish --configuration Release --output {buildDirectory.FullName} --runtime linux-x64 --self-contained true -p:PublishSingleFile=true");
            }
            Console.WriteLine($"Project {projectDirectory.Name} built");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error when building API project {projectDirectory.Name} : " + e.Message);
        }
    }

    private static void CopyOnServer(DirectoryInfo pathToCopy, string remotePath)
    {
        SshConfiguration sshConfig = ConfigurationManager.Configuration.SshConfiguration;

        using (SftpClient sftpClient = new SftpClient(sshConfig.Host, sshConfig.Username, sshConfig.Password))
        {
            sftpClient.Connect();

            SSHManager.SyncDirectories(sftpClient, pathToCopy, remotePath);

            sftpClient.Disconnect();
        }
        Console.WriteLine("Uploading done !");
    }

    private static void CleanDirectoryOnServer(string remotePathToDelete)
    {
        SshConfiguration sshConfig = ConfigurationManager.Configuration.SshConfiguration;

        using (SftpClient sftpClient = new SftpClient(sshConfig.Host, sshConfig.Username, sshConfig.Password))
        {
            sftpClient.Connect();

            SSHManager.CleanDirectories(sftpClient, remotePathToDelete);

            sftpClient.Disconnect();
        }

        Console.WriteLine("Deleting done !");
    }

    private static void AddExecAuthorizationOnServer(string remoteTarget)
    {
        SshConfiguration sshConfig = ConfigurationManager.Configuration.SshConfiguration;

        using (SshClient sshClient = new SshClient(sshConfig.Host, sshConfig.Username, sshConfig.Password))
        {
            sshClient.Connect();

            string command = $"chmod 777 {remoteTarget}";
            Console.WriteLine($"command executed :  {command}");

            string result = sshClient.CreateCommand(command).Execute();

            Console.WriteLine(result);

            sshClient.Disconnect();
        }
    }

    private static void StopServiceOnServer(string serviceName)
    {
        SshConfiguration sshConfig = ConfigurationManager.Configuration.SshConfiguration;

        using (SshClient sshClient = new SshClient(sshConfig.Host, sshConfig.Username, sshConfig.Password))
        {
            sshClient.Connect();

            string command = $"sudo systemctl stop {serviceName}";
            Console.WriteLine("Executing : " + command);
            string result = sshClient.CreateCommand(command).Execute();

            Console.WriteLine(result);

            sshClient.Disconnect();
        }
    }

    private static void StartServiceOnServer(string serviceName)
    {
        SshConfiguration sshConfig = ConfigurationManager.Configuration.SshConfiguration;

        using (SshClient sshClient = new SshClient(sshConfig.Host, sshConfig.Username, sshConfig.Password))
        {
            sshClient.Connect();

            string command = $"sudo systemctl start {serviceName}";
            Console.WriteLine("Executing : " + command);
            string result = sshClient.CreateCommand(command).Execute();

            Console.WriteLine(result);

            sshClient.Disconnect();
        }
    }

    private static void DeleteDatabaseOnServer(string remotePathToDelete)
    {
        SshConfiguration sshConfig = ConfigurationManager.Configuration.SshConfiguration;

        using (SftpClient sftpClient = new SftpClient(sshConfig.Host, sshConfig.Username, sshConfig.Password))
        {
            sftpClient.Connect();

            SSHManager.DeleteDatabase(sftpClient, remotePathToDelete);

            sftpClient.Disconnect();
        }
    }
}