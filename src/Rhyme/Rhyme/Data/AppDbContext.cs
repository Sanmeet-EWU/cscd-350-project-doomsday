using Microsoft.EntityFrameworkCore;
using Rhyme.Models;

namespace Rhyme.Data;

public class AppDbContext : DbContext
{
    public DbSet<WordWithPhonemes> WordsWithPhonemes { get; set; }
    public DbSet<WordWithPlainSyllables> WordsWithPlainSyllables { get; set; }
    public DbSet<Rhyme.Models.Rhyme> Rhymes { get; set; }


    public string DbPath { get; }

    public AppDbContext() : base()
    {
        DbPath = Path.Combine("C:/git/rhymeo/Rhyme/Rhyme/rhymeo.db");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
