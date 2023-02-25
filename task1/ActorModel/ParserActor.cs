using System.Diagnostics;
using System;
using task1.Actors;
using task1.Helpers;
using task1.Services;

namespace task1.ActorModel;

public class ParserActor : AbstractActor<string>
{
    private readonly CsvParser _csvParser;
    private readonly EventBus _eventBus;

    public ParserActor(CsvParser csvParser, EventBus eventBus)
    {
        _csvParser = csvParser;
        _eventBus = eventBus;
    }

    public override int ThreadCount => 10;

    public override async Task HandleError(string message, Exception ex)
    {
        Debug.WriteLine($"Error handling {message} with error {ex.Message}");

        await Task.Delay(1000);

        await SendAsync(message);
    }

    public override async Task HandleMessage(string message)
    {
        var result = await _csvParser.Parse(message);
        await _eventBus.RaiseOnFileParsed(result);
    }
}
