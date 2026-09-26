namespace OmegaExplorer.Shared.Enums
{
    public enum EnumCustomStatusCode
    {
        JwtTokenExpired = 1100,

        /// <summary>
        /// When spaceship is not able to register a new action in battle because
        /// no more action points
        /// </summary>
        TooManyActionInBattle = 1200,

        LimitReached = 1201,
    }
}