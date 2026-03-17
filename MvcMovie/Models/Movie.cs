using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [StringLength(30)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [Range(1, 100)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    // New field requirement: rating for the movie.
    [Required]
    [StringLength(5)]
    [RegularExpression(@"^(G|PG|PG-13|R|NC-17)$")]
    public string Rating { get; set; } = string.Empty;
}

