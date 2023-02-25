using System.Diagnostics;
using System.Text;
using System.Text.Json;
using task1.Actors;
using task1.DTOs;
using task1.DTOs.jsonOutput;
using task1.DTOs.metalogOutput;
using task1.Helpers;

namespace task1.ActorModel;

public class JsonRecordPreparerActor: AbstractActor<(string, ParsedFile)>
{
    public JsonRecordPreparerActor(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public override int ThreadCount => 10;

    private readonly EventBus _eventBus;
    
    public override async Task HandleMessage((string, ParsedFile) message)
    {
        var (outputPath, arg) = message;
        
        var folderName = $"{DateTime.Now:yyyy-dd-MM}";
        var recordsFolder = Path.Combine(outputPath, folderName);

        if (!Directory.Exists(folderName))
        {
            Directory.CreateDirectory(recordsFolder);
        }
        
        var parsedFilesAmount = Directory.GetFiles(recordsFolder).Length; 
        var parsedLinesAmount = 0;
        var parseErrorsAmount = 0;
        var invalidFilesAsText = new List<string>();
        
        parsedLinesAmount += arg.ParsedLinesAmount;
        if (arg.FoundErrorsAmount > 0)
        {
            parseErrorsAmount += arg.FoundErrorsAmount;
            invalidFilesAsText.Add(arg.Filepath);
        }

        var stats = new ParsedInfoStats(
            parsedLinesAmount,
            parseErrorsAmount,
            invalidFilesAsText);

        var outputFilename = $"output{++parsedFilesAmount}.json";
        var outputFilePath = Path.Combine(recordsFolder, outputFilename);

        await using var writer = File.CreateText(outputFilePath);
        var strBuilderService = new StringBuilder(parsedLinesAmount);

        var onlyParsedItems = arg
            .ParsedTransactions
            .Where(x => x != null);
        
        var json = CreateJsonFileOutput(onlyParsedItems!);
        
        strBuilderService.AppendLine(json);

        await writer.WriteAsync(strBuilderService);
       
        _eventBus.RaiseOnStatsUpdated(stats);

    }

    public override async Task HandleError((string, ParsedFile) message, Exception ex)
    {
        Debug.WriteLine($"Error handling {message} with error {ex.Message}");

        await Task.Delay(1000);

        await SendAsync(message);
    }
    
    private static string CreateJsonFileOutput(IEnumerable<PaymentTransaction> transactions)
    {
        var paymentTransactions = transactions.ToArray();

        var groupCitiesAndServices = (
            from t in paymentTransactions
            let city = t.Address.Split(',')[0]!
            group t by city into g
            select new Data
            {
                City= g.Key,
                Services = (from s in g
                    let service = s.Service
                    group s by service into g1
                    select new Service
                    {
                        Name = g1.Key,
                        Payers = g1.Select(x=> new Payer
                        {
                            Name = string.Join(' ', x.Firstname, x.Lastname),
                            Payment = x.Payment,
                            Date = x.Date,
                            AccountNumber = x.AccountNumber
                        }).ToList()
                    }).ToList()
            }).First();

        var options = new JsonSerializerOptions
        {
            Converters = { new DateOnlyJsonConverter() }
        };
        var json = JsonSerializer.Serialize(groupCitiesAndServices, options);
        return json;
    }

}