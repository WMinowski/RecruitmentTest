using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainStandard;
using Infrastructure;

namespace RecruitmentTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityService _cityService;

        public CityController(CityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public ActionResult<List<City>> Get()
        {
            var temp = new List<City>();
             foreach(IDBEntity e in _cityService.Get())
            {
                temp.Add(e as City);
            }

            return temp;
        }

        [HttpGet("{id}", Name = "GetCity")]
        public ActionResult<City> Get(string id)
        {
            var city = _cityService.Get(int.Parse(id));

            if (city == null)
            {
                return NotFound();
            }

            return city as City;
        }

        [HttpPost]
        public ActionResult<City> Create(City city)
        {
            _cityService.Create(city);

            return CreatedAtRoute("GetCity", new { id = city.Id.ToString() }, city);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, City cityIn)
        {
            var city = _cityService.Get(int.Parse(id));

            if (city == null)
            {
                return NotFound();
            }

            _cityService.Update(int.Parse(id), cityIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var city = _cityService.Get(int.Parse(id));

            if (city == null)
            {
                return NotFound();
            }

            _cityService.Remove(city.Id);

            return NoContent();
        }
    }
}
