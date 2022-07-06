using Meli.AccessLayer.ContextMongoDB;
using Meli.AccessLayer.Contract;
using Meli.Common.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meli.AccessLayer.Repository
{
    public class DnaRepository : IIndividualRepository
    {
        private readonly IDnaContext _dnaContext;

        public DnaRepository(IDnaContext dnaContext)
        {
            _dnaContext = dnaContext;
        }

        public async Task<IEnumerable<Dna>> GetDnaCollection()
        {
            return await _dnaContext.DnaCollection.Find(d => true).ToListAsync();
        }

        public async Task<IEnumerable<Dna>> GetDnaSecuence(string secuence)
        {
            return await _dnaContext.DnaCollection.Find(dna => dna.DnaSecuence == secuence).ToListAsync();
        }

        public async Task InsertDna(Dna dna)
        {
            await _dnaContext.DnaCollection.InsertOneAsync(dna);
        }
    }
}
