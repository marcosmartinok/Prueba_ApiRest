using Chistes.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chistes.Controllers
{
	[Route("[controller]")]
	public class JokeController : ControllerBase
	{
		private readonly JokeRepository _jokeRepository;

		public JokeController(JokeRepository jokeRepository)
		{
			_jokeRepository = jokeRepository;
		}

		[HttpGet]
		[Route("api/jokes")]
		public async Task<IActionResult> GetAsync(string source = "")
		{
			try
			{
				if (string.IsNullOrWhiteSpace(source))
				{
					return Ok(await _jokeRepository.ReturnRandomJokeAsync());
				}
				else if (source.Equals("Chuck"))
				{
					return Ok(await _jokeRepository.ReturnRandomChuckJoke());
				}
				else if (source.Equals("Dad"))
				{
					return Ok(await _jokeRepository.ReturnRandomDadJoke());
				}
				else
				{
					return BadRequest("Origen no válido. Debe ser 'Chuck' o 'Dad'.");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}

		[HttpPost]
		[Route("api/jokes")]
		public async Task<IActionResult> PostAsync(string joke)
		{
			try
			{
				await _jokeRepository.SaveJoke(joke);
				return Ok("Chiste guardado correctamente");
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}

		[HttpPut]
		[Route("api/jokes")]
		public async Task<IActionResult> UpdateAsync(int number, string joke)
		{
			try
			{
				await _jokeRepository.UpdateJoke(number, joke);
				return Ok("Chiste actualizado correctamente");
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}

		[HttpDelete]
		[Route("api/jokes")]
		public async Task<IActionResult> DeleteAsync(int number)
		{
			try
			{
				await _jokeRepository.DeleteJoke(number);
				return Ok("Chiste eliminado correctamente");
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("api/jokes/GetFromDb")]
		public async Task<IActionResult> GetFromDbAsync()
		{
			try
			{
				var jokes = await _jokeRepository.GetAllJokesAsync();

				if (jokes != null && jokes.Any())
				{
					return Ok(jokes);
				}
				else
				{
					return NotFound("No jokes found.");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}
	}
}
