using System.Text.Json.Serialization;

namespace task1.DTOs.jsonOutput;

public class Data : Composite
{
    [JsonPropertyName("city")]
    public string City { get; set; } = null!;

    [JsonPropertyName("services")]
    public List<Service> Services
    {
        get => _components.Cast<Service>().ToList();
        set
        {
            Add(value);
        }
    }
}

