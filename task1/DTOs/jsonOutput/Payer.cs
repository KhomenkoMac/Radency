using System.Text.Json.Serialization;

namespace task1.DTOs.jsonOutput;

public class Payer : IComponent
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("payment")]
    public decimal Payment { get; set; }

    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    [JsonPropertyName("account_number")]
    public long AccountNumber { get; set; }

    [JsonPropertyName("total")]
    public decimal Total => Payment;
}

