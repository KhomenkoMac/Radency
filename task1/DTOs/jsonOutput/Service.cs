using System.Text.Json.Serialization;

namespace task1.DTOs.jsonOutput;

public class Service : Composite
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("payers")]
    public List<Payer> Payers
    {
        get => _components.Cast<Payer>().ToList();
        set
        {
            Add(value);
        }
    }
}

