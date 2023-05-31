namespace data.Entities;

/// <summary>
/// Class that represents an entity for a player.
/// </summary>
public class Player
{
    /// <summary>
    /// Gets or sets the unique identifier for the player.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the first name for this player.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name for this player.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the position for this player.
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sport for this player.
    /// </summary>
    public string Sport{ get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name brief for this player.
    /// </summary>
    public string NameBrief { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the age for this player.
    /// </summary>
    public int? Age { get; set; }
}
