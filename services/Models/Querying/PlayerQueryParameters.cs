namespace services.Models.Querying
{
    /// <summary>
    /// Class that represents query parameters for bulk querying of players. 
    /// </summary>
    public class PlayerQueryParameters 
    {
        /// <summary>
        /// Gets or sets the sport of players to get.
        /// </summary>
        public string? Sport { get; set; }

        /// <summary>
        /// Gets or sets the first letter of the last name of players to get.
        /// </summary>
        public string? LastNameStartsWith { get; set; }

        /// <summary>
        /// Gets or sets the specific age of players to get.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets an inclusive range of ages of players to get. Must be in the form "From:To".
        /// </summary>
        public string? AgeRange { get; set; }

        /// <summary>
        /// Gets or sets the specific position of players to get.
        /// </summary>
        public string? Position { get; set; }
    }
}

