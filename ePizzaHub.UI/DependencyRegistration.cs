using ePizzaHub.UI.Helpers;
using ePizzaHub.UI.RazorPay;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ePizzaHub.UI
{
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterEPizzaApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<TokenHandler>();

            services.AddHttpClient("ePizzaAPI", option =>
            {
                option.BaseAddress = new Uri(configuration["EPizzaAPI:Url"]!);
                option.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<TokenHandler>();

            return services;
        }

        public static IServiceCollection AddAuthenticationMechanism(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login";
                    options.LogoutPath = "/Login/Signout";
                });

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IRazorPayService, RazorPayService>();

            return services;
        }
    }
}
