using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReserGo.Common.Requests.Security;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Security;
using ReserGo.Shared.Interfaces;
using ReserGo.Shared;
using ReserGo.Common.Response;
using ReserGo.Common.Models;

namespace ReserGo.WebAPI.Controllers.Security.Admin {
    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase {
        private readonly ILoginService _loginService;
        private readonly ISecurity _security;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginService loginService, ILogger<LoginController> logger, ISecurity security) {
            _loginService = loginService;
            _logger = logger;
            _security = security;
        }

        /// <summary>
        /// Authenticates a user and returns a login response with HATEOAS links.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>
        /// A 200 OK response with the login details and HATEOAS links if successful.
        /// A 401 Unauthorized response if the credentials are invalid.
        /// A 400 Bad Request response if the request is malformed.
        /// A 500 Internal Server Error response if an unexpected error occurs.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resource<LoginResponse>>> Loging([FromBody] LoginRequest request) {
            try {
                AuthenticateResponse? auth = await _loginService.Login(request);

                if (auth == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");

                CookieOptions cookieOptions = _security.GetCookiesOptions();
                Response.Cookies.Append(Consts.AuthToken, auth.Token, cookieOptions);

                var response = new LoginResponse {
                    Message = "Login successful",
                    Id = auth.Id,
                    Role = auth.Role,
                    Username = auth.Username,
                    RoleName = auth.RoleName
                };

                var resource = new Resource<LoginResponse> {
                    Data = response,
                    Links = new List<Link> {
                        new Link {
                            Href = Url.Action(nameof(GetCurrentUser)),
                            Rel = "self",
                            Method = "GET"
                        },
                        new Link {
                            Href = Url.Action(nameof(Logout)),
                            Rel = "logout",
                            Method = "POST"
                        }
                    }
                };

                return Ok(resource);
            }
            catch (ArgumentNullException e) {
                _logger.LogWarning("User not unauthorized");
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (KeyNotFoundException e) {
                _logger.LogWarning("User not unauthorized");
                _logger.LogError(e, e.Message);
                return Unauthorized($"The user: {request.Login} or the password is Incorrect");
            }
            catch (Exception e) {
                _logger.LogWarning("User not unauthorized");
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");
            }
        }

        /// <summary>
        /// Retrieves the current authenticated user's information with HATEOAS links.
        /// </summary>
        /// <returns>
        /// A 200 OK response with the user information and HATEOAS links if authenticated.
        /// A 401 Unauthorized response if the user is not authenticated.
        /// </returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<Resource<ConnectedUser>> GetCurrentUser() {
            ConnectedUser? currentUser = _security.GetCurrentUser();
            if (currentUser == null) {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var resource = new Resource<ConnectedUser> {
                Data = currentUser,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetCurrentUser)),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
                        Href = Url.Action(nameof(Logout)),
                        Rel = "logout",
                        Method = "POST"
                    }
                }
            };

            return Ok(resource);
        }

        /// <summary>
        /// Logs out the current user by deleting the authentication token cookie.
        /// </summary>
        /// <returns>
        /// A 200 OK response with a success message and HATEOAS links.
        /// </returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Resource<object>> Logout() {
            Response.Cookies.Delete(Consts.AuthToken);
            _logger.LogInformation("User logged out");

            var resource = new Resource<object> {
                Data = new { message = "Logout successful" },
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(Loging)),
                        Rel = "login",
                        Method = "POST"
                    }
                }
            };

            return Ok(resource);
        }
    }
}