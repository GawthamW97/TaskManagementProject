using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Class;
using TaskManagementApp.Data;
using TaskManagementApp.Mapper;
using TaskManagementApp.Repository;
using TaskManagementApp.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Serilog;
using TaskManagementApp.Middleware;
using Azure.Identity;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    // Also add specific policy for local development
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins(
                "https://localhost:5001",
                "https://localhost:5000",
                "http://localhost:5001",
                "http://localhost:5000",
                "http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddHttpContextAccessor();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Task Management API",
        Description = "API to manage Tasks"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

builder.Services.AddDbContext<TaskManagementDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("TaskManagementConnString");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("TaskManagementConnString connection string is missing from configuration!");
    }

    // In production (Azure), use Managed Identity to get access token
    if (!builder.Environment.IsDevelopment())
    {
        try
        {
            var credential = new DefaultAzureCredential();
            var token = credential.GetTokenAsync(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }),
                CancellationToken.None
            ).GetAwaiter().GetResult();

            var connection = new SqlConnection(connectionString)
            {
                AccessToken = token.Token
            };

            options.UseSqlServer(connection, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                );
            });
        }
        catch (Exception ex)
        {
            // Fallback to connection string if token acquisition fails
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                );
            });
        }
    }
    else
    {
        // Development: use connection string directly
        options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        });
    }

    // Suppress the pending changes warning so app can start even with unapplied migrations
    options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageUploadRepository, ImageUploadRepository>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("TaskManagement")
    .AddEntityFrameworkStores<TaskManagementDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
var app = builder.Build();

// Apply pending migrations at startup - but don't block startup if database is unavailable
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();
        var migrationLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        migrationLogger.LogInformation("Starting database migration check...");
        await dbContext.Database.MigrateAsync();
        migrationLogger.LogInformation("Migrations applied successfully");
    }
    catch (Exception ex)
    {
        var errorLogger = app.Services.GetRequiredService<ILogger<Program>>();
        errorLogger.LogWarning(ex, "Database migration skipped - database may be unavailable. Error: {Message}", ex.Message);
        // Migrations will be retried on next request if database becomes available
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management v1");
    });
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowLocalhost");

app.UseAuthentication();

app.UseAuthorization();

// Configure static file serving for Images directory (if it exists)
var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
if (Directory.Exists(imagesPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(imagesPath),
        RequestPath = "/Images"
    });
}

app.MapControllers();

app.Run();
