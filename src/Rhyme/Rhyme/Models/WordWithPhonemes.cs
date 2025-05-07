using System.ComponentModel.DataAnnotations;

namespace Rhyme.Models;

public class WordWithPhonemes
{
    public int WordWithPhonemesId { get; set; }
    [Required]
    public required string Word { get; set; }
    [Required]
    public required string[] Phonemes { get; set; }
}
