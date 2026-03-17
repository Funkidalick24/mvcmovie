using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new MvcMovieContext(
            serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>());

        if (context.Movie.Any())
        {
            return;
        }

        // Feel free to swap these with your personal favorites.
        context.Movie.AddRange(
            new Movie
            {
                Title = "The Matrix",
                ReleaseDate = new DateTime(1999, 3, 31),
                Genre = "Sci Fi",
                Price = 9.99m,
                Rating = "R"
            },
            new Movie
            {
                Title = "Inception",
                ReleaseDate = new DateTime(2010, 7, 16),
                Genre = "Sci Fi",
                Price = 10.99m,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Spirited Away",
                ReleaseDate = new DateTime(2001, 7, 20),
                Genre = "Animation",
                Price = 8.99m,
                Rating = "PG"
            });

        context.SaveChanges();
    }
}
