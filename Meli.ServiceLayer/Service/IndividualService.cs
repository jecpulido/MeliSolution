using AutoMapper;
using Meli.AccessLayer.Contract;
using Meli.Common.Entities;
using Meli.Common.Enum;
using Meli.Common.Models;
using Meli.Common.Utils;
using Meli.ServiceLayer.Contract;
using Meli.ServiceLayer.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meli.ServiceLayer.Service
{
    public class IndividualService : IIndividualService
    {

        private readonly IIndividualRepository _dnaRepository;
        private readonly IMapper _mapper;

        public IndividualService(IIndividualRepository dnaRepository, IMapper mapper)
        {
            _dnaRepository = dnaRepository;
            _mapper = mapper;
        }

        public async Task<int> ValidateIndividual(IndividualModel individualModel) 
        {            
            Individual individual = _mapper.Map<IndividualModel, Individual>(individualModel);
            var validations = IndividualValidations.validationsPro;
            foreach (var fun in validations)
            {
                var result = fun(individual);
                if (!result.ContainsKey(ValidationType.Mutant) && result.ContainsValue(false))
                {
                    return Validation.GetStatudCode(result.Keys.First(),false);
                }
                else if (result.ContainsKey(ValidationType.Mutant)) {
                    bool isMutant = result[ValidationType.Mutant];
                    string dnaSecuence = DnaTools.GenerateDnaSecuence(individual.Dna);
                    var exist = await _dnaRepository.GetDnaSecuence(dnaSecuence);                    
                    if (exist == null || exist.Count() == 0)
                        await _dnaRepository.InsertDna(new Dna() { DnaSecuence = dnaSecuence, IsMutant = isMutant });
                    return Validation.GetStatudCode(ValidationType.Mutant, isMutant);
                }
            }
            return Validation.GetStatudCode(ValidationType.Mutant,false);
        }

        public async Task<StatsModel> GetStats()
        {
            var dnaCollection = await _dnaRepository.GetDnaCollection();
            int mutants = dnaCollection.Where(dna => dna.IsMutant == true).ToList().Count();
            int humans = dnaCollection.Count() - mutants;
            return new StatsModel()
            {
                count_mutant_dna = mutants,
                count_human_dna = humans
            };                    
        }

    }
}
