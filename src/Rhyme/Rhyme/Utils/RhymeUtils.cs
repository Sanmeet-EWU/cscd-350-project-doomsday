using Rhyme.Data;
using Rhyme.FileIO;
using System.Drawing;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;

namespace Rhyme.Utils;

public class RhymeUtils
{
    public AppDbContext Db { get; set; }

    private static string[] Colors = [
        "crimson",
        "lime",
        "palegreen",
        "darkorchid",
        "chartreuse",
        "sienna",
        "goldenrod",
        "teal",
        "navy",
        "olive",
        "maroon",
        "turquoise",
        "salmon",
        "plum",
        "peru",
    ];
    public RhymeUtils(AppDbContext db)
    {
        Db = db;
    }

    public List<Word> Run()
    {
        string sampleText = FileUtils.GetLyrics("sample-text.txt");
        int[] numberOfWordsEachLine = FileUtils.GetNumberOfWordsByLineList(sampleText);
        IEnumerable<string> words = FileUtils.GetWordsList(sampleText);
        List<string[]> phonemesByWord = GetPronunciation(words);

        int syllablesCount = 0;
        foreach (string[] word in phonemesByWord)
        {
            syllablesCount += word.Length;
        }
        int[,] rhymeScoreMapper = new int[syllablesCount, syllablesCount];
        List<List<int>> rhymeMatchMapper = new(syllablesCount);
        for (int i = 0; i < syllablesCount; i++)
        {
            rhymeMatchMapper.Add(new List<int>());
        }

        for (int outerIndex = 0; outerIndex < syllablesCount; outerIndex++)
        {
            for (int innerIndex = 0; innerIndex < syllablesCount; innerIndex++)
            {
                if (outerIndex != innerIndex)
                {
                    string firstSyllable = GetSyllableByIndex(outerIndex, phonemesByWord);
                    string secondSyllable = GetSyllableByIndex(innerIndex, phonemesByWord);
                    int score = ScoreSyllables(firstSyllable, secondSyllable);
                    if (score > 0)
                    {
                        rhymeMatchMapper[outerIndex].Add(innerIndex);
                    }
                    rhymeScoreMapper[innerIndex, outerIndex] = score;
                    rhymeScoreMapper[outerIndex, innerIndex] = score;
                }
            }
        }

        List<Syllable> result = new();
        int colorIndex = 0;
        for (int index = 0; index < syllablesCount; index++)
        {
            string color = "";
            string syllableString = GetSyllableByIndex(index, phonemesByWord);
            if (rhymeMatchMapper[index].Count > 0)
            {
                if (index < rhymeMatchMapper[index][0])
                {
                    color = Colors[colorIndex];
                    colorIndex++;
                }
                else
                {
                    color = result[rhymeMatchMapper[index][0]].Color;
                }
            }
            Syllable syllable = new Syllable(syllableString, color);
            result.Add(syllable);
        }

        return ConvertSyllablesToWords(result, phonemesByWord, words.ToList());
    }

    private List<Word> ConvertSyllablesToWords(List<Syllable> syllables, List<string[]> phonemesByWord, List<string> words)
    {
        List<Word> result = new();

        int syllablesCount = 0;

        for (int i = 0; i < words.Count; i++)
        {
            string[] phonemes = phonemesByWord[i];
            string word = words[i];
            string plainTextWord = words[i].Trim();
            List<string> plainTextSyllables = ConvertWordToPlainTextSyllables(word);
            if (plainTextSyllables.Count != phonemes.Length)
            {
                result.Add(new Word([new Syllable(plainTextWord, "")]));
            }
            else
            {
                List<Syllable> syllablesInWord = new();
                for (
                  int syllableIndex = 0;
                  syllableIndex < plainTextSyllables.Count;
                  syllableIndex++
                )
                {
                    if (syllableIndex > 0)
                    {
                        syllablesCount++;
                    }
                    syllables[syllablesCount].SyllableContent =
                      plainTextSyllables[syllableIndex];
                    syllablesInWord.Add(syllables[syllablesCount]);
                }
                result.Add(new Word(syllablesInWord.ToArray()));
            }
            syllablesCount++;
        }

        return result;
    }

    public List<string> ConvertWordToPlainTextSyllables(string word)
    {
        word = word.Trim().ToUpper();
        Rhyme.Models.Rhyme? foundSyllables = Db.Rhymes.FirstOrDefault(rhyme => rhyme.Word.Equals(word));
        if (foundSyllables == null)
        {
            return [];
        }
        return foundSyllables.PlainSyllables.ToList();
    }

    private List<string[]> GetPronunciation(IEnumerable<string> words)
    {
        List<string[]> phonemesByWord = new List<string[]>();
        foreach (string word in words)
        {
            var foundRhyme = Db.Rhymes.FirstOrDefault(x => x.Word.Equals(word.ToUpper()));
            if (foundRhyme is null)
            {
                phonemesByWord.Add([word]);
            }
            else
            {
                phonemesByWord.Add(foundRhyme.PhonemeSyllables);
            }
        }
        return phonemesByWord;
    }

    private int ScoreSyllables(string firstSyllable, string secondSyllable)
    {
        string[] vowels = [ "AA", "AE", "AH", "AO", "AW", "AY", "EH",
            "ER", "EY", "IH", "IY", "OW", "OY", "UH", "UW" ];
        string[] firstPhonemes = firstSyllable.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string[] secondPhonemes = secondSyllable.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        for (int i = 0; i < firstPhonemes.Length; i++)
        {
            string phonemeWithoutStress = firstPhonemes[i].Length > 2 ? firstPhonemes[i].Substring(0, 2) : firstPhonemes[i];
            if (vowels.Contains(phonemeWithoutStress))
            {
                string? equalPhoneme = secondPhonemes.FirstOrDefault(x => x.StartsWith(phonemeWithoutStress));
                if (equalPhoneme is not null)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    private string GetSyllableByIndex(int index, List<string[]> syllables)
    {
        int counter = 0;
        foreach (string[] word in syllables)
        {
            foreach (string syllable in word)
            {
                if (counter == index)
                {
                    return syllable;
                }
                counter++;
            }
        }
        // change this when you refactor to check against member property containing number of syllables (at start of method)
        throw new ArgumentException("Index is greater than or equal to number of syllables in words.");
    }
}

public class Syllable
{
    public string SyllableContent { get; set; }
    public string Color { get; set; }

    public Syllable(string syllableContent, string color)
    {
        SyllableContent = syllableContent;
        Color = color;
    }
}

public class Word
{
    public Syllable[] Syllables { get; set; } = [];

    public Word(Syllable[] syllables)
    {
        Syllables = syllables;
    }
}