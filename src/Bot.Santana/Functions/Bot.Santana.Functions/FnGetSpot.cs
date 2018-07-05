using System;
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
    public static class FnGetSpot
    {
        [FunctionName("FnGetSpot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string date = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "date", true) == 0)
                .Value;

            string identity = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "identity", true) == 0)
                .Value;

            var client = new MongoClient("<YOUR_MONGO_CONNECTION_HERE");


            var database = client.GetDatabase("santanabotdb");

            var spotCollection = database.GetCollection<Spot>("spot");

            var filter = Builders<Spot>.Filter.Eq(nameof(Spot.Date), Convert.ToDateTime(date).Date);

            var document = spotCollection.Find(filter).FirstOrDefault();
            if (document != null)
            {

                var deleteSpotFilter = Builders<Spot>.Filter.Eq(nameof(Spot._id), document._id);
                var result = await spotCollection.DeleteOneAsync(deleteSpotFilter);


                var deleteRequestFilter = Builders<Request>.Filter.Eq(nameof(Request.Identity), identity);
                var requestCollection = database.GetCollection<Request>("request");
                var requestResult = await requestCollection.DeleteOneAsync(deleteRequestFilter);
            }


            return document == null
                ? req.CreateResponse(HttpStatusCode.NotFound)
                : req.CreateResponse(HttpStatusCode.OK, document);
        }
    }
}
