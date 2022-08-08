using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradeArtTestProject.Services;
using TradeArtTestProject.Services.Interfaces;

namespace TradeArtTestProject.Tests;

public class Startup
{
    private readonly IConfiguration _config;

    public Startup()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Tests.json");

        _config = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IGraphQLClient>(s =>
            new GraphQLHttpClient(_config["GraphQlUrl"], new SystemTextJsonSerializer()));

        services.AddScoped<IShaService, ShaService>();
        services.AddScoped<IPricesService, PricesService>();
    }
}
