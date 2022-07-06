using AutoMapper;
using Meli.AccessLayer.ContextMongoDB;
using Meli.AccessLayer.Contract;
using Meli.AccessLayer.Repository;
using Meli.Common.Entities;
using Meli.Common.Models;
using Meli.ServiceLayer.Service;
using Meli.WepApi.Controllers;
using Meli.WepApi.MapperProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Meli.Test
{
    public class MutantTest
    {
        private readonly DnaContext _dnaContext;
        private readonly DnaRepository _dnaRepository;
        private static IMapper _mapper;
        private readonly IndividualService _individualService;
        private readonly MutantController _mutantController;

        public MutantTest()
        {
            var config = InitConfiguration();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            var options = Options.Create(new MongoSettings()
            {
                ConnectionString = config.GetSection("MongoDb:ConnectionString").Value,
                Database = config.GetSection("MongoDb:Database").Value
            });

            _dnaContext = new DnaContext(options);
            _dnaRepository = new DnaRepository(_dnaContext);
            _individualService = new IndividualService(_dnaRepository, _mapper);
            _mutantController = new MutantController(_individualService);

        }

        [Fact]
        public async void TestDnaInvalid_NotContent()
        {                      
            var individual = new IndividualModel();
            IActionResult actionResult = await _mutantController.Post(individual);                        
            Assert.Equal(((int)HttpStatusCode.BadRequest), ((StatusCodeResult)actionResult).StatusCode);
        }

        [Fact]
        public async void TestDnaInvalid_Symmetrical()
        {
            var individual = new IndividualModel() { Dna =  new List<string>(){ "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACT" } };
            IActionResult actionResult = await _mutantController.Post(individual);
            Assert.Equal(((int)HttpStatusCode.BadRequest), ((StatusCodeResult)actionResult).StatusCode);

            individual = new IndividualModel() { Dna = new List<string>() { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA"} };

            actionResult = await _mutantController.Post(individual);
            Assert.Equal(((int)HttpStatusCode.BadRequest), ((StatusCodeResult)actionResult).StatusCode);
        }

        [Fact]
        public async void TestDnaInvalid_Characters()
        {
            var individual = new IndividualModel() { Dna = new List<string>() { "ATGCZ", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" } };

            IActionResult actionResult = await _mutantController.Post(individual);
            Assert.Equal(((int)HttpStatusCode.BadRequest), ((StatusCodeResult)actionResult).StatusCode);
        }

        [Fact]
        public async void TestDnaInvalid_Mutant()
        {
            var individual = new IndividualModel() { Dna = new List<string>() { "ATGCGA", "CGGTGC", "TTATGT", "AGAAGG", "ACCCTA", "TCACTG" } };

            IActionResult actionResult = await _mutantController.Post(individual);
            Assert.Equal(((int)HttpStatusCode.Forbidden), ((StatusCodeResult)actionResult).StatusCode);
        }

        [Fact]
        public async void TestDnaValid_Mutant()
        {
            var individual = new IndividualModel() { Dna = new List<string>() { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" } };
            IActionResult actionResult = await _mutantController.Post(individual);
            Assert.Equal(((int)HttpStatusCode.OK), ((StatusCodeResult)actionResult).StatusCode);
            //Dna no esta registrado (cambiar secuencia por una nueva)
            individual = new IndividualModel() { Dna = new List<string>() { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "TTTTTA", "TCACTG" } };
            actionResult = await _mutantController.Post(individual);
            Assert.Equal(((int)HttpStatusCode.OK), ((StatusCodeResult)actionResult).StatusCode);
        }

        [Fact]
        public async void TestStat()
        {           
            IActionResult actionResult = await _mutantController.Stats();
            //Assert.Equal(((int)HttpStatusCode.OK), ((StatusCodeResult)actionResult).StatusCode);
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.NotNull(((ObjectResult)actionResult).Value);
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }
    }
}
