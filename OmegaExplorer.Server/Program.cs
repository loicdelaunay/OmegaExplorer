using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OmegaExplorer.Client.Hubs.Events;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Game.Cycles.Hubs;
using OmegaExplorer.Server.Services._Game.Notifications;
using OmegaExplorer.Server.Services._Game.SpatialObjects;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Jwt.Middlewares;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Mappers;
using OmegaExplorer.Server.Services.ProjectFiles;
using OmegaExplorer.Server.Services.Servers.Models.Classes;
using OmegaExplorer.Server.Services.Servers.Models.Enums;
using OmegaExplorer.Server.Services.SignalR;
using OmegaExplorer.Server.Services.SignalR.Providers;
using OmegaExplorer.Server.Services.Volumes;
using OmegaExplorer.Shared.Enums;
using OmegaExplorer.Shared.Utilities.Random;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.OpenTelemetry;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Text.Json.Serialization;

namespace OmegaExplorer.Server;

/// <summary>
/// </summary>
public static class Program
{
    public const string API_VERSION = "v1";

    public static string? CompleteApiURL { get; private set; }

    public static string CompleteClientURL => ConfigurationManager.Configuration.Client.AddressClient;

    public static EnumStartMode StartMode { get; private set; } = EnumStartMode.Development;

    public static bool StartFromAspire { get; private set; } = false;

    /// <summary>
    ///     Main method and entry point
    /// </summary>
    /// <param name="args"> </param>
    public static async Task Main(string[] args)
    {
        Console.WriteLine(value: @"" +
                                 @"________                              ___________              .__                              
\_____  \   _____   ____   _________  \_   _____/__  _________ |  |   ___________   ___________ 
 /   |   \ /     \_/ __ \ / ___\__  \  |    __)_\  \/  /\____ \|  |  /  _ \_  __ \_/ __ \_  __ \
/    |    \  Y Y  \  ___// /_/  > __ \_|        \>    < |  |_> >  |_(  <_> )  | \/\  ___/|  | \/
\_______  /__|_|  /\___  >___  (____  /_______  /__/\_ \|   __/|____/\____/|__|    \___  >__|   
        \/      \/     \/_____/     \/        \/      \/|__|                           \/       ");

        try
        {
            PrepareLogger();

            Console.WriteLine(value: $"Server is starting with args {args.ToStringSplit()}...");

            await PreStart();

            if (args.Contains("nswag"))
            {
                StartMode = EnumStartMode.NSwag;
                await StartAsNSwag(args);
            }
            else
            {
                await Start(args);
            }
        }
        catch (Exception e)
        {
            Log.Fatal(e, "The application failed at start -ups");
        }
        finally
        {
            await Task.WhenAny(
                Log.CloseAndFlushAsync().AsTask(),
                Task.Delay(TimeSpan.FromSeconds(10)));
        }
    }

    private static async Task PreStart()
    {
        ConfigurationManager.Initialize();
    }

    private static async Task Start(string[] args)
    {
        Log.Logger.Information("Starting...");

        //Initialize managers


        RandomWithSeed.Initialize(ConfigurationManager.Configuration.Server.Seed);

        var webApplicationBuilder = WebApplication
            .CreateBuilder(args);

        webApplicationBuilder.Host.UseSerilog();

        // Add services to the container.
        webApplicationBuilder.AddServiceDefaults();

        // Useful !
        webApplicationBuilder.Services.AddScoped<SpacialObjectRepository>();

        webApplicationBuilder.Services.AddSingleton<IUserIdProvider, UserIdFromJwtProvider>();

        if (StartFromAspire)
        {
            PrepareTelemetry(webApplicationBuilder);
        }

        ConfigureServices(webApplicationBuilder);

        //PrepareUrls();

        var app = webApplicationBuilder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await db.Database.MigrateAsync();
        }

        ConfigureApplication(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseStaticFiles();

        await app.RunAsync();
        await app.WaitForShutdownAsync();
    }

    private static async Task StartAsNSwag(string[] args)
    {
        Log.Logger.Information("Starting as nswag...");

        var webApplicationBuilder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        webApplicationBuilder.AddServiceDefaults();

        ConfigureServicesAsNswag(webApplicationBuilder);

        //PrepareUrls();

        var app = webApplicationBuilder.Build();

        ConfigureApplication(app);

        app.UseSwagger();
        app.UseHttpsRedirection();

        await app.StartAsync();
        await app.WaitForShutdownAsync(new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);
    }

    private static void PrepareTelemetry(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
               .WithTracing(tracerProviderBuilder =>
               {
                   tracerProviderBuilder
                       .AddAspNetCoreInstrumentation(options => // <-- Keep to automatically enrich
                       {
                           options.EnableAspNetCoreSignalRSupport = true;
                           options.RecordException = true;
                       })
                       .AddEntityFrameworkCoreInstrumentation(options => // EF Core calls
                       {
                           options.SetDbStatementForText = true; // include SQL in span
                       });
               })
               .WithMetrics(metricProviderBuilder =>
               {
                   metricProviderBuilder.AddAspNetCoreInstrumentation();
                   metricProviderBuilder.AddRuntimeInstrumentation();
                   metricProviderBuilder.AddHttpClientInstrumentation();
               })
               .WithLogging(loggingBuilder => { });
    }

    private static void PrepareLogger()
    {
        Serilog.Debugging.SelfLog.Enable(Console.Error);

        // Initial Serilog Initial Configuration
        var loggerConfiguration = new LoggerConfiguration()
                                                  .MinimumLevel.Information() // Minimum level of journalization
                                                  .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day); // Write in a file with daily rotation

        if (StartFromAspire)
        {
            loggerConfiguration.Enrich.WithProperty("IsSuccessful", false) // Default value for IsSuccessful
                               .Enrich.WithProperty("service.name", "OmegaExplorer.Server")
                               .Enrich.FromLogContext()

                               .WriteTo.Console(new RenderedCompactJsonFormatter())
                               .WriteTo.SQLite($"{VolumeManager.GetLogDatabaseFile().FullName}", tableName: "Logs", LogEventLevel.Information)
                               .WriteTo.OpenTelemetry(otlpOptions =>
                               {
                                   otlpOptions.Endpoint = new Uri("http://localhost:4317").ToString(); // default OTel port
                                   otlpOptions.Protocol = OtlpProtocol.Grpc;

                                   // optional: add resource attributes
                                   otlpOptions.ResourceAttributes = new Dictionary<string, object>
                                   {
                                       ["service.namespace"] = "OmegaExplorer",
                                       ["service.instance.id"] = Environment.MachineName
                                   };
                               });
        }
        else
        {
            loggerConfiguration.WriteTo.Console(new CustomColoredFormatter());
        }

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    private static void ConfigureServices(WebApplicationBuilder webApplicationBuilder)
    {
        Log.Logger.Information("Configuring services...");

        ProgramService.ConfigureDatabaseServices(webApplicationBuilder);

        ProgramService.ConfigureRepositories(webApplicationBuilder);

        ProgramService.ConfigureServicesFeatures(webApplicationBuilder);

        ProgramService.ConfigureServicesGameFeatures(webApplicationBuilder);

        #region MAPPER

        Log.Logger.Information("Configuring AutoMapper services...");
        webApplicationBuilder.Services.AddAutoMapper(typeof(OmegaExplorerMapperProfile).Assembly);
        Log.Logger.Success("AutoMapper configured", EnumLogSeverity.Information);

        #endregion

        #region CONTROLLERS

        Log.Logger.Information("Configuring controllers services...");

        webApplicationBuilder.Services.AddControllersWithViews()
                             .AddJsonOptions(options =>
                             {
                                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                             });

        Log.Logger.Success($"Controllers configured");

        #endregion

        #region SWAGGER

        Log.Logger.Information($"Configuring Swagger services...");

        webApplicationBuilder.Services.AddSwaggerGen(options =>
        {
            options.MapType<FileContentResult>(() => new OpenApiSchema()
            {
                Type = "file"
            });

            options.SwaggerDoc(API_VERSION, new OpenApiInfo
            {
                Title = "OmegaExplorer API",
                Version = API_VERSION,
                Description = "API pour OmegaExplorer"
            });

            OpenApiSecurityScheme jwtSecurityScheme = new()
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.UseAllOfToExtendReferenceSchemas();

            options.UseAllOfForInheritance();

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {jwtSecurityScheme, Array.Empty<string>()}
            });

            options.AddSignalRSwaggerGen();
        });

        Log.Logger.Success($"Swagger configured");

        #endregion

        #region CORS

        Log.Logger.Information($"Configuring CORS services...");
        webApplicationBuilder.Services.AddCors();
        Log.Logger.Success($"Cores configured");

        #endregion

        #region SIGNALR

        Log.Logger.Information($"Configuring SignalR services...");

        webApplicationBuilder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; }).AddJsonProtocol();

        Log.Logger.Success($"SignalR configured");

        #endregion

        #region ROUTING

        Log.Logger.Information($"Configuring routing services...");
        webApplicationBuilder.Services.AddRouting(options => { options.LowercaseUrls = true; });
        Log.Logger.Success($"Routing configured");

        #endregion

        #region RATE LIMITER

        Log.Logger.Information($"Configuring rate limiter services...");
        // Configure Rate Limiting
        webApplicationBuilder.Services.AddRateLimiter(rateLimiterOptions => rateLimiterOptions
                                                          .AddFixedWindowLimiter("fixed", fixedWindowRateLimiterOptions =>
                                                          {
                                                              fixedWindowRateLimiterOptions.PermitLimit = 100;
                                                              fixedWindowRateLimiterOptions.Window = TimeSpan.FromSeconds(value: 10);
                                                              fixedWindowRateLimiterOptions.QueueLimit = 2;
                                                          }));
        Log.Logger.Information("Rate limiter configured");

        #endregion

        #region AUTHENTICATION

        Log.Logger.Information("Configuring authentication services...");
        webApplicationBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                             .AddJwtBearer(options =>
                             {
                                 options.SaveToken = true;

                                 options.Events = new JwtBearerEvents
                                 {
                                     OnMessageReceived = context =>
                                     {
                                         var accessToken = context.Request.Query["access_token"];

                                         if (!SignalRUtility.IsRequestFromHub(context, accessToken))
                                         {
                                             return Task.CompletedTask;
                                         }

                                         context.Token = accessToken;

                                         return Task.CompletedTask;
                                     },

                                     OnAuthenticationFailed = (context) =>
                                     {
                                         context.Response.OnStarting(async () => { await context.Response.WriteAsJsonAsync(new HttpResponseException("JwtToken expired", (int)EnumCustomStatusCode.JwtTokenExpired)); });

                                         return Task.CompletedTask;
                                     },
                                 };

                                 options.TokenValidationParameters = new TokenValidationParameters
                                 {
                                     RequireExpirationTime = true,
                                     ClockSkew = TimeSpan.FromSeconds(ConfigurationManager.Configuration.Jwt.DurationInSeconds),
                                     ValidateIssuer = true,
                                     ValidateAudience = false,
                                     ValidateLifetime = true,
                                     ValidateIssuerSigningKey = true,
                                     ValidIssuer = ConfigurationManager.Configuration.Jwt.Issuer,
                                     ValidAudience = ConfigurationManager.Configuration.Jwt.Issuer,
                                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.Configuration.Jwt.Key))
                                 };
                             });

        Log.Logger.Success("Auth configured", EnumLogSeverity.Information);

        #endregion

        Log.Logger.Success("Services configured !", EnumLogSeverity.Information);
    }

    private static void ConfigureServicesAsNswag(WebApplicationBuilder webApplicationBuilder)
    {
        Log.Logger.Information("Configuring services...");

        #region CONTROLLERS

        Log.Logger.Information("Configuring controllers services...");

        webApplicationBuilder.Services.AddControllersWithViews()
                             .AddJsonOptions(options =>
                             {
                                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                             });

        Log.Logger.Success($"Controllers configured");

        #endregion

        #region SWAGGER

        Log.Logger.Information($"Configuring Swagger services...");

        webApplicationBuilder.Services.AddSwaggerGen(options =>
        {
            options.MapType<FileContentResult>(() => new OpenApiSchema()
            {
                Type = "file"
            });

            options.SwaggerDoc(API_VERSION, new OpenApiInfo
            {
                Title = "OmegaExplorer API",
                Version = API_VERSION,
                Description = "API pour OmegaExplorer"
            });

            OpenApiSecurityScheme jwtSecurityScheme = new()
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.UseAllOfToExtendReferenceSchemas();

            options.UseAllOfForInheritance();

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {jwtSecurityScheme, Array.Empty<string>()}
            });

            options.AddSignalRSwaggerGen();
        });

        Log.Logger.Success($"Swagger configured");

        #endregion

        #region CORS

        Log.Logger.Information($"Configuring CORS services...");
        webApplicationBuilder.Services.AddCors();
        Log.Logger.Success($"Cores configured");

        // Set logger 
        webApplicationBuilder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

        #endregion

        #region SIGNALR

        Log.Logger.Information($"Configuring SignalR services...");

        webApplicationBuilder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; }).AddJsonProtocol();

        Log.Logger.Success($"SignalR configured");

        #endregion

        #region ROUTING

        Log.Logger.Information($"Configuring routing services...");
        webApplicationBuilder.Services.AddRouting(options => { options.LowercaseUrls = true; });
        Log.Logger.Success($"Routing configured");

        #endregion

        #region RATE LIMITER

        Log.Logger.Information($"Configuring rate limiter services...");
        // Configure Rate Limiting
        webApplicationBuilder.Services.AddRateLimiter(rateLimiterOptions => rateLimiterOptions
                                                          .AddFixedWindowLimiter("fixed", fixedWindowRateLimiterOptions =>
                                                          {
                                                              fixedWindowRateLimiterOptions.PermitLimit = 100;
                                                              fixedWindowRateLimiterOptions.Window = TimeSpan.FromSeconds(value: 10);
                                                              fixedWindowRateLimiterOptions.QueueLimit = 2;
                                                          }));
        Log.Logger.Information("Rate limiter configured");

        #endregion

        #region AUTHENTICATION

        Log.Logger.Information("Configuring authentication services...");
        webApplicationBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                             .AddJwtBearer(options =>
                             {
                                 options.SaveToken = true;

                                 options.Events = new JwtBearerEvents
                                 {
                                     OnMessageReceived = context =>
                                     {
                                         var accessToken = context.Request.Query["access_token"];

                                         if (!SignalRUtility.IsRequestFromHub(context, accessToken))
                                         {
                                             return Task.CompletedTask;
                                         }

                                         context.Token = accessToken;

                                         return Task.CompletedTask;
                                     },

                                     OnAuthenticationFailed = (context) =>
                                     {
                                         context.Response.OnStarting(async () => { await context.Response.WriteAsJsonAsync(new HttpResponseException("JwtToken expired", (int)EnumCustomStatusCode.JwtTokenExpired)); });

                                         return Task.CompletedTask;
                                     },
                                 };

                                 options.TokenValidationParameters = new TokenValidationParameters
                                 {
                                     RequireExpirationTime = true,
                                     ClockSkew = TimeSpan.FromSeconds(ConfigurationManager.Configuration.Jwt.DurationInSeconds),
                                     ValidateIssuer = true,
                                     ValidateAudience = false,
                                     ValidateLifetime = true,
                                     ValidateIssuerSigningKey = true,
                                     ValidIssuer = ConfigurationManager.Configuration.Jwt.Issuer,
                                     ValidAudience = ConfigurationManager.Configuration.Jwt.Issuer,
                                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.Configuration.Jwt.Key))
                                 };
                             });

        Log.Logger.Success("Auth configured", EnumLogSeverity.Information);

        #endregion

        Log.Logger.Success("Services configured !", EnumLogSeverity.Information);
    }

    private static void ConfigureApplication(WebApplication application)
    {
        application.UseCors(configurePolicy => configurePolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        if (StartMode != EnumStartMode.NSwag)
        {
            application.UseSerilogRequestLogging();
        }

        application.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next();
        });

        application.UseAuthentication();
        application.UseRateLimiter();

        if (application.Environment.IsDevelopment())
        {
            application.UseDeveloperExceptionPage();
            application.UseSwagger();
            application.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.DefaultModelRendering(ModelRendering.Example);
                c.EnableTryItOutByDefault();
                c.SwaggerEndpoint($"/swagger/{API_VERSION}/swagger.json", "OmegaExplorer " + API_VERSION);
            });
            application.UseReDoc(c => { c.RoutePrefix = "doc"; });
        }
        application.UseRouting();

        // Add a static file provider for the static directory
        application.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(FileProjectManager.GetStaticDirectory("").FullName),
            RequestPath = "/static"
        });


        // Add a static file provider for the Views/Resources directory
        DirectoryInfo viewsPath = new(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Resources"));
        viewsPath.CreateIfNotExist();
        application.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(viewsPath.FullName),
            RequestPath = "/resources"
        });

        application.UseMiddleware<JwtMiddleware>();
        application.UseAuthorization();

        //Endpoints
        application.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        application.UseEndpoints(endpoints => { endpoints.MapHub<CycleHub>(CycleHubInformation.URI_HUB); });
        application.UseEndpoints(endpoints => { endpoints.MapHub<CycleProcessHub>(CycleProcessHubInformation.URI_HUB); });
        application.UseEndpoints(endpoints => { endpoints.MapHub<NotificationHub>(NotificationHubInformation.URI_HUB); });

        // Health checks endpoint
        application.UseEndpoints(endpoints =>
        {
            // Home page Home (MVC ou Razor pages)
            endpoints.MapControllerRoute(
                                         name: "default",
                                         pattern: "{controller=HomeView}/{action=Index}/{id?}");


            endpoints.MapControllers();
        });
    }
}
