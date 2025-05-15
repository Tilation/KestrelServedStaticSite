using NSwag;
using NSwag.CodeGeneration.TypeScript;

namespace KestrelServedStaticSite
{
    class TSGenerator : BackgroundService
    {
        readonly IConfiguration configuration;

        private readonly IHostApplicationLifetime _lifetime;
        private readonly TaskCompletionSource _source = new();

        public TSGenerator(IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            this.configuration = configuration;
            _lifetime = lifetime;
            _lifetime.ApplicationStarted.Register(() => _source.SetResult());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _source.Task.ConfigureAwait(false); // Wait for the task to complete!

            var document = await OpenApiDocument.FromUrlAsync($"{configuration["ASPNETCORE_URLS"]}/swagger/v1/swagger.json", stoppingToken);

            var settings = new TypeScriptClientGeneratorSettings
            {
                ClassName = "{controller}Client",
            };

            var generator = new TypeScriptClientGenerator(document, settings);
            var code = generator.GenerateFile();
            File.WriteAllText("./generated.ts", code);
        }
   
    }
}
