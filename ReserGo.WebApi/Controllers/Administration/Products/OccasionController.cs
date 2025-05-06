using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Occasion;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.Common.Models;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[AdminOnly]
[ApiController]
[Tags("Products | Occasion")] 
[Route("api/administration/products/occasions/")]
public class OccasionController : ControllerBase {
    
    private readonly ILogger<OccasionController> _logger;
    private readonly IOccasionService _occasionService;
    private readonly ISecurity _security;

    public OccasionController(ILogger<OccasionController> logger, IOccasionService occasionService, ISecurity security) {
        _logger = logger;
        _security = security;
        _occasionService = occasionService;
    }
    
    /// <summary>
    /// Create a new occasion.
    /// </summary>
    /// <param name="request">The occasion creation request containing necessary information.</param>
    /// <returns>The created occasion object.</returns>
    /// <response code="201">Occasion created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(OccasionCreationRequest request) {
        try {
            OccasionDto data = await _occasionService.Create(request);

            var resource = new Resource<OccasionDto> {
                Data = data,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetById), new { id = data.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
                        Href = Url.Action(nameof(Update), new { id = data.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Created("create", resource);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while creating the occasion.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Retrieve an occasion by their ID.
    /// </summary>
    /// <param name="id">The ID of the occasion.</param>
    /// <returns>The Occasion object.</returns>
    /// <response code="200">Occasion found and returned.</response>
    /// <response code="404">Occasion not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<OccasionDto>>> GetById(int id) {
        try {
            var occasion = await _occasionService.GetById(id);
            if (occasion == null) {
                return NotFound($"Occasion with ID {id} not found.");
            }

            var resource = new Resource<OccasionDto> {
                Data = occasion,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetById), new { id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
                        Href = Url.Action(nameof(Update), new { id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the occasion.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Retrieve an occasion by their StayId.
    /// </summary>
    /// <param name="id">The StayId of the occasion.</param>
    /// <returns>The Occasion object.</returns>
    /// <response code="200">Occasion found and returned.</response>
    /// <response code="404">Occasion not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<OccasionDto>>> GetByStayId(long id) {
        try {
            var occasion = await _occasionService.GetByStayId(id);
            if (occasion == null) {
                return NotFound($"Occasion with StayId {id} not found.");
            }

            var resource = new Resource<OccasionDto> {
                Data = occasion,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetByStayId), new { id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
                        Href = Url.Action(nameof(Update), new { id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the occasion.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Retrieve occasions for the connected user.
    /// </summary>
    /// <returns>A list of occasions associated with the connected user.</returns>
    /// <response code="200">Occasions retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-occasions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<OccasionDto>>>>> GetOccasionsForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) {
                return Unauthorized("User not authenticated");
            }

            var occasions = await _occasionService.GetOccasionsByUserId(connectedUser.UserId);

            var resources = occasions.Select(occasion => new Resource<OccasionDto> {
                Data = occasion,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetById), new { id = occasion.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
                        Href = Url.Action(nameof(Update), new { id = occasion.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            });

            var resourceCollection = new Resource<IEnumerable<Resource<OccasionDto>>> {
                Data = resources,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetOccasionsForConnectedUser)),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resourceCollection);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving occasions for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Update an existing occasion.
    /// </summary>
    ///  <param name="id">The stayId to search the object.</param>
    /// <param name="request">The Occasion update request.</param>
    /// <returns>The updated Occasion object.</returns>
    /// <response code="200">Occasion updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Occasion not found.</response>
    [HttpPut("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<OccasionDto>>> Update(long id, OccasionUpdateRequest request) {
        try {
            var updatedOccasion = await _occasionService.Update(id, request);

            var resource = new Resource<OccasionDto> {
                Data = updatedOccasion,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetById), new { id }),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            return NotFound(ex.Message);
        }
    }
    
    /// <summary>
    /// Remove an Occasion by their ID.
    /// </summary>
    /// <param name="id">The ID of the Occasion to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Occasion removed successfully.</response>
    /// <response code="404">Occasion not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id) {
        try {
            await _occasionService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the Occasion.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
