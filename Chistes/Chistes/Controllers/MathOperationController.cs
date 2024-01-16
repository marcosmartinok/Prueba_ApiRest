using Chistes.Api.Services;
using Microsoft.AspNetCore.Mvc;


namespace Chistes.Controllers
{
	[Route("[controller]")]
	public class MathOperationController : ControllerBase
	{
		[HttpGet]
		[Route("api/matematica/MCM")]
		public IActionResult GetLCM(List<int> numbers)
		{
			try
			{
				int lcm = MathOperationService.CalculateLCM(numbers);
				return Ok(lcm);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("api/matematica/Incremento")]
		public IActionResult GetIncrement(int number)
		{
			try
			{
				int nextNumber = MathOperationService.CalculateNextNumber(number);
				return Ok(nextNumber);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}

	}
}
