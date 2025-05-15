using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;

namespace KestrelServedAngular
{
    public class KestrelServedStaticSiteBuilder
    {
        private WebApplicationBuilder _builder = null!;
        private WebApplication _app = null!;

        public void CreateBuilder(string[] args)
        {
            _builder = WebApplication.CreateBuilder(args);
        }

        public void ConfigureServices()
        {
            IConfigurationSection ksss = _builder.Configuration.GetSection("KestrelServedStaticSite");
            // Add services to the container.
            _builder.Services.AddControllers(options =>
            {

            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _builder.Services.AddEndpointsApiExplorer();
            _builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "",
                    Version = "v1"
                });

                options.AddSecurityDefinition(ksss["CookieAuthName"], new OpenApiSecurityScheme()
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
                                Type = ReferenceType.SecurityScheme, Id = ksss["CookieAuthName"]
                            }
                        },
                        Array.Empty<String>()
                    }
                });
            });

            _builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
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

            _builder.Services.AddAuthorization();

            _builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = ksss["StaticSiteRelativeRootPath"];
            });
        }

        public void Build()
        {
            _app = _builder.Build();
            var ksss = _app.Configuration.GetSection("KestrelServedStaticSite");

            if (_app.Environment.IsDevelopment())
            {
                _app.UseSwagger();
                _app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1"));
            }
            else
            {
                _app.UseHsts();
            }

            //app.UseHttpsRedirection();
            _app.UseStaticFiles();
            _app.UseSpaStaticFiles();
            _app.UseRouting();
            _app.UseAuthentication();
            _app.UseAuthorization();

            _app.MapControllers();

            _app.UseSpa(spa =>
            {
                spa.Options.SourcePath = ksss["StaticSiteRelativeRootPath"];

                if (_app.Environment.IsDevelopment())
                {
                    // Desarrollo con proxy al servidor de desarrollo de Angular
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }

        public void Run()
        {
            _app.Run();
        }
    }
}
