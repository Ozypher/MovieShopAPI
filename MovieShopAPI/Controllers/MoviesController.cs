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
}