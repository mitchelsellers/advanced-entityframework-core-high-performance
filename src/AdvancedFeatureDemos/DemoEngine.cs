using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedFeatureDemos;

public interface IDemoEngine
{
    Task ClearSpeakersAsync();
    Task InsertStandardDataAsync();
    Task RandomlyUpdateSpeakerCountAsync();
    Task ListSpeakersAsync();
    Task ListSpeakersFullHistoryAsync();
}

public class DemoEngine(DemoDbContext context) : IDemoEngine
{
    public async Task ClearSpeakersAsync()
    {
        //Quickly delete the defaults
        var query = context.Speakers.Where(s => s.Name == "John Doe" || s.Name == "Jane Smith");
        await query.ExecuteDeleteAsync();
    }

    public async Task InsertStandardDataAsync()
    {
        context.Add(new Models.Speaker
        {
            Name = "John Doe",
            Title = "Software Engineer",
            Company = "Tech Company",
            Bio = "John is a software engineer with over 10 years of experience.",
            SessionsGiven = 5
        });
        context.Add(new Models.Speaker
        {
            Name = "Jane Smith",
            Title = "Senior Developer",
            Company = "Another Tech Company",
            Bio = "Jane is a senior developer with expertise in cloud computing.",
            SessionsGiven = 10
        });
        await context.SaveChangesAsync();
    }

    public async Task RandomlyUpdateSpeakerCountAsync()
    {
        var random = new Random();
        var speakers = await context.Speakers.ToListAsync();
        foreach (var speaker in speakers)
        {
            speaker.SessionsGiven += random.Next(1, 5);
        }
        await context.SaveChangesAsync();

        Console.WriteLine("Speakers Updated - Current Staus of Speakers:");
        await ListSpeakersAsync();
    }

    public async Task ListSpeakersAsync()
    {
        var speakers = await context.Speakers.ToListAsync();
        foreach (var speaker in speakers)
        {
            var speakerEntry = context.Entry(speaker);
            Console.WriteLine($"Name: {speaker.Name}, Title: {speaker.Title}, Company: {speaker.Company}, Sessions Given: {speaker.SessionsGiven}, PeriodStart: {speakerEntry.Property<DateTime>("PeriodStart").CurrentValue}");
        }
    }

    public async Task ListSpeakersFullHistoryAsync()
    {
        Console.WriteLine("Full Speaker HIstory; ORdered by Speaker and Start");

        var speakers = await context.Speakers
            .TemporalAll()
            .OrderBy(s => s.Name)
            .ThenBy(s => EF.Property<DateTime>(s, "PeriodStart"))
            .Select(s => new
            {
                s.Name,
                s.Title,
                s.Company,
                s.SessionsGiven,
                PeriodStart = EF.Property<DateTime>(s, "PeriodStart"),
                PeriodEnd = EF.Property<DateTime>(s, "PeriodEnd")
            })
            .ToListAsync();
        foreach (var speaker in speakers)
        {
            Console.WriteLine($"Name: {speaker.Name}, Title: {speaker.Title}, Company: {speaker.Company}, Sessions Given: {speaker.SessionsGiven}, PeriodStart: {speaker.PeriodStart}, PeriodEnd: {speaker.PeriodEnd}");
        }
    }
}
