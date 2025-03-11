using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using NLog;
using NLog.Web; 
using Microsoft.AspNetCore.Authentication.Cookies;

using ReserGo.DataAccess.Interfaces;
using ReserGo.DataAccess.Implementations;
using ReserGo.Shared.Interfaces;
using ReserGo.Shared.Implementations;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Implementations;
using ReserGo.Common.Security;

using ReserGo.DataAccess;
namespace ReserGo.WebAPI;

public class Program {
	private const string CORS_POLICY = "CORS_POLICY";
    public static void Main(string[] args) {
        
        var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
		logger.Info("Starting Application ...");

		try {
			var builder = WebApplication.CreateBuilder(args);
			builder.Configuration.AddUserSecrets<Program>();
			var rawConfig = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddEnvironmentVariables()
				.AddJsonFile("appsettings.json")
				.AddUserSecrets<Program>()
				.Build();

			var appSettingsSection = rawConfig.GetSection("AppSettings");
			builder.Services.Configure<AppSettings>(appSettingsSection);
			builder.Services.Configure<AppSettings>(builder.Configuration);


			// Context
			builder.Services.AddTransient<ReserGoContext>();

			// Add services to the container.
			builder.Services.AddScoped<ISecurity, Security>();
          
			builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();
			builder.Services.AddScoped<ILoginDataAccess, LoginDataAccess>();
			builder.Services.AddScoped<IAuthDataAccess, AuthDataAccess>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			
			// Ajouter le service de cache en mémoire
			builder.Services.AddMemoryCache();
			
			// Configure CORS
			builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173", "https://resergo-admin.adjysedar.fr")
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
			
			// Ajout du service d'authentification avec Cookie
			builder.Services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/api/auth/signin"; 
					options.LogoutPath = "/api/auth/logout"; 
					options.Cookie.HttpOnly = true;
					options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					options.Cookie.SameSite = SameSiteMode.None;
					options.Cookie.Domain = "resergo-admin.adjysedar.fr";
				});
			
			builder.Services.AddAuthorization();
			
			// Configuration des cookies pour le support cross-origin
			builder.Services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.None;
				options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
				options.Secure = CookieSecurePolicy.Always;
			});
			
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(opt =>
			{
				opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Health", Version = "v1.0.0" });
				opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});
				opt.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[]{}
					}
				});

				// Ajoutez ici la configuration pour inclure les commentaires XML
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				opt.IncludeXmlComments(xmlPath); 
			});

			
			
			// NLog: Setup NLog for Dependency injection
			builder.Logging.ClearProviders();
			builder.Host.UseNLog();
			
			var jwtSettings = new JwtSettings {
				Key = builder.Configuration.GetSection("Key")?.Get<string>() ?? string.Empty,
				Issuer = builder.Configuration.GetSection("Issuer")?.Value ?? string.Empty,
				Audience = builder.Configuration.GetSection("Audience")?.Get<string>() ?? string.Empty,
				ExpireMinutes = builder.Configuration.GetSection("ExpireMinutes")?.Get<int>() ?? 0,
			};
			
				var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
				builder.Services.AddAuthentication(options =>
					{
						options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
						options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					})
					.AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = true,
							ValidateAudience = true,
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							ValidIssuer = jwtSettings.Issuer,
							ValidAudience = jwtSettings.Audience,
							IssuerSigningKey = new SymmetricSecurityKey(key),
							ClockSkew = TimeSpan.Zero
						};
						
						options.Events = new JwtBearerEvents
						{
							OnMessageReceived = context =>
							{
								if (context.Request.Cookies.ContainsKey("AuthToken"))
								{
									context.Token = context.Request.Cookies["AuthToken"];
								}
								return Task.CompletedTask;
							}
						};
						options.RequireHttpsMetadata = true; // Assurez-vous que HTTPS est utilisé
						options.SaveToken = true; // Sauvegarde le token dans le contexte de la requête
					});
				

			// Configuration pour le service keep-alive
			/*if (builder.Environment.IsProduction())
				builder.Services.AddHostedService<KeepAliveService>();
			*/
			
			
			var app = builder.Build();

			using (var scope = app.Services.CreateScope()) {
				var dbContext = scope.ServiceProvider.GetRequiredService<ReserGoContext>();

				// Here is the migration executed
				dbContext.Database.Migrate();
			} 

			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment()) {
				app.UseSwagger();
				app.UseSwaggerUI();
		//	}

			app.UseHttpsRedirection();
			app.UseCors(CORS_POLICY);
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		} catch (Exception e) {
			logger.Error(e);
			throw;
		} finally {
			// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
			NLog.LogManager.Shutdown();
		}
    }

}

