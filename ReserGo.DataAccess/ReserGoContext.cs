using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReserGo.Common.Entity;
using ReserGo.Common.Security;

namespace ReserGo.DataAccess;

    public class ReserGoContext : DbContext {
	    
	    public DbSet<User> Users { get; set; }
	    public DbSet<Login> Login { get; set; }
	    public DbSet<Hotel> Hotel { get; set; }
	    public DbSet<Occasion> Occasion { get; set; }
	    public DbSet<Restaurant> Restaurant { get; set; }
	    public DbSet<BookingHotel> BookingHotel { get; set; }
	    public DbSet<BookingOccasion> BookingOccasion { get; set; }
	    public DbSet<BookingRestaurant> BookingRestaurant { get; set; }
	    
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

			// Config Hotel, Occasion, Restaurant
			modelBuilder.Entity<Hotel>()
				.HasKey(h => h.Id);
			
			modelBuilder.Entity<Occasion>()
				.HasKey(o => o.Id);
			
			modelBuilder.Entity<Restaurant>()
				.HasKey(r => r.Id);
			
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

