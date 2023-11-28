using System;
using System.Text;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public class ViewMessages
    {
        private readonly ILogger<ViewMessages> _logger;
        private readonly Container _cosmosContainer;

        public ViewMessages(ILogger<ViewMessages> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosContainer = cosmosClient.GetContainer("SmartEnhetDatabase", "EventDataContainer");

        }

        [Function(nameof(ViewMessages))]
        public async Task Run([EventHubTrigger("iothub-ehub-kyh-iothub-25230144-fa41a83a62", Connection = "IotHubEndpoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                var data = Encoding.UTF8.GetString(@event.Body.ToArray());

                _logger.LogInformation("Event Body: {data}");

                var cosmosItem = JsonConvert.DeserializeObject<CosmosItem>(data);

                cosmosItem.Id = Guid.NewGuid().ToString();

                // Save the event data to Cosmos DB
                await SaveCosmosItemToCosmosDbAsync(cosmosItem);
            }
        }
        
        private async Task SaveCosmosItemToCosmosDbAsync(CosmosItem cosmosItem)
        {
            try
            {

                _logger.LogInformation($"Id before saving to Cosmos DB: {cosmosItem.Id}");

                // Insert the item into Cosmos DB
                await _cosmosContainer.CreateItemAsync(cosmosItem, new PartitionKey(cosmosItem.Id));
                _logger.LogInformation("Saving CosmosItem to Cosmos DB: {@CosmosItem}", cosmosItem);

            }
            catch (CosmosException cosmosException)
            {
                _logger.LogError($"Cosmos DB Error ({cosmosException.StatusCode}): {cosmosException.Message}\n{cosmosException.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving event data to Cosmos DB: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public class CosmosItem
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            public string Message { get; set; }

            public DateTime Created { get; set; }
        }
    }
}