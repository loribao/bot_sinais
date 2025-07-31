using BotSignal.RPA.RealTimeQuote;
using MassTransit;
using MassTransit.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

// Substitua o bloco que usa UseSystemTextJsonSerializer por configuração correta do serializer.
// Remova o uso de cfg.UseSystemTextJsonSerializer, pois não existe esse método em IRabbitMqBusFactoryConfigurator.
// Em vez disso, configure o serializer via AddMassTransit e UseSystemTextJson.


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddMassTransit(conf =>
{

    conf.SetKebabCaseEndpointNameFormatter();
    conf.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Environment.GetEnvironmentVariable("ConnectionStrings__messaging"));
        cfg.ConfigureEndpoints(context);
        //cfg.AddDeserializer()
       
    });
    // Configura o contexto de serialização do MassTransit
    conf.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
    });
    conf.AddHostedService<Worker>();
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}




Todo[] sampleTodos =
[
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
];

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos)
        .WithName("GetTodos");

todosApi.MapGet("/{id}", Results<Ok<Todo>, NotFound> (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? TypedResults.Ok(todo)
        : TypedResults.NotFound())
    .WithName("GetTodoById");

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
