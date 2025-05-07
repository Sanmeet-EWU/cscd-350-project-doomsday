using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyme.Models;

public class Rhyme
{
    public int RhymeId { get; set; }

    [Required]
    public required string Word { get; set; }
    [Required]
    public required string[] PlainSyllables { get; set; }
    [Required]
    public required string[] PhonemeSyllables { get; set; }
}