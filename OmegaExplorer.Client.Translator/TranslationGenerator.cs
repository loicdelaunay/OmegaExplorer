using Newtonsoft.Json;

namespace OmegaExplorer.Client.Translator
{
    public static class TranslationGenerator
    {
        public static string GetParentFolder()
        {
            string workingDirectory = Environment.CurrentDirectory;
            DirectoryInfo? parent = Directory.GetParent(workingDirectory);

            if (parent == null)
            {
                throw new ArgumentNullException($"no parent for {workingDirectory}");
            }

            DirectoryInfo? parent1 = parent.Parent;

            if (parent1 == null)
            {
                throw new ArgumentNullException($"no parent for {parent}");
            }
            DirectoryInfo? parent2 = parent1.Parent;

            if (parent2 == null)
            {
                throw new ArgumentNullException($"no parent for {parent1}");
            }
            DirectoryInfo? parent3 = parent2.Parent;

            if (parent3 == null)
            {
                throw new ArgumentNullException($"no parent for {parent2}");
            }
            string projectDirectory = parent3.FullName;

            return projectDirectory;
        }

        public static FileInfo GetInputFile()
        {
            FileInfo inputTranslation =
                new(GetParentFolder() + @"\OmegaExplorer.Client.Translator\Translation\Translation.csv");
            return inputTranslation;
        }

        public static void Generate()
        {
            DirectoryInfo outputFolder = new(GetParentFolder() + @"\OmegaExplorer.Client\I18ntext");

            if (!outputFolder.Exists)
            {
                Console.WriteLine("ERROR, output folder not existing : " + outputFolder.FullName);
            }

            FileInfo inputTranslation = GetInputFile();

            if (!inputTranslation.Exists)
            {
                Console.WriteLine("ERROR, translation file not existing at : " + inputTranslation);
            }

            List<CsvModel> res = CsvManager.Deserialize(inputTranslation);

            JSONModel en = new JSONModel();
            JSONModel fr = new JSONModel();
            JSONModel es = new JSONModel();
            JSONModel de = new JSONModel();

            foreach (CsvModel translation in res)
            {
                en.KeyValuePairs.Add(translation.Key, translation.English);
                fr.KeyValuePairs.Add(translation.Key, translation.French);
                es.KeyValuePairs.Add(translation.Key, translation.Spanish);
                de.KeyValuePairs.Add(translation.Key, translation.Deutsch);
            }

            File.WriteAllText(outputFolder + "/Translation.en.json", InterpretObject(en));
            File.WriteAllText(outputFolder + "/Translation.fr.json", InterpretObject(fr));
            File.WriteAllText(outputFolder + "/Translation.es.json", InterpretObject(es));
            File.WriteAllText(outputFolder + "/Translation.de.json", InterpretObject(de));
        }

        public static string InterpretObject(JSONModel modelToSerialize)
        {
            string res;

            res = JsonConvert.SerializeObject(modelToSerialize, Formatting.Indented);
            res = res.Remove(0, 22);
            res = res.Substring(0, res.Length - 1);

            return res;
        }
    }
}
