using OmegaExplorer.Server.Extensions;

namespace OmegaExplore.Deployer;

public static class PathManager
{
    public static string Mode = "development";


    public static DirectoryInfo GetApiProjectFolder()
    {
        DirectoryInfo res = new DirectoryInfo("../../../../OmegaExplorer.Server");

        return res;
    }

    public static DirectoryInfo GetAppProjectFolder()
    {
        DirectoryInfo res = new DirectoryInfo("../../../../OmegaExplorer.Client");

        return res;
    }

    public static DirectoryInfo GetApiBuiltFolder()
    {
        DirectoryInfo res = new DirectoryInfo($"./built/{Mode}/api");
        res.CreateIfNotExist();
        return res;
    }

    public static DirectoryInfo GetAppBuiltFolder()
    {
        DirectoryInfo res = new DirectoryInfo($"./built/{Mode}/app");
        res.CreateIfNotExist();
        return res;
    }

    public static string GetApiDeployFolder()
    {
        string res = $"/app/{Mode}/api/";
        return res;
    }

    public static string GetAppDeployFolder()
    {
        string res = $"/app/{Mode}/app/";
        return res;
    }

    public static string GetApiDeployExePath()
    {
        string res = $"/app/{Mode}/api/OmegaExplorer.Server";
        return res;
    }

    public static string GetAppDeployExePath()
    {
        string res = $"/app/{Mode}/app/OmegaExplorer.Client";
        return res;
    }

    public static string GetApiServiceName()
    {
        string res = $"omegaexplorer.{Mode}.api";
        return res;
    }

    public static string GetAppServiceName()
    {
        string res = $"omegaexplorer.{Mode}.app";
        return res;
    }
}