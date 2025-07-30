using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var usernameDB = builder.AddParameter("usernameDB", secret: true);
var passwordDB = builder.AddParameter("passwordDB", secret: true);
var usernameKEYCLOCK = builder.AddParameter("usernameKEYCLOCK");
var passwordKEYCLOCK = builder.AddParameter("passwordKEYCLOCK", secret: true);
var usernameRabbit = builder.AddParameter("usernameRabbit");
var passwordRabbit = builder.AddParameter("passwordRabbit", secret: true);

var keycloak = builder.AddKeycloak("keycloak", 8080, usernameKEYCLOCK, passwordKEYCLOCK)
    .WithDataVolume()
    .WithRealmImport("realm.json");

var cache = builder.AddRedis("cache");
var rabbitmq = builder.AddRabbitMQ("messaging", usernameRabbit, passwordRabbit);
var postgres = builder.AddPostgres("postgres",usernameDB,passwordDB);
var signalsdb = postgres.AddDatabase("Signals");
var dataManagementdb = postgres.AddDatabase("DataManagement");
var strategiesdb = postgres.AddDatabase("Strategies");

var apiService = builder.AddProject<Projects.BotSinais_ApiService>("apiservice")
    .WithReference(rabbitmq)
    .WithReference(signalsdb)
    .WithReference(dataManagementdb)
    .WithReference(strategiesdb)
    .WithHttpHealthCheck("/health")
    .WithReference(keycloak)
    .WaitFor(keycloak);

builder.AddProject<Projects.BotSinais_Presentation_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(keycloak)
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.BotSinais_Presentation_Web>("botsinais-presentation-web");

builder.Build().Run();
