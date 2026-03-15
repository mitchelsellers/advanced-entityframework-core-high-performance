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
            Console.WriteLine($"Name: {speaker.Name}, Title: {speaker.Title}, Company: {speaker.Company}, Sessions Given: {speaker.SessionsGiven}, Valid From: {speakerEntry.Property<DateTime>("ValidTo").CurrentValue}");
        }
    }

    public async Task ListSpeakersFullHistoryAsync()
    {
        var speakers = await context.Speakers
            .TemporalAll()
            .OrderBy(s => s.Name)
            .ThenBy(s => EF.Property<DateTime>(s, "ValidFrom"))
            .Select(s => new
            {
                s.Name,
                s.Title,
                s.Company,
                s.SessionsGiven,
                ValidFrom = EF.Property<DateTime>(s, "ValidFrom"),
                ValidTo = EF.Property<DateTime>(s, "ValidTo")
            })
            .ToListAsync();
        foreach (var speaker in speakers)
        {
            var speakerEntry = context.Entry(speaker);
            Console.WriteLine($"Name: {speaker.Name}, Title: {speaker.Title}, Company: {speaker.Company}, Sessions Given: {speaker.SessionsGiven}, Valid From: {speaker.ValidFrom}, Valid To: {speaker.ValidTo}");
        }
    }
}
