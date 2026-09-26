using OmegaExplore.Deployer;
using OmegaExplore.Deployer.Utilities.Configuration;

Console.WriteLine("Welcome to OmegaExplorer.Deployer system !");
ConfigurationManager.Initialize();
bool exit = false;

while (!exit)
{
    Console.WriteLine("1.) Deployer | 2.) Restart | 3.) Delete database | x.) Exit");
    string? res = Console.ReadLine();

    switch (res)
    {
        case "1":
            DeployPipelineManager.Deploy();
            break;
        case "2":
            DeployPipelineManager.Restart();
            break;
        case "3":
            DeployPipelineManager.DeleteDatabase();
            break;
        case "x":
            exit = true;
            break;
    }
}