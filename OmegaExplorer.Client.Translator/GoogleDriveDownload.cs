namespace OmegaExplorer.Client.Translator
{
    public static class GoogleDriveDownload
    {
        const string DOWNLOAD_PATH = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQ2WoT0BPdGmz_osy_efC5ztWdlJBgct8ELqfL_VfnVIQxmRn-mBJdXzVN0QaCjXSuQ7iVpNyusr4z2/pub?output=csv";
        public static async Task UpdateTranslationFileAsync()
        {
            FileInfo translationFileCsv = TranslationGenerator.GetInputFile();


            Console.Write("Downloading update in " + translationFileCsv);

            Uri uri = new Uri(DOWNLOAD_PATH);
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(uri);
            try
            {
                if (translationFileCsv.Exists)
                {
                    translationFileCsv.Delete();
                }

                await using FileStream fs = new FileStream(translationFileCsv.FullName, FileMode.CreateNew);
                await response.Content.CopyToAsync(fs);

                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
