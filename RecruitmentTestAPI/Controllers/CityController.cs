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
    }
}
