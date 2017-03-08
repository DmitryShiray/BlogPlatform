namespace BlogPlatform.Infrastructure
{
    public enum AuthenticationStatus
    {
        /// <summary>
        /// Indicates that user has been successfully authenticated in the system.
        /// </summary>
        Success,

        /// <summary>
        /// Indicates that user with the specified credentials was not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// Indicates that user with the specified login exists but the provided password does not
        /// match.
        /// </summary>
        InvalidPassword,

        /// <summary>
        /// Indicates that failure occured during authentication.
        /// </summary>
        Failure
    }
}
 