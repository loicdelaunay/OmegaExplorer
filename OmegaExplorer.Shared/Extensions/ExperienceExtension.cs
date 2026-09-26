namespace OmegaExplorer.Shared.Extensions
{
    public static class ExperienceExtension
    {
        private const double COMPLEXITY_FACTOR = 1.3;
        private const double BASE_THRESHOLD = 100;

        /// <summary>
        /// Compute the current level from the experience
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public static int GetLevel(this int experience)
        {
            int level = (int)(Math.Log(1 - (experience * (1 - COMPLEXITY_FACTOR) / BASE_THRESHOLD)) / Math.Log(COMPLEXITY_FACTOR));

            return Math.Max(level, 0);
        }

        /// <summary>
        /// Compute the experience required to reach the current level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static int GetExperienceForLevel(int level)
        {
            return (int)(BASE_THRESHOLD * (1 - Math.Pow(COMPLEXITY_FACTOR, level)) / (1 - COMPLEXITY_FACTOR));
        }

        /// <summary>
        /// Compute the current experience from the experience depending on the level reached
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public static int GetExperienceCurrentLevel(this int experience)
        {
            int currentLevel = experience.GetLevel();
            int experienceForCurrentLevel = GetExperienceForLevel(currentLevel);
            return experience - experienceForCurrentLevel;
        }

        /// <summary>
        /// Compute the total count of experience needed to reach the next level
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public static int GetExperienceToNextLevel(this int experience)
        {
            int currentLevel = experience.GetLevel();
            int experienceForNextLevel = GetExperienceForLevel(currentLevel + 1);
            return experienceForNextLevel - experience;
        }

        /// <summary>
        /// Get the total number of experience needed to reach the next level from  0 to X
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public static int GetExperienceTotalToNextLevel(this int experience)
        {
            int res = GetExperienceCurrentLevel(experience) + GetExperienceToNextLevel(experience);
            return res;
        }
    }
}