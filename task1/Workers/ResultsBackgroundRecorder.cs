using System.Diagnostics;
using System.Text;
using task1.DTOs.metalogOutput;
using task1.Helpers;

namespace task1.Workers;

public class ResultsBackgroundRecorder : BackgroundService
{
    private readonly ILogger<CsvFilesHandlerWorker> _logger;
    private readonly IConfiguration _configuration;

    public ResultsBackgroundRecorder(ILogger<CsvFilesHandlerWorker> logger, EventBus eventBus, IConfiguration configuration)
    {
        _logger = logger;
        //_ = Timer; // initializing timer with the rest of time before recording parse stats
        eventBus.Subscribe(OnUpdatedStats);
        _configuration = configuration;
    }
    
    private TimeSpan MyTime => new(4, 53, 00);

    private TimeSpan TimeToNextRecordSession
    {
        get
        {
            TimeSpan remainingToNextSession;
            var tempMyTime = DateTime.Today.Add(MyTime);
            var tempNow = DateTime.Now;
            if (tempNow > tempMyTime)
            {
                remainingToNextSession = DateTime.Today.AddDays(1).Add(MyTime) - DateTime.Now;
            }
            else
            {
                remainingToNextSession = DateTime.Today.Add(MyTime) - DateTime.Now;
            }
            
            Debug.WriteLine($"remains to record: {remainingToNextSession}");
            
            return remainingToNextSession;
        }
    }

    private PeriodicTimer Timer => new PeriodicTimer(TimeToNextRecordSession);


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var paths = _configuration
            .GetSection(CsvFilesHandlerWorker.CsvFileHandlerOptions.InputNOutputPaths)
            .Get<CsvFilesHandlerWorker.CsvFileHandlerOptions>();
        var pathToMetaLogFile = Path.Combine(paths.OutputHandledDirectoryPath, $"{DateTime.Now:yyyy-dd-MM}", "meta.log");
        
        var stringBuilderService = new StringBuilder(typeof(ParsedInfoStats).GetProperties().Length);
        
        while (!stoppingToken.IsCancellationRequested && await Timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Making recording.... Stand by");
            
            await using var writer = File.CreateText(pathToMetaLogFile);

            stringBuilderService.AppendLine($"parsed_files: {_parsedInfoStats.Count}");
            stringBuilderService.AppendLine($"parsed_lines: {_parsedInfoStats.Sum(x=> x.ParsedLinesAmount)}");
            stringBuilderService.AppendLine($"found_errors: {_parsedInfoStats.Sum(x=> x.ParseErrorsAmount)}");
            stringBuilderService.AppendLine($"invalid_files: [{string.Join(", ", _parsedInfoStats.Where(x=> x.InvalidFilesAsText.Any()).SelectMany(x=> x.InvalidFilesAsText).Distinct())}]");

            await writer.WriteAsync(stringBuilderService, stoppingToken);

            stringBuilderService.Clear();
        }
    }

    private readonly List<ParsedInfoStats> _parsedInfoStats = new();

    private void OnUpdatedStats(ParsedInfoStats arg)
    {
        _parsedInfoStats.Add(arg);
    }
}
