using Rhyme.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rhyme.Utils;

namespace Rhyme;

public class Program
{
    public static async Task Main(string[] args)
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();
        await Seeder.Seed(db);

        RhymeUtils rhymeUtils = new(db);

        string lyricsPath = "sample-text.txt";

        if (args.Length > 0)
        {
            lyricsPath = args[0];
        }

        List<Word> result = rhymeUtils.Run(lyricsPath);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(result, options);

        Console.WriteLine(json);
    }
}
