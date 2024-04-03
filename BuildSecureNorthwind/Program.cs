using BuildSecureNorthwind.Contexts;
using BuildSecureNorthwind.SessionManagement;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace BuildSecureNorthwind;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(opt =>
        {
            opt.AddServerHeader = false;
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews(opt =>
        {
            opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });
        builder.Services.AddScoped<SessionCartManager>();
        builder.Services.AddSession(opt =>
        {
            opt.Cookie.Name = "northwind.session";
        });
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext<NorthwindContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnStr"));
        });

        builder.Services.AddAntiforgery(opt =>
        {
            opt.Cookie.Name = "northwind.antiforgery";
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                opt.Cookie.Name = "northwind.token";
            });

        var app = builder.Build();

        if(builder.Environment.IsProduction())
        {
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        if(builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/home/error");
        }

        app.Use((context, next) =>
        {
            context.Response.Headers.Remove(HeaderNames.XPoweredBy);
            context.Response.Headers.Remove(HeaderNames.Server);
            return next();
        });

        app.UseStaticFiles();

        app.UseSession();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Product}/{action=Index}/{id?}");

        app.Run();
    }
}