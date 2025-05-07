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

        List<Word> result = rhymeUtils.Run();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true    // pretty-print
        };
        string json = JsonSerializer.Serialize(result, options);

        Console.WriteLine(json);
    }
}
