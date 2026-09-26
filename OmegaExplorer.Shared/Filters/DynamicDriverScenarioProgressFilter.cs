namespace OmegaExplorer.Shared.Filters
{
    public class DynamicDriverScenarioProgressFilter : DynamicDriverFilter
    {
        private const string FILTER_BY_FINISHED = "finished";

        public DynamicDriverScenarioProgressFilter FilterByFinished(bool finished)
        {
            Data.Add(FILTER_BY_FINISHED, finished.ToString());

            return this;
        }

        public bool? GetFilterByFinished()
        {
            if (Data.TryGetValue(FILTER_BY_FINISHED, out string? raw))
            {
                if (bool.TryParse(raw, out bool finished))
                {
                    return finished;
                }
            }

            return null;
        }
    }
}
