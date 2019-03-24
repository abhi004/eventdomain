using System;
using System.Threading.Tasks;
using System.Configuration;

namespace EventPublish
{
    using Microsoft.Azure.EventGrid;
    using Microsoft.Azure.EventGrid.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;

    class Program
    {
       private static readonly string eventdomainkey = Environment.GetEnvironmentVariable("event_domain_key");
       private static readonly string eventdomainhotname = Environment.GetEnvironmentVariable("event_domain_hostname");
        static async Task Main(string[] args)
        {
            string domainHostname = new Uri(eventdomainhotname).Host;
            dynamic EventSourceList = await EventPublishMethod.GetEventSourceAsync();

            foreach (var item in EventSourceList)
            {
                List<EventGridEvent> eventlist = new List<EventGridEvent>();
                for (int i = 0; i < 4; i++)
                {
                    eventlist.Add(new EventGridEvent
                    {
                        Data = item.Data,
                        Id = item.RandomId,
                        DataVersion = "1.0",
                        EventType = "integrationevent.published",
                        Subject = item.Subject,
                        Topic = item.Topic,
                    });

                }
                var content = JsonConvert.SerializeObject(eventlist);
                TopicCredentials domaintopic = new TopicCredentials(eventdomainkey);
                Lazy<EventGridClient> domainclient = new Lazy<EventGridClient>(new EventGridClient(domaintopic));
                var _client = domainclient.Value;
                _client.PublishEventsAsync(domainHostname, eventlist).GetAwaiter().GetResult();

            }

          }

      }
}

