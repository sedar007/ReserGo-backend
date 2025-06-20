using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using ReserGo.Business.Implementations;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Security;
using ReserGo.DataAccess;
using ReserGo.DataAccess.Implementations;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Implementations;
using ReserGo.Shared.Interfaces;
using ReserGo.Tiers.Implementations;
using ReserGo.Tiers.Interfaces;
using ReserGo.Tiers.Models;
using ReserGo.WebAPI.Hubs;
using ReserGo.WebAPI.Services;

namespace ReserGo.WebAPI;

public class Program {
    public static void Main(string[] args) {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
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
            // Shared
            builder.Services.AddScoped<ISecurity, Security>();

            // Tiers
            builder.Services.AddSingleton<CloudinaryModel>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

            // France Gouv
            builder.Services.AddHttpClient<IFranceGouvApiService, FranceGouvApiService>();
            builder.Services.AddScoped<IFranceGouvService, FranceGouvService>();

            // User
            builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Login
            builder.Services.AddScoped<ILoginDataAccess, LoginDataAccess>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            builder.Services.AddScoped<IGoogleService, GoogleService>();

            // Hotel
            builder.Services.AddScoped<IHotelDataAccess, HotelDataAccess>();
            builder.Services.AddScoped<IHotelService, HotelService>();

            // Room
            builder.Services.AddScoped<IRoomDataAccess, RoomDataAccess>();
            builder.Services.AddScoped<IRoomService, RoomService>();

            // Room Availability
            builder.Services.AddScoped<IRoomAvailabilityDataAccess, RoomAvailabilityDataAccess>();
            builder.Services.AddScoped<IRoomAvailabilityService, RoomAvailabilityService>();

            // Restaurant
            builder.Services.AddScoped<IRestaurantDataAccess, RestaurantDataAccess>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();

            // Event
            builder.Services.AddScoped<IEventDataAccess, EventDataAccess>();
            builder.Services.AddScoped<IEventService, EventService>();

            //Image
            builder.Services.AddScoped<IImageService, ImageService>();

            // Hotel Offer
            builder.Services.AddScoped<IHotelOfferDataAccess, HotelOfferDataAccess>();
            builder.Services.AddScoped<IHotelOfferService, HotelOfferService>();

            // Restaurant Offer
            builder.Services.AddScoped<IRestaurantOfferDataAccess, RestaurantOfferDataAccess>();
            builder.Services.AddScoped<IRestaurantOfferService, RestaurantOfferService>();

            // Event Offer
            builder.Services.AddScoped<IEventOfferDataAccess, EventOfferDataAccess>();
            builder.Services.AddScoped<IEventOfferService, EventOfferService>();

            // Notification
            builder.Services.AddScoped<INotificationDataAccess, NotificationDataAccess>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            // Booking Hotel
            builder.Services.AddScoped<IBookingHotelDataAccess, BookingHotelDataAccess>();
            builder.Services.AddScoped<IBookingHotelService, BookingHotelService>();

            // Booking Restaurant
            builder.Services.AddScoped<IBookingRestaurantDataAccess, BookingRestaurantDataAccess>();
            builder.Services.AddScoped<IBookingRestaurantService, BookingRestaurantService>();

            // Booking Event
            builder.Services.AddScoped<IBookingEventDataAccess, BookingEventDataAccess>();
            builder.Services.AddScoped<IBookingEventService, BookingEventService>();

            // Booking
            builder.Services.AddScoped<IBookingService, BookingService>();

            // Metrics
            builder.Services.AddScoped<IMetricsService, MetricsService>();

            // Add services to the cache memory.
            builder.Services.AddMemoryCache();

            builder.Services.AddSignalR();

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(Consts.CorsPolicy,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173", "http://localhost:4173","http://localhost:5174",
                                "https://resergo-admin.adjysedar.fr",
                                "resergo-admin.adjysedar.fr", "adjysedar.fr", "https://resergo.adjysedar.fr",
                                "resergo.adjysedar.fr")
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = Consts.ApplicationName, Version = "v1.0.0" });
                // Add configuration here to include XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });


            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            var jwtSettings = new JwtSettings {
                Key = builder.Configuration.GetSection(Consts.Key)?.Get<string>() ?? string.Empty,
                Issuer = builder.Configuration.GetSection(Consts.Issuer)?.Value ?? string.Empty,
                Audience = builder.Configuration.GetSection(Consts.Audience)?.Get<string>() ?? string.Empty,
                ExpireMinutes = builder.Configuration.GetSection(Consts.ExpireMinutes)?.Get<int>() ?? 0
            };

            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = context =>
                        {
                            // Check for token in cookies
                            if (context.Request.Cookies.ContainsKey(Consts.AuthToken))
                                context.Token = context.Request.Cookies[Consts.AuthToken];

                            // Check for token in SignalR query string
                            var accessToken = context.Request.Query[Consts.AuthToken];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments(Consts.NotificationHubPath))
                                context.Token = accessToken;

                            return Task.CompletedTask;
                        }
                    };
                });


            // Configuration for keep-alive service
            if (builder.Environment.IsProduction())
                builder.Services.AddHostedService<KeepAliveService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope()) {
                var dbContext = scope.ServiceProvider.GetRequiredService<ReserGoContext>();

                // Here is the migration executed
                dbContext.Database.Migrate();
            }

            // Configure https 
            if (app.Environment.IsProduction())
                // Active les headers proxy pour détecter que Render utilise HTTPS
                app.UseForwardedHeaders(new ForwardedHeadersOptions {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseCors(Consts.CorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<NotificationHub>(Consts.NotificationHubPath);
            app.MapControllers();
            app.Run();
        }
        catch (Exception e) {
            logger.Error(e.Message);
            throw;
        }
        finally {
            LogManager.Shutdown();
        }
    }
}