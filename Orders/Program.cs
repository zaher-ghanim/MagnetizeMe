using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrdersAPI.Services;
using RepositoryAPI;
using System.Security.Claims;
using System.Text;
using Users.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });

            // Map List<IFormFile> to an array of binary files
            c.MapType<List<IFormFile>>(() => new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                }
            });

            // Map IFormFile to binary
            c.MapType<IFormFile>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });

            // Added by zaher
            // Add JWT Authentication to Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
        });


        builder.Services.AddDbContext<EcommerceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        IServiceCollection serviceCollection = builder.Services.AddScoped<IOrdersService, OrderService>();

        // Added by zaher
        // Configure JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

        // add a policy for admin-only access:
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
        });

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
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                //NameClaimType = JwtRegisteredClaimNames.Sub // Map the Sub claim to the Name property
            };
        });

        // Add authorization policies (if needed)
        builder.Services.AddAuthorization();




        var app = builder.Build();

        app.UseStaticFiles();
        //to ensure static file custom.js is servd

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test My Order API");
                c.InjectJavascript("/swagger-ui/custom.js?ver=1");  // Custom JS to enable multi-file selection
            });
        }


        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();


        app.Run();
    }

    //public static IHostBuilder CreateHostBuilder(string[] args) =>
    //   Host.CreateDefaultBuilder(args)
    //       .ConfigureWebHostDefaults(webBuilder =>
    //       {
    //           webBuilder.ConfigureServices(services =>
    //           {
    //               services.AddSwaggerGen(c =>
    //               {
    //                   c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    //               });
    //           });

    //           webBuilder.Configure(app =>
    //           {
    //               app.UseSwagger();
    //               app.UseSwaggerUI(c =>
    //               {
    //                   // Configure Swagger UI options here
    //                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //                   c.InjectJavascript("/swagger-ui/custom.js");
    //               });
    //           });
    //       });

}