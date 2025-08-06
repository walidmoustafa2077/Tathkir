using Microsoft.AspNetCore.HttpOverrides;
using Tathkīr_API.Services;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
    }
}
