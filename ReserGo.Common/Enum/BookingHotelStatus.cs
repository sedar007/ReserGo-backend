namespace ReserGo.Common.Enum;
public enum BookingHotelStatus {
    Published,       // La réservation a été publiée
    Pending,         // En attente de confirmation
    Confirmed,       // Réservation confirmée
    Cancelled,       // Réservation annulée
    CheckedIn,       // Client enregistré (check-in)
    CheckedOut       // Client parti (check-out)
}
