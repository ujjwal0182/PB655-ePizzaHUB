
using ePizzaHub.UI.Helpers;
using ePizzaHub.UI.RazorPay;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ePizzaHub.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login";
                    options.LogoutPath = "/Login/Signout";
                });

            builder.Services.AddAuthorization();

            //This way my APIs are configured on the service injection layer, How can you get this in the controller ?

            builder.Services.AddHttpContextAccessor(); ///It is a method to register the dependency injection of services inside this DI container.

            builder.Services.AddTransient<TokenHandler>(); //This is a custom handler to add the token in the header of every API call, so we don't have to worry about adding the token in every API call in the controller.

            builder.Services.AddHttpClient("ePizzaAPI", option =>
            {
                option.BaseAddress = new Uri(builder.Configuration["EPizzaAPI:Url"]!);
                option.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<TokenHandler>();

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IRazorPayService, RazorPayService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
