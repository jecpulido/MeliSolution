using Meli.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meli.AccessLayer.Contract
{
    public interface IIndividualRepository
    {
        Task<IEnumerable<Dna>> GetDnaCollection();
        Task<IEnumerable<Dna>> GetDnaSecuence(string secuence);

        Task InsertDna(Dna dna);
    }
}
