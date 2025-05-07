using System.ComponentModel.DataAnnotations;

namespace Rhyme.Models;

public class WordWithPlainSyllables
{
    public int WordWithPlainSyllablesId { get; set; }
    [Required]
    public required string Word { get; set; }
    [Required]
    public required string[] Syllables { get; set; }
}
