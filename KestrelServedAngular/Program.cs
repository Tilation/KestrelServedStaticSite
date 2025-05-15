
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;

namespace KestrelServedAngular
{
    public class Program
    {
        private const string CookieAuthName = "cookieAuth";
        private const string AngularCSRRelativePath = "AngularCSR/csrapp/dist/csrapp/browser";
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {

            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "",
                    Version = "v1"
                });

                options.AddSecurityDefinition(CookieAuthName, new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Name = ".AspNetCore.Cookies",
                    Description = "Authentication Cookie"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme, Id = CookieAuthName
                            }
                        },
                        Array.Empty<String>()
                    }
                });
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = AngularCSRRelativePath;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1"));
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = AngularCSRRelativePath;

                if (app.Environment.IsDevelopment())
                {
                    // Desarrollo con proxy al servidor de desarrollo de Angular
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            app.Run();
        }
    }
}
