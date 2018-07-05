using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Santana.Functions.Model
{
    
    public class Spot
    {
        public ObjectId _id { get; set; }
        public string Identity { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
