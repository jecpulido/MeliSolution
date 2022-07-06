using Meli.AccessLayer.Contract;
using Meli.Common.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meli.AccessLayer.ContextMongoDB
{
    public class DnaContext : IDnaContext
    {
        public readonly IMongoDatabase _db;

        public DnaContext(IOptions<MongoSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<Dna> DnaCollection => _db.GetCollection<Dna>("Dna");
    }
}
