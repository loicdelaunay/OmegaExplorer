namespace OmegaExplore.Deployer.Utilities;

public static class ProjectFile
{
    public static FileInfo GetProjectFile(string path)
    {
        string pathResult = Path.Combine(Environment.CurrentDirectory, path);
        FileInfo res = new(pathResult);
        return res;
    }
}