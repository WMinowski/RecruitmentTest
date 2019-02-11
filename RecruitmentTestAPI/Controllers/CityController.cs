using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;

namespace RecruitmentTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CityController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public ActionResult<List<City>> Get()
        {
            var temp = _customerService.GetCities();
            return temp;
        }

        [HttpGet("{id:length(24)}", Name = "GetCity")]
        public ActionResult<City> Get(string id)
        {
            var city = _customerService.GetCity(int.Parse(id));

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        [HttpPost]
        public ActionResult<City> Create(City city)
        {
            _customerService.CreateCity(city);

            return CreatedAtRoute("GetCity", new { id = city.Id.ToString() }, city);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, City cityIn)
        {
            var city = _customerService.GetCity(int.Parse(id));

            if (city == null)
            {
                return NotFound();
            }

            _customerService.UpdateCity(int.Parse(id), cityIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var city = _customerService.GetCity(int.Parse(id));

            if (city == null)
            {
                return NotFound();
            }

            _customerService.RemoveCity(city.Id);

            return NoContent();
        }
    }
}
