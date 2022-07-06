using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meli.Common.Entities
{
    [BsonCollection("dna")]
    public class Dna : Document
    {
        [BsonElement("dnaSecuence")]
        public string DnaSecuence{ get; set; }

        [BsonElement("isMutant")]
        public bool IsMutant { get; set; }
        
    }
}
