using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models;

public class MovieGenreYearViewModel
{
    public List<Movie> Movies { get; set; } = new();

    public SelectList? Genres { get; set; }
    public SelectList? Years { get; set; }

    [Display(Name = "Genre")]
    public string? MovieGenre { get; set; }

    [Display(Name = "Year or newer")]
    public int? MovieYear { get; set; }

    [Display(Name = "Title contains")]
    public string? SearchString { get; set; }
}
