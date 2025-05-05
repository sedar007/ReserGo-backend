using Microsoft.AspNetCore.Mvc;

using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[AdminOnly]
[ApiController]
[Tags("Products | Restaurant")] 
[Route("api/administration/products/restaurants/")]
public class RestaurantController : ControllerBase {
    
    private readonly ISecurity _security;
    private readonly ILogger<RestaurantController> _logger;
    private readonly IRestaurantService _restaurantService;
    
    public RestaurantController(ISecurity security, ILogger<RestaurantController> logger, IRestaurantService restaurantService) {
        _security = security;
        _logger = logger;
        _restaurantService = restaurantService;
    }
    
    /// <summary>
    /// Create a new restaurant.
    /// </summary>
    /// <param name="request">The restaurant creation request containing necessary information.</param>
    /// <returns>The created restaurant object.</returns>
    /// <response code="201">Restaurant created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(RestaurantCreationRequest request) {
        try {
            RestaurantDto data = await _restaurantService.Create(request);
            return Created("create", data);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the Restaurant.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
        
    }
    
    /// <summary>
    /// Retrieve a Restaurant by their ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant.</param>
    /// <returns>The restaurant object.</returns>
    /// <response code="200">Restaurant found and returned.</response>
    /// <response code="404">Restaurant not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RestaurantDto?>> GetById(int id) {
        try {
            var restaurant = await _restaurantService.GetById(id);
            return Ok(restaurant);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the restaurant.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Retrieve a restaurant by their StayId.
    /// </summary>
    /// <param name="id">The StayId of the restaurant.</param>
    /// <returns>The restaurant object.</returns>
    /// <response code="200">Restaurant found and returned.</response>
    /// <response code="404">Restaurant not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RestaurantDto?>> GetByStayId(long id) {
        try {
            var restaurant = await _restaurantService.GetByStayId(id);
            return Ok(restaurant);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the Restaurant.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Retrieve restaurants for the connected user.
    /// </summary>
    /// <returns>A list of restaurants associated with the connected user.</returns>
    /// <response code="200">Restaurants retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-restaurants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetRestaurantsForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) {
                return Unauthorized("User not authenticated");
            }

            var hotels = await _restaurantService.GetRestaurantsByUserId(connectedUser.UserId);
            return Ok(hotels);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving restaurants for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Update an existing restaurant.
    /// </summary>
    /// <param name="id">The stayId to search the object.</param>
    /// <param name="request">The restaurant update request.</param>
    /// <returns>The updated Restaurant object.</returns>
    /// <response code="200">Restaurant updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Restaurant not found.</response>
    [HttpPut("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Update(long id, RestaurantUpdateRequest request)
    {
        try {
            var updatedUser = await _restaurantService.Update(id, request);
            return Ok(updatedUser);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            return NotFound(ex.Message);
        }
    }
    
    /// <summary>
    /// Remove an Restaurant by their ID.
    /// </summary>
    /// <param name="id">The ID of the Restaurant to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Restaurant removed successfully.</response>
    /// <response code="404">Restaurant not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id) {
        try {
            await _restaurantService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the Restaurant.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
