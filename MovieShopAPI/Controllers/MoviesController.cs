using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers;
//attribute routing
[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Route("{id:int}")]
    [HttpGet]
    public async Task<IActionResult> GetMovie(int id)
    {
        var movie = await _movieService.GetMovieDetails(id);
        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }
    [Route("top-grossing")]
    [HttpGet]
    public async Task<IActionResult> GetTop30Grossing()
    {
        var moviemodel = await _movieService.GetTop30GrossingMovies();
        if (moviemodel == null)
        {
            return NotFound();
        }

        return Ok(moviemodel);
    }

    [Route("genre/{genreId}")]
    [HttpGet]
    public async Task<IActionResult> GetGenreWithPagination(int genreId)
    {
        var movies = await _movieService.GetMoviesByGenrePagination(genreId);
        if (movies == null)
        {
            return NotFound();
        }

        return Ok(movies);
    }
}