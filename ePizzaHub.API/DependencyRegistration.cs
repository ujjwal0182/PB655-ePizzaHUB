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
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
            services.AddTransient<IPaymentService, PaymentService>();

            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            //we need to register our DBContext in the service collection, so that we can inject it in the repository layer.
            services.AddDbContext<PB655Context>(x =>
            {
                x.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        public static IServiceCollection RegisterJsonWebToken(this IServiceCollection services, IConfiguration configuration)
        {
            //The first logic just the implement the token and second logic was to implement it.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero, // Set clock skew to zero to prevent token expiration issues
                    };
                });

            return services;
        }
    }
}
