using task1.ActorModel;
using task1.DTOs.metalogOutput;
using task1.Helpers;

namespace task1.Workers;

public class CsvFilesHandlerWorker : BackgroundService
{
    public class CsvFileHandlerOptions
    {
        public const string InputNOutputPaths = "InputNOutputPaths";

        public string ObservedDirectoryPath { get; set; } = null!;
        public string OutputHandledDirectoryPath { get; set; } = null!;
    }

    private readonly JsonRecordPreparerActor _jsonRecordPreparer;
    private readonly ParserActor _parser;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CsvFilesHandlerWorker> _logger;

    public CsvFilesHandlerWorker(FileSystemWatcher watcher, JsonRecordPreparerActor jsonRecordPreparer, ParserActor parser, EventBus eventBus, IConfiguration configuration, ILogger<CsvFilesHandlerWorker> logger)
    {
        _jsonRecordPreparer = jsonRecordPreparer;
        _parser = parser;
        _configuration = configuration;
        _logger = logger;

        watcher.Created += OnCreated;

        eventBus.Subscribe(OnFileHandled);
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var observedDirPath = _configuration
            .GetSection(CsvFileHandlerOptions.InputNOutputPaths)
            .Get<CsvFileHandlerOptions>().ObservedDirectoryPath;

        var files = Directory.GetFiles(observedDirPath, "*.csv");

        foreach (var file in files)
        {
            await _parser.SendAsync(file);
        }
    }

    private async void OnCreated(object sender, FileSystemEventArgs e)
    {
        await _parser.SendAsync(e.FullPath);
    }

    private async Task OnFileHandled(ParsedFile arg)
    {
        if (arg.ParsedTransactions.All(x => x == null)) return;
        
        var paths = _configuration
            .GetSection(CsvFileHandlerOptions.InputNOutputPaths)
            .Get<CsvFileHandlerOptions>();

        await _jsonRecordPreparer.SendAsync(new ValueTuple<string, ParsedFile>(paths.OutputHandledDirectoryPath, arg));
    }

    

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //    await Task.Delay(1000, stoppingToken);
        //}
        return Task.CompletedTask;
    }
}

