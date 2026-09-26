using System.Diagnostics;

namespace OmegaExplore.Deployer;

public static class CommandManager
{
    public static void RunCommand(string command)
    {
        try
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", // Le shell Windows (command prompt)
                RedirectStandardInput = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                Arguments = "/c " + command
            };

            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);

            process.WaitForExit();
            process.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when executing command {command} : {ex.Message}");
        }
    }
}