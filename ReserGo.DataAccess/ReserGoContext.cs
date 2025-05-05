using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Security;

namespace ReserGo.DataAccess;

    public class ReserGoContext : DbContext {
	    
	    public DbSet<User> Users { get; set; }
	    public DbSet<Login> Login { get; set; }
	    public DbSet<Hotel> Hotel { get; set; }
	    public DbSet<Occasion> Occasion { get; set; }
	    public DbSet<Restaurant> Restaurant { get; set; }
	    public DbSet<HotelOffer> HotelOffer { get; set; }
	    public DbSet<RestaurantOffer> RestaurantOffer { get; set; }
	    public DbSet<BookingHotel> BookingHotel { get; set; }
	    public DbSet<BookingOccasion> BookingOccasion { get; set; }
	    public DbSet<BookingRestaurant> BookingRestaurant { get; set; }
	    public DbSet<Address> Addresses { get; set; }
	    
        private readonly string _sqlConnectionString;

        public ReserGoContext(IOptions<AppSettings> options) {
			_sqlConnectionString = options.Value.SqlConnectionString;
		}
		// Add this constructor for testing purposes
		public ReserGoContext(DbContextOptions<ReserGoContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseNpgsql(_sqlConnectionString);

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
				.HasMany(u => u.BookingsOccasion)
				.WithOne(w => w.User)
				.HasForeignKey(w => w.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			
			modelBuilder.Entity<User>()
				.HasMany(u => u.BookingsRestaurant)
				.WithOne(w => w.User)
				.HasForeignKey(w => w.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			
			modelBuilder.Entity<User>()
				.HasOne(u => u.Address)
				.WithOne(a => a.User) // Relation One-to-One avec Address
				.HasForeignKey<User>(u => u.AddressId)
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
				.HasMany(u => u.Occasions)
				.WithOne(r => r.User)
				.HasForeignKey(r => r.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			
			// Config relation User-HotelOffer
			modelBuilder.Entity<User>()
				.HasMany(u => u.HotelOffers)
				.WithOne(ho => ho.User)
				.HasForeignKey(ho => ho.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			
			// Config relation User-RestaurantOffer
			modelBuilder.Entity<User>()
				.HasMany(u => u.RestaurantOffers)
				.WithOne(ro => ro.User)
				.HasForeignKey(ro => ro.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Config Hotel, Occasion, Restaurant
			modelBuilder.Entity<Hotel>()
				.HasKey(h => h.Id);
			modelBuilder.Entity<Hotel>()
				.HasIndex(h => h.StayId)
				.IsUnique(); 
			
			modelBuilder.Entity<Occasion>()
				.HasKey(o => o.Id);
			modelBuilder.Entity<Occasion>()
				.HasIndex(o => o.StayId)
				.IsUnique();
			
			modelBuilder.Entity<Restaurant>()
				.HasKey(r => r.Id);
			modelBuilder.Entity<Restaurant>()
				.HasIndex(r => r.StayId)
				.IsUnique();
			
			// Config Bookings 
			
			// BookingHotel
			modelBuilder.Entity<BookingHotel>()
				.HasKey(b => b.Id);
			
			modelBuilder.Entity<BookingHotel>()
				.HasOne(b => b.User)
				.WithMany(u => u.BookingsHotel) 
				.HasForeignKey(b => b.UserId);
			
			modelBuilder.Entity<BookingHotel>()
				.HasOne(b => b.Hotel)
				.WithMany(h => h.BookingsHotel)
				.HasForeignKey(b => b.HotelId);
			
			// Config HotelOffer
			modelBuilder.Entity<HotelOffer>()
				.HasKey(ho => ho.Id);
			modelBuilder.Entity<HotelOffer>()
				.HasOne(ho => ho.Hotel)
				.WithMany(h => h.HotelOffers)
				.HasForeignKey(ho => ho.HotelId)
				.OnDelete(DeleteBehavior.Cascade);
			
			// Config RestaurantOffer
			modelBuilder.Entity<RestaurantOffer>()
				.HasKey(ro => ro.Id);
			modelBuilder.Entity<RestaurantOffer>()
				.HasOne(ro => ro.Restaurant)
				.WithMany(r => r.RestaurantOffers)
				.HasForeignKey(ro => ro.RestaurantId)
				.OnDelete(DeleteBehavior.Cascade);
			
			// BookingOccasion
			modelBuilder.Entity<BookingOccasion>()
				.HasKey(b => b.Id);
			
			modelBuilder.Entity<BookingOccasion>()
				.HasOne(b => b.User)
				.WithMany(u => u.BookingsOccasion) 
				.HasForeignKey(b => b.UserId);
			
			modelBuilder.Entity<BookingOccasion>()
				.HasOne(b => b.Occasion)
				.WithMany(h => h.BookingsOccasion)
				.HasForeignKey(b => b.OccasionId);
			
			// BookingRestaurant
			modelBuilder.Entity<BookingRestaurant>()
				.HasKey(b => b.Id);
			
			modelBuilder.Entity<BookingRestaurant>()
				.HasOne(b => b.User)
				.WithMany(u => u.BookingsRestaurant) 
				.HasForeignKey(b => b.UserId);
			
			modelBuilder.Entity<BookingRestaurant>()
				.HasOne(b => b.Restaurant)
				.WithMany(h => h.BookingRestaurant)
				.HasForeignKey(b => b.RestaurantId);
		}
    }

