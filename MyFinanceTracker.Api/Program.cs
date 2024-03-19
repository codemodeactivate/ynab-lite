using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyFinanceTracker.Api.Data;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Authentication.Google;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Add logging
// Configure logging
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Retrieve the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var services = builder.Services;
var configuration = builder.Configuration;

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Optional: adjust the clock skew if necessary
        };
    });

//google auth
// Configure Google authentication
services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    googleOptions.CallbackPath = "/oauthplayground";
});

// This line ensures that authorization can be applied to endpoints
builder.Services.AddAuthorization();

// Register and configure AppDbContext to use MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

// Add support for controllers
builder.Services.AddControllers(); // This line is crucial for your app to recognize controllers
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configure OAuth2 options for Swagger UI
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("/api/Auth/google-auth", UriKind.Relative),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "Read access to your openid" },
                    { "email", "Read your email address" },
                    
                }
            }
        }
    });

    // Configure OAuth2 security requirements for Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new string[] { }
        }
    });

    // Including XML Comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath); // Make sure this line is within the AddSwaggerGen call
});

var app = builder.Build();

// Seed the database with example data
using (var scope = app.Services.CreateScope())
{
    var scopedServices = scope.ServiceProvider;
    var context = scopedServices.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.OAuthClientId(configuration["Authentication:Google:ClientId"]); // Set the OAuth client ID for Swagger UI
        c.OAuthAppName("YNAB-Lite"); // Set the OAuth app name for Swagger UI
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); // This line is crucial for enabling JWT authentication
app.UseAuthorization(); // This line is crucial for enabling authorization

app.MapControllers(); // Ensure this line is present to map attribute-routed controllers

app.Run();
