using Microsoft.AspNetCore.Mvc;

namespace ReserGo.WebAPI.Controllers;

[ApiController]
[Route("")]
public class HealthController : ControllerBase {
    /// <summary>
    ///     Checks the health status of the API.
    /// </summary>
    /// <returns>A message indicating that the API is online.</returns>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetHealth() {
        return Ok("API is alive");
    }
}