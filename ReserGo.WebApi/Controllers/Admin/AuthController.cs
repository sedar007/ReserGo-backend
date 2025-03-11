using ReserGo.Common.Requests.Security;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Security;
using ReserGo.Shared.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ReserGo.WebAPI.Controllers.Admin {

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase {

        private readonly IAuthService _authService;
        private readonly ISecurity _security;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, ISecurity security) {
            _authService = authService;
            _logger = logger;
            _security = security;
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Loging([FromBody] LoginRequest request) {
            try {
                var response = await _authService.Login(request);

                if (response == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");

                var cookieOptions = new CookieOptions {
                   // Domain = "resergo-admin.adjysedar.fr",
                    HttpOnly = true, // Prevents access via JavaScript (XSS protection)
                    Secure = true, // Only active in HTTPS
                    SameSite = SameSiteMode.None, // Prevents CSRF attacks
                    Expires = DateTime.UtcNow.AddMinutes(30) // Expiration duration
                };

                Response.Cookies.Append("AuthToken", response.Token, cookieOptions);
                _logger.LogInformation("Token stored in HTTP-only cookie");

                _logger.LogInformation("User logged in");
                return Ok(new { message = "Login successful", response.Id, response.Role,  response.Username, response.RoleName, response.Token });
            }

            catch (ArgumentNullException e) {
                _logger.LogWarning("User not unauthorized");
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (KeyNotFoundException e) {
                _logger.LogWarning("User not unauthorized");
                return Unauthorized($"The user: {request.Login} or the password is Incorrect");
            }
            catch (Exception e) {
                _logger.LogWarning("User not unauthorized");
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");
            }
        }

        /// <summary>
        /// Gets the current authenticated user.
        /// </summary>
        /// <returns>User info if authenticated.</returns>
        [HttpGet("me")]
        [Authorize] // Ensure the user is authenticated
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentUser() {
            CurrentUser? currentUser = _security.GetCurrentUser();
            if (currentUser == null) {
                return Unauthorized(new { message = "User not authenticated" });
            }

            return Ok(currentUser);
        }

        /// <summary>
        /// Logs out the current user by deleting the auth token cookie.
        /// </summary>
        /// <returns>Success message.</returns>
        [HttpPost("logout")]
        [Authorize] // Ensure only authenticated users can log out
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout() {
            Console.WriteLine("Logout");
            Response.Cookies.Delete("AuthToken");
            _logger.LogInformation("User logged out");
            return Ok(new { message = "Logout successful" });
        }

    }
}
