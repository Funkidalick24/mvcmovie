using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers;

public class MoviesController : Controller
{
    private readonly MvcMovieContext _context;

    public MoviesController(MvcMovieContext context)
    {
        _context = context;
    }

    // GET: Movies
    public async Task<IActionResult> Index(string? movieGenre, int? movieYear, string? searchString)
    {
        IQueryable<string> genreQuery = _context.Movie
            .OrderBy(m => m.Genre)
            .Select(m => m.Genre)
            .Distinct();

        IQueryable<int> yearQuery = _context.Movie
            .Select(m => m.ReleaseDate.Year)
            .Distinct()
            .OrderByDescending(y => y);

        var movies = _context.Movie.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            movies = movies.Where(s => s.Title.Contains(searchString));
        }

        if (!string.IsNullOrWhiteSpace(movieGenre))
        {
            movies = movies.Where(x => x.Genre == movieGenre);
        }

        // Requirement: select a year; show movies released in that year or newer.
        if (movieYear is not null)
        {
            movies = movies.Where(m => m.ReleaseDate.Year >= movieYear);
        }

        var vm = new MovieGenreYearViewModel
        {
            Genres = new SelectList(await genreQuery.ToListAsync()),
            Years = new SelectList(await yearQuery.ToListAsync()),
            Movies = await movies
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync(),
            MovieGenre = movieGenre,
            MovieYear = movieYear,
            SearchString = searchString
        };

        return View(vm);
    }

    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Movies/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        _context.Add(movie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: Movies/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        try
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(movie.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}

