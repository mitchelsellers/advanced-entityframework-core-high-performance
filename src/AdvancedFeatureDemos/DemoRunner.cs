using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedFeatureDemos;

/// <summary>
///     Represents the runner entry point for the demo system, responsible for inputs & command selection
/// </summary>
public interface IDemoRunner
{
    Task RunDemoAsync();
}

public class DemoRunner(IDemoEngine engine) : IDemoRunner
{
    public async Task RunDemoAsync()
    {
        Console.WriteLine("Welcome to the Advanced Feature Demos");
        Console.WriteLine("Clearing And Inserting Temporary Data For Demos...");
        await engine.ClearSpeakersAsync();
        await engine.InsertStandardDataAsync();
        Console.WriteLine("Data Setup Complete");

        ListDemoCommands();
        var exitRequested = false;
        while (!exitRequested)
        {
            var input = CollectInput();
            if (input == null)
                continue;

            var inputMatched = Enum.TryParse<DemoCommandOptions>(input, out var commandOption);
            if (!inputMatched)
            {
                Console.WriteLine("Invalid command entered.  Enter 1 to list all commands, 99 to exit");
                continue;
            }

            switch (commandOption)
            {
                case DemoCommandOptions.ListCommands:
                    ListDemoCommands();
                    break;
                case DemoCommandOptions.RandomlyUpdateSpeakerSessionCounts:
                    await engine.RandomlyUpdateSpeakerCountAsync();
                    break;
                case DemoCommandOptions.ListCurrentSpeakerData:
                    await engine.ListSpeakersAsync();
                    break;
                case DemoCommandOptions.Exit:
                    exitRequested = true;
                    break;
            }
        }
    }


    private void ListDemoCommands()
    {
        Console.WriteLine("The following commands are available to demo");
        foreach (var item in Enum.GetValues<DemoCommandOptions>())
            Console.WriteLine($"{(int)item} - {item.GetDisplayNameOrStringValue()}");
    }

    private string? CollectInput()
    {
        Console.WriteLine("Please Enter Requested Command?");
        return Console.ReadLine();
    }
}