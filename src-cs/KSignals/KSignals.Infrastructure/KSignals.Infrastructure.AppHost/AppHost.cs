using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var usernameDB = builder.AddParameter("usernameDB", secret: true);
var passwordDB = builder.AddParameter("passwordDB", secret: true);
var usernameKEYCLOCK = builder.AddParameter("usernameKEYCLOCK");
var passwordKEYCLOCK = builder.AddParameter("passwordKEYCLOCK", secret: true);
var usernameRabbit = builder.AddParameter("usernameRabbit");
var passwordRabbit = builder.AddParameter("passwordRabbit", secret: true);

var keycloak = builder.AddKeycloak("keycloak", 8080, usernameKEYCLOCK, passwordKEYCLOCK)
    .WithDataVolume();
   // .WithRealmImport("realm.json");

var cache = builder.AddRedis("cache");
var rabbitmq = builder.AddRabbitMQ("messaging", usernameRabbit, passwordRabbit).WithManagementPlugin().WithDataVolume(isReadOnly: false);
var postgres = builder.AddPostgres("postgres", usernameDB, passwordDB);
var signalsdb = postgres.AddDatabase("Signals");
var dataManagementdb = postgres.AddDatabase("DataManagement");
var strategiesdb = postgres.AddDatabase("Strategies");

var apiService = builder.AddProject<Projects.KSignals_Infrastructure_ApiService>("apiservice");
    //.WithReference(rabbitmq)
    //.WithReference(signalsdb)
    //.WithReference(dataManagementdb)
    //.WithReference(strategiesdb)
    //.WithHttpHealthCheck("/health")
    //.WithReference(keycloak)
    //.WaitFor(keycloak);


var web = builder.AddProject<Projects.KSignals_Infrastructure_Web>("ksignals-infrastructure-web");
var maui_web = builder.AddProject<Projects.KSignals_Presentation_Web>("ksignals-presentation-web");
//var maui = builder.AddProject<Projects.KSignals_Presentation>("ksignals-presentation").WithReference(maui_web);
builder.Build().Run();
