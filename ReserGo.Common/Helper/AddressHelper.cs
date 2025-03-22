using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;
public static class AddressHelper {
    public static AddressDto ToDto(this Address address) {
        return new AddressDto {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            Country = address.Country,
        };
    }
}
