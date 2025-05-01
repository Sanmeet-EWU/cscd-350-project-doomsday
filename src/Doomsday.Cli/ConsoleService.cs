using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using Spectre.Console;

namespace Doomsday.Cli;

public class ConsoleService
{
    public static CanvasImage GetImage(Image image, int size)
    {
        string imgPath = Path.Combine("..", "..", "..", $"{image.ToString()}.png");
        CanvasImage imageRenderer = new(imgPath);
        imageRenderer.MaxWidth = size;

        return imageRenderer;
    }

    private static void ShowIntroduction()
    {
        Layout layout = new Layout("Root").SplitColumns(
            new Layout("Left").SplitRows(
                new Layout("Top"),
                new Layout("Middle", new Markup(string.Empty)),
                new Layout("Bottom")
            ),
            new Layout("Right")
        );

        layout["Right"]
            .Update(Align.Center(GetImage(Image.Madvillainy, 28), VerticalAlignment.Middle));
        layout["Left"]
            ["Top"]
            .Update(
                Align.Center(
                    new Markup(
                        "Welcome to [red]Operation: Doomsday[/]!\nMade by: [bold green]Joshua Lester[/]"
                    ),
                    VerticalAlignment.Bottom
                )
            );
        layout["Left"]
            ["Bottom"]
            .Update(
                Align.Center(
                    new Markup("Press [green]Enter[/] to continue..."),
                    VerticalAlignment.Top
                )
            );

        AnsiConsole.Write(layout);
        Console.ReadLine();
    }

    private static Choice ShowSelectionScreen()
    {
        int numberOfTabs = Console.WindowWidth / 2 / 4 - 10;
        string tabs = string.Empty;
        if (numberOfTabs > 0)
        {
            tabs = new string('\t', numberOfTabs);
        }
        SelectionPrompt<string> prompt = new SelectionPrompt<string>()
            .Title($"\n\n\n\n{tabs}[grey]What would you like to do?[/]")
            .PageSize(100)
            .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
            .AddChoices(
                [
                    $"{tabs}[bold white]Color my rhyme[/]\n",
                    //$"{tabs}[bold white]Search for a rhyme[/]\n", // keep this out until implemented
                    $"{tabs}[bold white]Quit[/]\n",
                ]
            );

        int heightModified = Console.WindowHeight / 2 + 2;
        var img = Align.Center(GetImage(Image.OperationDoomsday, heightModified));
        AnsiConsole.Write("\n\n");
        AnsiConsole.Write(img);

        char choice = AnsiConsole.Prompt(prompt).ToLower()[tabs.Length + 12];
        Console.WriteLine(choice);
        return choice switch
        {
            'c' => Choice.ColorMyRhyme,
            //'s' => Choice.SearchForARhyme, // keep this out until implemented
            'q' => Choice.Quit,
            _ => throw new ArgumentException(
                "Switch statement fell through all statements. Something is wrong."
            ),
        };
    }

    public static void Run()
    {
        // Prepare console for application
        AnsiConsole.Clear();
        AnsiConsole.Cursor.Hide();

        ShowIntroduction();

        Choice choice;
        do
        {
            AnsiConsole.Clear();
            choice = ShowSelectionScreen();
            switch (choice)
            {
                case Choice.ColorMyRhyme:
                    AnsiConsole.Clear();
                    AnsiConsole.WriteLine("Color my rhyme");
                    Console.ReadLine();
                    break;
                //case Choice.SearchForARhyme: // keep this out until implemented
                //    AnsiConsole.Clear();
                //    AnsiConsole.WriteLine("Search for a rhyme");
                //    break;
                case Choice.Quit:
                    AnsiConsole.Clear();
                    break;
            }
        } while (choice != Choice.Quit);

        // Cleanup for exit
        AnsiConsole.Clear();
        AnsiConsole.Cursor.Show();
    }
}

public enum Image
{
    Madvillainy,
    OperationDoomsday,
}

public enum Choice
{
    ColorMyRhyme,

    //SearchForARhyme, // keep this out until implemented
    Quit,
}
