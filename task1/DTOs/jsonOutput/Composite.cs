using System.Text.Json.Serialization;

namespace task1.DTOs.jsonOutput;

public abstract class Composite : IComponent
{
    protected readonly List<IComponent> _components = new();

    public void Add(IComponent component)
    {
        _components.Add(component);
    }

    public void Add(IEnumerable<IComponent> components)
    {
        _components.AddRange(components);
    }

    [JsonPropertyName("total")]
    public virtual decimal Total => _components.Select(x => x.Total).Sum();
}

