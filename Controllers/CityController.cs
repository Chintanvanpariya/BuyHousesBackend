using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]   // this thing makes the [Required] attribute in model work !!
    public class CityController : ControllerBase
    {
        private readonly DataContext dc;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CityController(DataContext dc, IUnitOfWork uow, IMapper mapper)
        {
            this.dc = dc;
            this.uow = uow;
            this.mapper = mapper;
        }
         
        //   api/city
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
           var cities = await uow.CityRepository.GetCitiesAsync();

            //var citiesDto = from c in cities
            //                select new CityDto()
            //                {
            //                    Id = c.Id,
            //                    Name = c.Name
            //                };

            var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);

            return Ok(citiesDto);
        }


        // api/city/1
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "houston";
        }

        // in first one the name "cityname" spelling and in the 
        // second one the {cityname} spelling 
        // should be same the input parameter name in the method, 
        // spellings are case insensitive.
        
        
        [HttpPost("add")]             // api/city/add?cityname=Miami
        //HttpPost("add/{cityname?}")]  // api/city/add/Mumbai
        public async Task<IActionResult> AddCity(string cityName)
        {

            Console.WriteLine(cityName);
            City city = new City();
            city.Name = cityName;

            //await dc.Cities.AddAsync(city);
            //await dc.SaveChangesAsync();

            uow.CityRepository.AddCity(city);
            await uow.SaveAsync();

            return Ok();
        }

       
        [HttpPost("post")]
        public async Task<IActionResult> AddCity(CityDto cityDto)
        {
            //await dc.Cities.AddAsync(cityNAme);

            //var city = new City
            //{
            //    Name = cityDto.Name
            //};
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var city = mapper.Map<City>(cityDto);

            uow.CityRepository.AddCity(city);

            await uow.SaveAsync();
            return StatusCode(201);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            //var city = await dc.Cities.FindAsync(id);
            //dc.Cities.Remove(city);

            uow.CityRepository.DeleteCity(id);
            await uow.SaveAsync();

            return Ok(id);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityDto cityDto)
        {
            var cityfromdb = await uow.CityRepository.FindCity(id);
            if(cityfromdb == null)
                return BadRequest("update not allowed");

            mapper.Map(cityDto, cityfromdb);


            await uow.SaveAsync();

            return StatusCode(200);
        }


        [HttpPut("updateCityname/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityUpdateDto cityDto)
        {
            var cityfromdb = await uow.CityRepository.FindCity(id);
            mapper.Map(cityDto, cityfromdb);
            await uow.SaveAsync();

            return StatusCode(200);
        }


        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCityPatch(int id, JsonPatchDocument<City> citytopatch)
        {
            var cityfromdb = await uow.CityRepository.FindCity(id);
           
            citytopatch.ApplyTo(cityfromdb, ModelState);
            await uow.SaveAsync();
            return StatusCode(200);
        }

    }
}
