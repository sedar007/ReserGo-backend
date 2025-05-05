using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[ApiController]
[Tags("Offers | Restaurant")] 
[AdminOnly]
[Route("api/administration/offers/restaurants/")]
public class RestaurantOfferController : ControllerBase {
    
    private readonly ILogger<RestaurantController> _logger;
    private readonly IRestaurantOfferService _restaurantOfferService;
    private readonly ISecurity _security;

    public RestaurantOfferController(ILogger<RestaurantController> logger, IRestaurantOfferService restaurantOfferService, ISecurity security) {
        _logger = logger;
        _restaurantOfferService = restaurantOfferService;
        _security = security;
    }
    
    /// <summary>
    /// Create a new restaurant offer.
    /// </summary>
    /// <param name="request">The restaurant offer creation request containing necessary information.</param>
    /// <returns>The created restaurant offer object.</returns>
    /// <response code="201">Restaurant offer created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(RestaurantOfferCreationRequest request) {
        try {
            RestaurantOfferDto data = await _restaurantOfferService.Create(request);
            return Created("create", data);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while creating the restaurant offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    

    /// <summary>
    /// Retrieve a restaurant offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant offer.</param>
    /// <returns>The restaurant offer object.</returns>
    /// <response code="200">Restaurant offer found and returned.</response>
    /// <response code="404">Restaurant offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RestaurantOfferDto?>> GetById(int id) {
        try {
            var restaurantOffer = await _restaurantOfferService.GetById(id);
            return Ok(restaurantOffer);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the restaurant offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve restaurant offers for the connected user.
    /// </summary>
    /// <returns>A list of restaurant offers associated with the connected user.</returns>
    /// <response code="200">Restaurant offers retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-offers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RestaurantOfferDto>>> GetOffersForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) {
                return Unauthorized("User not authenticated");
            }

            var restaurantOffers = await _restaurantOfferService.GetRestaurantsByUserId(connectedUser.UserId);
            return Ok(restaurantOffers);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving restaurant offers for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Update an existing restaurant offer.
    /// </summary>
    /// <param name="id">The ID of the restaurant offer to update.</param>
    /// <param name="request">The restaurant offer update request.</param>
    /// <returns>The updated restaurant offer object.</returns>
    /// <response code="200">Restaurant offer updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Restaurant offer not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RestaurantOfferDto>> Update(int id, RestaurantOfferUpdateRequest request) {
        try {
            var updatedOffer = await _restaurantOfferService.Update(id, request);
            return Ok(updatedOffer);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Remove a restaurant offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant offer to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Restaurant offer removed successfully.</response>
    /// <response code="404">Restaurant offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id) {
        try {
            await _restaurantOfferService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while deleting the restaurant offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
