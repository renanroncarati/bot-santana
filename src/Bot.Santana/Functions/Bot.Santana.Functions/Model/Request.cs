using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Santana.Functions.Model
{
    public class Request
    {
        public ObjectId _id { get; set; }
        public string Identity { get; set; }
    }
}
