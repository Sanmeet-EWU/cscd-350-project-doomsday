namespace Rhyme.FileIO;
using System.Text.RegularExpressions;

public class FileUtils
{
    private static readonly string[] s_newLineDelimiters = ["\r\n", "\n", "\r"];
    public static string GetLyrics(string path)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
        string adjustedPath = Path.Combine("..", "..", "..", path);
        if (!File.Exists(adjustedPath))
        {
            var exeFolder = AppContext.BaseDirectory;
            adjustedPath = Path.Combine(exeFolder, path);
            if (!File.Exists(adjustedPath))
            {
                throw new FileNotFoundException(adjustedPath);
            }
        }
        return File.ReadAllText(adjustedPath);
    }

    public static IEnumerable<string> GetWordsList(string lyrics)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(lyrics);

        string[] modifiedLyrics = Regex.Split(lyrics, "(\r\n|\n)");

        List<string> result = [];

        foreach (string s in modifiedLyrics)
        {
            if (s == "\r\n" || s == "\n")
            {
                result.Add("NEWLINECHAR");
            } else
            {
                var words = s
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(s => new string(s.Where(char.IsLetterOrDigit).ToArray()))
                    .Where(s => s.Length > 0);

                result.AddRange(words);
            }
        }
        //for (int i = 0; i < modifiedLyrics.Length; i++)
        //{
        //    string[] split = modifiedLyrics[i].Split(" ")
        //        .Select(s =>
        //        {
        //            IEnumerable<char> characters = s.Where(c => (char.IsLetter(c) || char.IsNumber(c)));
        //            return string.Join(string.Empty, characters);
        //        }).ToArray();
        //    foreach (string s in split)
        //    {
        //        result.Add(s);
        //    }
        //}

        return result;

        //return lyrics.Split(" ", StringSplitOptions.RemoveEmptyEntries 
        //    | StringSplitOptions.TrimEntries)
        //    .Select(s =>
        //    {
        //        IEnumerable<char> characters = s.Where(c => (char.IsLetter(c) || char.IsNumber(c)));
        //        return string.Join(string.Empty, characters);
        //    });
    }

    public static IEnumerable<string> GetCmuDict(string? path = null)
    {
        path ??= Path.Combine("..", "..", "..", "cmudict-2.txt");
        if (path.Trim().Length is 0)
        {
            throw new ArgumentException($"{nameof(path)} cannot be empty or whitespace.");
        }
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }
        List<string> result = [];
        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            using (StreamReader sr = new StreamReader(fileStream))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.StartsWith("##"))
                    {
                        result.Add(line);
                    }
                }
            }
        }

        return result;
    }

    public static IEnumerable<string> GetPlainSyllableDict(string? path = null)
    {
        path ??= Path.Combine("..", "..", "..", "syllabledict.txt");

        if (path.Trim().Length is 0)
        {
            throw new ArgumentException($"{nameof(path)} cannot be empty or whitespace.");
        }
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }
        return File.ReadAllText(path).Split(s_newLineDelimiters, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static int[] GetNumberOfWordsByLineList(string lyrics)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(lyrics);
        string[] splitString = lyrics.Split(s_newLineDelimiters, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        int[] result = new int[splitString.Length];
        for (int i = 0; i < splitString.Length; i++)
        {
            result[i] = splitString[i].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Length;
        }
        return result;
    }
}
