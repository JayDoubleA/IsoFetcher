using IsoFetcher.Enums;
using IsoFetcher.Models;
using IsoFetcher.Services;
using IsoFetcher.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{   
    private readonly IWorldBankIsoProcessingService _processingService;

    public Program (
        IWorldBankIsoProcessingService processingService)
    {
        _processingService = processingService;
    }

    private static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("******************************************");
        Console.WriteLine("World Bank API Country Details Fetcher");
        Console.WriteLine("By Andrew Arnott");
        Console.WriteLine("******************************************");
        Console.WriteLine();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Gray;

        IServiceProvider serviceProvider = RegisterServices();

        Program program = serviceProvider.GetService<Program>(); // if this is null, a hideous exception is appropriate

        await program.IsoFetch(); // if this is null, a hideous exception is appropriate

        DisposeServices(serviceProvider);
    }

    private async Task IsoFetch()
    {
        var inUse = true;

        while (inUse)
        {            
            Console.WriteLine("Please enter a two or three letter ISO code to look up, or \"x\" to exit");

            Console.ForegroundColor = ConsoleColor.White;

            var isoInput = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Gray;

            if (isoInput == "x")
            {
                inUse = false;
            }
            else
            {
                var responseModel = await _processingService.ProcessIso(isoInput);

                DisplayOutcome(responseModel);
            }
        }
    }

    private void DisplayOutcome(WorldBankIsoResponseModel responseModel)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;

        switch (responseModel.ProcessingStatus)
        {
            case Status.Error:
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Sorry, an unexpected error has occurred.");
                Console.BackgroundColor = ConsoleColor.Black;
                break;
            case Status.FailedValidation:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sorry, that was not a valid ISO code. Valid ISO codes are two or three letters.");
                break;
            case Status.InvalidValueResponse:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The ISO code provided does not exist.");
                break;
            case Status.SomethingOdd:
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Something very odd has happened here.");
                Console.BackgroundColor = ConsoleColor.Black;
                break;
            case Status.SuccessResponse:
                var model = responseModel.Data.First(); // this won't be null on a success
                Console.WriteLine();
                Console.WriteLine($"Country Name: {model.Name}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"Region: {model.Region?.Value}");
                Console.WriteLine($"Capital City: {model.CapitalCity}");
                Console.WriteLine($"Longitude: {model.Longitude}");
                Console.WriteLine($"Latitude: {model.Latitude}");
                break;
            case Status.NotStarted:
                break;
            default:
                throw new IndexOutOfRangeException("Status not recognised");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();
    }    

    //Support
    private static IServiceProvider RegisterServices()
    {
        var services = new ServiceCollection();
      
        //services
        services.AddScoped<IWorldBankIsoRequestClientService, WorldBankIsoRequestClientService>();
        services.AddScoped<IWorldBankIsoResponseService, WorldBankIsoResponseService>();
        services.AddScoped<IWorldBankIsoProcessingService, WorldBankIsoProcessingService>();
        services.AddScoped<ILoggingService, DummyLoggingService>();
        //main
        services.AddScoped<Program>(); //<-- NOTE THIS

        return services.BuildServiceProvider();
    }

    private static void DisposeServices(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            return;
        }
        if (serviceProvider is IDisposable sp)
        {
            sp.Dispose();
        }
    }
}