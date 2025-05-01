using Spectre.Console;

namespace Doomsday.Cli;

public class Program
{
    public static void Main(string[] args)
    {
        AnsiConsole.AlternateScreen(() =>
        {
            ConsoleService.Run();
        });
    }
}
