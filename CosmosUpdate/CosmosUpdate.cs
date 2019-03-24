using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace CosmosUpdate
{
    public static class CosmosUpdate
    {
        [FunctionName("CosmosUpdate")]
        public static async Task<HttpResponseMessage> CreateCosmosDocument(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req,
    [CosmosDB(databaseName: "sample0102",
    collectionName: "sample01",
    ConnectionStringSetting = "CosmosDBConnection",CreateIfNotExists = true,PartitionKey = "/sample")]
    IAsyncCollector<CosmosFunctionEvent> cosmosevent, ILogger log)
        {
             EventGridEvent[] events = JsonConvert.DeserializeObject<EventGridEvent[]>(await req.Content.ReadAsStringAsync());

            if (events[0].EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
            {
                JObject intialvalidationcode = JObject.Parse(JsonConvert.SerializeObject(events[0].Data));

                var Response = new SubscriptionValidationResponse()
                {
                    ValidationResponse = (string)intialvalidationcode["validationCode"]
                };

            return req.CreateResponse(HttpStatusCode.OK, Response);
        }
            else
            {
                foreach (EventGridEvent item in events)
                {
                    CosmosFunctionEvent obj = new CosmosFunctionEvent();
                    obj.guid = item.Id;
                    obj.data = item.Data.ToString();
                    await cosmosevent.AddAsync(obj);
                }
                return req.CreateResponse(HttpStatusCode.OK, events);
            }

        }
    }
}
