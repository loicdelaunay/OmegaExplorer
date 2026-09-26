using CsvHelper;
using System.Globalization;

namespace OmegaExplorer.Client.Translator
{
    public static class CsvManager
    {
        public static List<CsvModel> Deserialize(FileInfo input)
        {
            List<CsvModel> res;

            using (StreamReader reader = new StreamReader(input.FullName))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                IEnumerable<CsvModel> translations = csv.GetRecords<CsvModel>();
                res = translations.ToList();

                //Remove sub header
                res.Remove(res.First());
            }

            return res;
        }
    }
}
