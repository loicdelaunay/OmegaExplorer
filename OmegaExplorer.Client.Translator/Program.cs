using OmegaExplorer.Client.Translator;

Console.WriteLine("Running translation generator");

try
{
    await GoogleDriveDownload.UpdateTranslationFileAsync();
}
catch (Exception e)
{
    Console.WriteLine("not possible to update file online" + e);
}


TranslationGenerator.Generate();