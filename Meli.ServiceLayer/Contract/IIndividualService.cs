using Meli.Common.Entities;
using Meli.Common.Enum;
using Meli.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meli.ServiceLayer.Contract
{
    public interface IIndividualService
    {
        Task<int> ValidateIndividual(IndividualModel individualModel);

        Task<StatsModel> GetStats();
    }
}
