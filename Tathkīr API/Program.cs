using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json;
using Tathkīr_API.Configs;
using Tathkīr_API.Services;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Bind ConfigsSettings from appsettings.json
            builder.Services.Configure<ConfigsSettings>(
                builder.Configuration.GetSection("Configs"));

            // 2. Read ConfigFilePath from ConfigsSettings
            var configSettings = builder.Configuration
                .GetSection("Configs")
                .Get<ConfigsSettings>();

            if (configSettings is null || string.IsNullOrWhiteSpace(configSettings.ConfigFilePath))
                throw new InvalidOperationException("Missing ConfigFilePath in appsettings.json");

            // 3. Load the external EnvironmentConfig from the file path
            var envConfig = LoadEnvironmentConfig(configSettings.ConfigFilePath);

            // 4. Register EnvironmentConfig as a singleton in DI
            builder.Services.AddSingleton(envConfig);

            // Add services to the container.
            builder.Services.AddHttpClient(); // For HttpClient injection

            // Register translation and keyword processing
            builder.Services.AddSingleton<IKeywordProcessor, KeywordProcessor>();
            builder.Services.AddHttpClient<ITranslationService, GoogleTranslationService>();

            // Register sub-services
            builder.Services.AddHttpClient<IGeoLocationService, GeoLocationService>();
            builder.Services.AddHttpClient<ICountryProvider, CountryProvider>();
            builder.Services.AddHttpClient<ICityProvider, CityProvider>();

            // Register your orchestrator
            builder.Services.AddScoped<ICountryService, CountryService>();

            builder.Services.AddHttpClient<IPrayerTimesService, PrayerTimesService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static EnvironmentConfig LoadEnvironmentConfig(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Environment config file not found.", path);

            var json = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<EnvironmentConfig>(json);

            if (config == null || string.IsNullOrWhiteSpace(config.GoogleTranslationAPI))
                throw new InvalidOperationException("Invalid or missing GoogleTranslationAPI in environment config.");

            return config;
        }
    }
}
