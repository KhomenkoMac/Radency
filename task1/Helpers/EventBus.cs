using task1.DTOs.metalogOutput;

namespace task1.Helpers;

public class EventBus
{
    public event Func<ParsedFile, Task> FileIsParsed = null!;
    public event Action<ParsedInfoStats> UpdatedParsedInfoStats = null!;

    public Task RaiseOnFileParsed(ParsedFile page) => FileIsParsed?.Invoke(page) ?? Task.CompletedTask;
    public void RaiseOnStatsUpdated(ParsedInfoStats parsedInfoStats) => UpdatedParsedInfoStats?.Invoke(parsedInfoStats);
    
    public void Subscribe(Func<ParsedFile, Task> handler)
    {
        FileIsParsed += handler;
    }

    public void Subscribe(Action<ParsedInfoStats> handler)
    {
        UpdatedParsedInfoStats += handler;
    }
}
