using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventPublish
{
 public   class EventPublishMethod
    {
        public static async Task<object> GetEventSourceAsync()
        {
            List<object> eventsource = new List<object>();
            eventsource.Add(new Cosmos<dynamic>()
            {
                Data = "CosmosEventFeed",
                RandomId = System.Guid.NewGuid().ToString(),
                Topic= "CosmosDBTopic",
                Subject="CosmosDBEventSource"

            });
            eventsource.Add(new LogicApps<dynamic>()
            {
                Subject = "LogicAppsEventSource",
                Data = "LogicAppsEventFeed",
                RandomId = System.Guid.NewGuid().ToString(),
                Topic = "LogicAppsTopic"

            });
            eventsource.Add(new ExternalWebhook<dynamic>()
            {
                Subject = "ExternalWebhookEventSource",
                Data = "ExternalWebhookEventFeed",
                RandomId = System.Guid.NewGuid().ToString(),
                Topic = "WebhookTopic"

            });
            eventsource.Add(new EventHub<dynamic>()
            {
              Subject = "EventHubEventSource",
                Data = "EventHubEventFeed",
                RandomId = System.Guid.NewGuid().ToString(),
                Topic = "EventHubTopic"

            });
            return  await Task.FromResult(eventsource);
        }
    }
}
