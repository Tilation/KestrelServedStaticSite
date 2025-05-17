using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Serilog;

namespace KestrelServedStaticSite
{
    public class KestrelServedStaticSiteBuilder
    {
        private WebApplicationBuilder _builder = null!;
        private WebApplication _app = null!;
        private Process? _debugServer = null;

        public KestrelServedStaticSiteBuilder()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("./logs.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();
        }

        public void CreateBuilder(string[] args)
        {
            Log.Information("Creating builder...");
            _builder = WebApplication.CreateBuilder(args);
        }

        public void ConfigureServices()
        {
            Log.Information("Configuring services...");

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

            _builder.Services.AddHostedService<TSGenerator>();
        }

        public void Build()
        {
            Log.Information("Building app...");
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

            if (_app.Environment.IsDevelopment())
            {
                Log.Information("Building development-specific features...");
                // Run NG Serve and route
                /*
                    "DebugServerCommand": "ng serve --host localhost --port 8787 --watch",
                    "DebugServerHost": "localhost",
                    "DebugServerPort": "8787"
                 */
                string command = ksss.GetValue<string>("DebugServerCommand");
                string host = ksss.GetValue<string>("DebugServerHost");
                int port = ksss.GetValue<int>("DebugServerPort");
                string workingDirectory = ksss.GetValue<string>("DebugServerCommandDir");

                if (string.IsNullOrWhiteSpace(command))
                {
                    throw new Exception();
                }

                Log.Information("Starting debug server...");
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {command}",
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory
                };
                _debugServer = Process.Start(startInfo);
                _debugServer.OutputDataReceived += (sender, args) =>
                {
                    Log.Information("{0}: {1}", "DebugServer", args.Data);
                };

                _debugServer.ErrorDataReceived += (sender, args) =>
                {
                    Log.Error("{0}: {1}", "DebugServer", args.Data);
                };
                _app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = ksss["StaticSiteRelativeRootPath"];

                    if (_app.Environment.IsDevelopment())
                    {
                        // Proxy
                        spa.UseProxyToSpaDevelopmentServer($"http://{host}:{port}");
                    }
                });
            }
        }

        public void Run()
        {
            Log.Information("Running...");
            _app.Run();
            if (_debugServer != null)
            {
                Log.Information("Killing debug server...");
                _debugServer.Kill();
            }
            Log.Information("Exited");
        }
    }
}
