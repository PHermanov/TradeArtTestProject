using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using SlimMessageBus.Host.AspNetCore;
using SlimMessageBus.Host.Memory;
using TradeArtTestProject.Communication;
using TradeArtTestProject.Communication.MessageConsumers;
using TradeArtTestProject.Communication.Messages;
using TradeArtTestProject.Services;
using TradeArtTestProject.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "TradeArtTestProject.xml");
    c.IncludeXmlComments(filePath);
});

// GraphQL client
builder.Services.AddScoped<IGraphQLClient>(s =>
    new GraphQLHttpClient(builder.Configuration["GraphQlUrl"], new SystemTextJsonSerializer()));

// Message bus
builder.Services.AddHttpContextAccessor(); // This is required for the SlimMessageBus.Host.AspNetCore plugin
builder.Services.AddSlimMessageBus((mbb, svp) =>
    {
        mbb
            .Produce<EmitDataMessage>(x => x.DefaultTopic("emit-topic"))
            .Consume<EmitDataMessage>(x => x.Topic("emit-topic")
                .WithConsumer<EmitDataMessageConsumer>()
                .Instances(100))
            .Produce<ProcessedDataMessage>(x => x.DefaultTopic("processed-topic"))
            .Consume<ProcessedDataMessage>(x => x.Topic("processed-topic")
                .WithConsumer<ProcessedDataMessageConsumer>()
                .Instances(100))
            .WithProviderMemory();
    },
    addConsumersFromAssembly: new[] { typeof(EmitDataMessageConsumer).Assembly }
);

// Services
builder.Services.AddScoped<IShaService, ShaService>();
builder.Services.AddScoped<IPricesService, PricesService>();
builder.Services.AddScoped<IMessageResultsStorage, MessageResultsStorage>();
builder.Services.AddScoped<IProcessDataService, ProcessDataService>();


var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();