using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Response;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;

namespace ReserGo.WebAPI.Controllers.Security.Admin;

[ApiController]
[Route("api/auth/google")]
public class GoogleController : ControllerBase {
    private readonly IGoogleService _googleService;
    private readonly ILogger<GoogleController> _logger;
    private readonly ISecurity _security;

    public GoogleController(IGoogleService googleService, ILogger<GoogleController> logger, ISecurity security) {
        _googleService = googleService;
        _logger = logger;
        _security = security;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Auth([FromBody] string credential) {
        try {
            var auth = await _googleService.Auth(credential);

            if (auth == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");
            var cookieOptions = _security.GetCookiesOptions();

            Response.Cookies.Append(Consts.AuthToken, auth.Token, cookieOptions);
            _logger.LogInformation("Token stored in HTTP-only cookie");
            _logger.LogInformation("User logged in");
            var response = new LoginResponse {
                Message = "Login successful",
                Id = auth.Id,
                Role = auth.Role,
                Username = auth.Username,
                RoleName = auth.RoleName
            };
            return Ok(response);
        }

        catch (ArgumentNullException e) {
            _logger.LogWarning("User not unauthorized");
            _logger.LogError(e, e.Message);
            return BadRequest();
        }
        catch (KeyNotFoundException e) {
            _logger.LogWarning("User not unauthorized");
            _logger.LogError(e, e.Message);
            return Unauthorized("User not authorized");
        }
        catch (Exception e) {
            _logger.LogWarning("User not unauthorized");
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");
        }
    }
}