using task1.ActorModel;
using task1.Helpers;
using task1.Services;
using task1.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton((serviceProvider) =>
        {
            const string observeCsvWithAnyName = "*.csv";

            var paths = serviceProvider
            .GetRequiredService<IConfiguration>()
            .GetSection(CsvFilesHandlerWorker
                       .CsvFileHandlerOptions
                       .InputNOutputPaths)
            .Get<CsvFilesHandlerWorker.CsvFileHandlerOptions>();


            return new FileSystemWatcher(
                paths.ObservedDirectoryPath,
                observeCsvWithAnyName)
            {
                EnableRaisingEvents = true
            };
        });

        services.AddSingleton<EventBus>();
        services.AddSingleton<CsvParser>();
        services.AddSingleton<ParserActor>();
        services.AddSingleton<JsonRecordPreparerActor>();
        services.AddHostedService<CsvFilesHandlerWorker>();
        services.AddHostedService<ResultsBackgroundRecorder>();
    })
    .Build();

host.Run();
