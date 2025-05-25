namespace ReserGo.Shared;

public static class Consts {
    public const string ApplicationName = "ReserGo";
    public const string CorsPolicy = "CORS_POLICY";
    public const string AuthToken = "AuthToken";
    public const string DefaultProfile = "system/w8k94kbgqqu6zytrrgjg";
    public const int CacheDurationMinutes = 30;
    public const int CookiesExpiration = 120;
    
    
    // User Message
    public const string UserNotFound = "User not found";
    
    
    
    // Restaurant Offer
    // Cache
    public const string RestaurantOffersUserIdCacheKey = "restaurantOffers_user_";
    public const string RestaurantOffersCacheKey = "restaurantOffers_";
    public const string CacheKeyAvailabilitiesUser = "availabilities_user_{0}_{1}_{2}";
    public const string CacheKeyAvailabilitiesHotel = "availabilities_hotel_{0}_{1}_{2}";

        
}