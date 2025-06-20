using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReserGo.Common.Entity;
using ReserGo.Common.Security;

namespace ReserGo.DataAccess;

public class ReserGoContext : DbContext {
    private readonly string _sqlConnectionString;

    public ReserGoContext(IOptions<AppSettings> options) {
        _sqlConnectionString = options.Value.SqlConnectionString;
    }

    // Add this constructor for testing purposes
    public ReserGoContext(DbContextOptions<ReserGoContext> options) : base(options) {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Login> Login { get; set; }
    public DbSet<Hotel> Hotel { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<RoomAvailability> RoomAvailability { get; set; }
    public DbSet<Event> Event { get; set; }
    public DbSet<Restaurant> Restaurant { get; set; }
    public DbSet<HotelOffer> HotelOffer { get; set; }
    public DbSet<RestaurantOffer> RestaurantOffer { get; set; }
    public DbSet<EventOffer> EventOffer { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    public DbSet<BookingHotel> BookingHotel { get; set; }
    public DbSet<BookingEvent> BookingEvent { get; set; }
    public DbSet<BookingRestaurant> BookingRestaurant { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseNpgsql(_sqlConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // config user
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Login)
            .WithOne(l => l.User)
            .HasForeignKey<Login>(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.BookingsHotel)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.BookingsEvent)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.BookingsRestaurant)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config relation User-Hotel
        modelBuilder.Entity<User>()
            .HasMany(u => u.Hotels)
            .WithOne(h => h.User)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config relation User-Restaurant
        modelBuilder.Entity<User>()
            .HasMany(u => u.Restaurants)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config relation User-Events
        modelBuilder.Entity<User>()
            .HasMany(u => u.Events)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config relation User-HotelOffer
        modelBuilder.Entity<User>()
            .HasMany(u => u.HotelOffers)
            .WithOne(ho => ho.User)
            .HasForeignKey(ho => ho.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config relation User-Address
        modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config relation User-RestaurantOffer
        modelBuilder.Entity<User>()
            .HasMany(u => u.RestaurantOffers)
            .WithOne(ro => ro.User)
            .HasForeignKey(ro => ro.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config relation User-EventOffer
        modelBuilder.Entity<User>()
            .HasMany(u => u.EventOffers)
            .WithOne(oo => oo.User)
            .HasForeignKey(oo => oo.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config Hotel
        modelBuilder.Entity<Hotel>()
            .HasKey(h => h.Id);
        modelBuilder.Entity<Hotel>()
            .HasIndex(h => h.StayId)
            .IsUnique();

        modelBuilder.Entity<Hotel>()
            .HasMany(h => h.Rooms) // Configuring one-to-many relationship
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config Room
        modelBuilder.Entity<Room>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Room>()
            .HasIndex(r => new { r.HotelId, r.RoomNumber })
            .IsUnique();


        modelBuilder.Entity<Event>()
            .HasKey(o => o.Id);
        modelBuilder.Entity<Event>()
            .HasIndex(o => o.StayId)
            .IsUnique();

        modelBuilder.Entity<Restaurant>()
            .HasKey(r => r.Id);
        modelBuilder.Entity<Restaurant>()
            .HasIndex(r => r.StayId)
            .IsUnique();

        // Config Bookings 


        // Config HotelOffer
        modelBuilder.Entity<HotelOffer>()
            .HasKey(ho => ho.Id);
        modelBuilder.Entity<HotelOffer>()
            .HasOne(ho => ho.Hotel)
            .WithMany(h => h.HotelOffers)
            .HasForeignKey(ho => ho.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config Address
        modelBuilder.Entity<Address>()
            .HasKey(a => a.Id);
        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithOne(u => u.Address)
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config RestaurantOffer
        modelBuilder.Entity<RestaurantOffer>()
            .HasKey(ro => ro.Id);
        modelBuilder.Entity<RestaurantOffer>()
            .HasOne(ro => ro.Restaurant)
            .WithMany(r => r.RestaurantOffers)
            .HasForeignKey(ro => ro.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Config Address
        modelBuilder.Entity<Address>()
            .HasKey(a => a.Id);

        // Config EventOffer
        modelBuilder.Entity<EventOffer>()
            .HasKey(oo => oo.Id);
        modelBuilder.Entity<EventOffer>()
            .HasOne(oo => oo.Event)
            .WithMany(o => o.EventOffers)
            .HasForeignKey(oo => oo.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // BookingEvent
        modelBuilder.Entity<BookingEvent>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<BookingEvent>()
            .HasOne(b => b.User)
            .WithMany(u => u.BookingsEvent)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<BookingEvent>()
            .HasOne(b => b.Event)
            .WithMany(h => h.BookingsEvent)
            .HasForeignKey(b => b.EventId);

        // BookingRestaurant
        modelBuilder.Entity<BookingRestaurant>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<BookingRestaurant>()
            .HasOne(b => b.User)
            .WithMany(u => u.BookingsRestaurant)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<BookingRestaurant>()
            .HasOne(b => b.RestaurantOffer)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RestaurantOfferId);

        // BookingHotel
        modelBuilder.Entity<BookingHotel>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<BookingHotel>()
            .HasOne(b => b.User)
            .WithMany(u => u.BookingsHotel)
            .HasForeignKey(b => b.UserId);

        // Config Notification
        modelBuilder.Entity<Notification>()
            .HasKey(n => n.Id);
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // Config RoomAvailability
        modelBuilder.Entity<RoomAvailability>()
            .HasKey(ra => ra.Id); // Définir la clé primaire

        modelBuilder.Entity<RoomAvailability>()
            .HasOne(ra => ra.Hotel)
            .WithMany()
            .HasForeignKey(ra => ra.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomAvailability>()
            .HasOne(ra => ra.Room)
            .WithMany()
            .HasForeignKey(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomAvailability>()
            .HasIndex(ra => new
                { ra.RoomId, ra.StartDate, ra.EndDate }) // Index pour éviter les conflits de disponibilité
            .IsUnique();

        modelBuilder.Entity<RoomAvailability>()
            .HasMany(ra => ra.BookingsHotels)
            .WithOne(bh => bh.RoomAvailability)
            .HasForeignKey(bh => bh.RoomAvailabilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}