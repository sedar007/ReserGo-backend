using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;

using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.User;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;
public class HotelService : IHotelService {
    
    private readonly ILogger<UserService> _logger;
    private readonly IHotelDataAccess _hotelDataAccess;
    
    public HotelService(ILogger<UserService> logger, IHotelDataAccess hotelDataAccess) {
        _logger = logger;
        _hotelDataAccess = hotelDataAccess;
    }
    
    public async Task<HotelDto> Create(HotelCreationRequest request) {
        try {
            // TODO à changer avec la recherche avec stayId 
            /*Hotel? userByUsername = await _userDataAccess.GetByUsername(request.Username);
            if (userByUsername is not null) {
                string errorMessage = "This username is already in use.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }*/
            
            string error = HotelValidator.GetErrorCreationRequest(request);
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }
            
            Hotel newHotel = new Hotel {
                Name = request.Name,
                Location = request.Location,
                Capacity = request.Capacity
            };
            
            newHotel = await _hotelDataAccess.Create(newHotel);
            
            _logger.LogInformation("Hotel { id } created", newHotel.Id);
            return newHotel.ToDto();
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task<HotelDto?> GetById(int id) {
        try {
            Hotel? hotel = await _hotelDataAccess.GetById(id);
            if (hotel is null) {
                string errorMessage = "This hotel does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            _logger.LogInformation("Hotel { id } retrieved successfully", hotel.Id);
            HotelDto hotelDto = hotel.ToDto();
            return hotelDto;
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}