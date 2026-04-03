
using ePizzaHub.API.Middleware;
using ePizzaHub.Core.Concrete;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Repositories.Concrete;
using ePizzaHub.Repositories.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ePizzaHub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Scans all assemblies in the application and automatically registers
            // all classes that inherit from AutoMapper Profile (e.g., UserMappingProfile)
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //The first logic just the implement the token and second logic was to implement it.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero, // Set clock skew to zero to prevent token expiration issues
                    };
                });

            //we need to register our DBContext in the service collection, so that we can inject it in the repository layer.
            builder.Services.AddDbContext<PB655Context>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IItemService, ItemService>();
            builder.Services.AddTransient<ICartService, CartService>();
            builder.Services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<CommonResponseMiddleware>();
            //app.UseMiddleware<SecondMiddleware>(); //just to check the order of execution of middlewares. It will execute after CommonResponseMiddleware as it is added after that.

            app.MapControllers();

            app.Run();
        }
    }
}
