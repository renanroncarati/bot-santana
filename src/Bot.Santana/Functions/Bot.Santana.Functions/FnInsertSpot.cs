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
    public static class FnInsertSpot
    {
        [FunctionName("FnInsertSpot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            //string name = req.GetQueryNameValuePairs()
            //    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            //    .Value;

            var client = new MongoClient("<YOUR_MONGO_CONNECTION_HERE");


            var database = client.GetDatabase("santanabotdb");

           

            // Get request body
            var spot = await req.Content.ReadAsAsync<Spot>();

            var collection = database.GetCollection<Spot>("spot");
            await collection.InsertOneAsync(spot);

            // Set name to query string or body data
            //name = name ?? spot?.name;

            return spot == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + spot.Name);
        }
    }
}
