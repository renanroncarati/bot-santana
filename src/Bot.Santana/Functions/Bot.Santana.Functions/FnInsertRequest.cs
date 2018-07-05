using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bot.Santana.Functions.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Driver;

namespace Bot.Santana.Functions
{
    public static class FnInsertRequest
    {
        [FunctionName("FnInsertRequest")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var client = new MongoClient("<YOUR_MONGO_CONNECTION_HERE");


            var database = client.GetDatabase("santanabotdb");


            // Get request body
            var request = await req.Content.ReadAsAsync<Request>();

            var collection = database.GetCollection<Request>("request");

            await collection.InsertOneAsync(request);
            

            return request == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + request.Identity);
        }
    }
}
