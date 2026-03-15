using AdvancedFeatureDemos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedFeatureDemos;

public class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Find our custom stuff if needed
        builder.ApplyConfigurationsFromAssembly(typeof(DemoDbContext).Assembly);
    }

    public DbSet<Speaker> Speakers { get; set; }
}
