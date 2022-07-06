using Meli.Common.Entities;
using MongoDB.Driver;


namespace Meli.AccessLayer.ContextMongoDB
{
    public interface IDnaContext
    {
        IMongoCollection<Dna> DnaCollection { get; }
    }
}
