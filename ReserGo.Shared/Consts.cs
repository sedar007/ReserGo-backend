namespace ReserGo.Shared;

public static class Consts {
    public const string ApplicationName = "ReserGo";
    public const string CorsPolicy = "CORS_POLICY";
    public const string AuthToken = "AuthToken";
    public const string DefaultProfile = "system/w8k94kbgqqu6zytrrgjg";
    public const int CacheDurationMinutes = 30;
    public const int CookiesExpiration = 30;
    public const string ReceiveNotification = "ReceiveNotification";
    public const string UnexpectedError = "An unexpected error occurred.";
    public const string UserNotExist = "This user does not exist.";
    public const string UnauthorizedAccess = "Unauthorized access.";
    public const string Key = "Key";
    public const string Issuer = "Issuer";
    public const string Audience = "Audience";
    public const string ExpireMinutes = "ExpireMinutes";
    public const string NotificationHubPath = "/hubs/notifications";
    
    
    public const int DefaultPageSize = 50;


    // User Message
    public const string UserNotFound = "User not found";


    // Restaurant Offer
    // Cache
    public const string RestaurantOffersUserIdCacheKey = "restaurantOffers_user_{0}";

    public const string RestaurantOffersCacheKey = "restaurantOffers_{0}";

    public const string CacheKeyAvailabilitiesHotel = "availabilities_hotel_{0}_{1}_{2}";
    public const string CacheKeyHotelOffers = "hotelOffers_{0}";
    public const string CacheKeyAvailabilitiesRoom = "availabilities_room_{0}_{1}_{2}";
}