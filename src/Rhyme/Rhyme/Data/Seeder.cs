using Microsoft.EntityFrameworkCore;
using Rhyme.FileIO;
using Rhyme.Migrations;
using Rhyme.Models;
using RhymeModel = Rhyme.Models.Rhyme;

namespace Rhyme.Data;

public class Seeder
{
    public static async Task Seed(AppDbContext db)
    {
        bool didAddToDb = false;
        if (!db.WordsWithPhonemes.Any())
        {
            didAddToDb = true;
            var cmuWordsWithPhonemes = FileUtils.GetCmuDict();
            foreach (var cmuWordWithPhonemes in cmuWordsWithPhonemes)
            {
                string[] pair = cmuWordWithPhonemes.Split("  ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length != 2)
                {
                    throw new InvalidOperationException("Cmu Dictionary operation: splitting" +
                        " by word-phonemes delimiter does not result in two strings.");
                }
                WordWithPhonemes wordWithPhonemes = new()
                {
                    Word = pair[0],
                    Phonemes = pair[1].Split(" - ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                };
                Console.WriteLine(wordWithPhonemes.Word);
                await db.WordsWithPhonemes.AddAsync(wordWithPhonemes);
            }
            await db.SaveChangesAsync();
        }
        if (!db.WordsWithPlainSyllables.Any())
        {
            didAddToDb = true;
            var plainSyllables = FileUtils.GetPlainSyllableDict();
            foreach (string syllabizedWord in plainSyllables)
            {
                string[] syllables = syllabizedWord.ToUpper().Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                string word = string.Join(String.Empty, syllables);
                WordWithPlainSyllables wordWithPlainSyllables = new()
                {
                    Word = word,
                    Syllables = syllables
                };
                Console.WriteLine(wordWithPlainSyllables.Word);
                await db.WordsWithPlainSyllables.AddAsync(wordWithPlainSyllables);
            }
            await db.SaveChangesAsync();
        }
        if (didAddToDb)
        {
            List<WordWithPlainSyllables> wordsWithPlainSyllables = db.WordsWithPlainSyllables.ToList();
            foreach (WordWithPlainSyllables wordWithPlainSyllables in wordsWithPlainSyllables)
            {
                WordWithPhonemes? foundWordWithPhonemes = await db.WordsWithPhonemes
                    .FirstOrDefaultAsync(w => wordWithPlainSyllables.Word.Equals(w.Word));
                if (foundWordWithPhonemes is not null)
                {
                    RhymeModel rhyme = new()
                    {
                        Word = wordWithPlainSyllables.Word,
                        PhonemeSyllables = foundWordWithPhonemes.Phonemes,
                        PlainSyllables = wordWithPlainSyllables.Syllables,
                    };
                    Console.WriteLine("Rhyme db add: " +  wordWithPlainSyllables.Word);
                    await db.Rhymes.AddAsync(rhyme);
                }
            }
            await db.SaveChangesAsync();
        }
    }
}
