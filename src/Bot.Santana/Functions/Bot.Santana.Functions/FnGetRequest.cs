using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bot.Santana.Functions.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bot.Santana.Functions
{
    public static class FnGetRequest
    {
        [FunctionName("FnGetRequest")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string date = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "date", true) == 0)
                .Value;
            var client = new MongoClient("<YOUR_MONGO_CONNECTION_HERE");

            var database = client.GetDatabase("santanabotdb");

            var collection = database.GetCollection<Request>("request");

            var requests = await collection.Find(new BsonDocument()).ToListAsync();

            var random = new Random();
            var index = random.Next(0, (int)collection.Count(new BsonDocument()));

            var request = requests.ElementAt(index);

            return request == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse<Request>(HttpStatusCode.OK, request);
        }
    }
}
