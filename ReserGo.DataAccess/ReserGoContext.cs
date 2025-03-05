using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using ReserGo.Common.Security;

namespace ReserGo.DataAccess;

public class ReserGoContext {
    public class GameContext : DbContext {
        private string SQLConnectionString;

        public GameContext(IOptions<AppSettings> options)
		{
			SQLConnectionString = options.Value.SqlConnectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseNpgsql(SQLConnectionString);

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
		}

    }

}