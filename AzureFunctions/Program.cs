using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
          .ConfigureFunctionsWorkerDefaults()
          .ConfigureServices(services =>
          {
              services.AddSingleton<CosmosClient>(sp =>
              {
                  var accountEndpoint = "https://smartenhet-erim.documents.azure.com:443/";
                  var accountKey = "h6ezXmJ7l61Dsue575cwACF1yJjK79uCS4cHl9IseyGzcS6pHuxuyVazuO6uPgzHFEUyLO1txYVgACDbpLYrXA==";

                  var cosmosClientBuilder = new CosmosClientBuilder(accountEndpoint, accountKey);
                  // configure other CosmosClient settings here if needed
                  return cosmosClientBuilder.Build();
              });
          })
          .Build();

host.Run();