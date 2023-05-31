namespace data.Entities;

/// <summary>
/// Class that represents an entity for storing average age records.
/// </summary>
public class AverageAge
{
    public string Position { get; set; } = string.Empty;

    public string Sport{ get; set; } = string.Empty;

    public decimal Age { get; set; }
}
