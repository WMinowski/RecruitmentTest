using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainStandard;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RecruitmentTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly PlaceService _placeService;

        public PlaceController(PlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        public ActionResult<List<Place>> Get()
        {
            var temp = new List<Place>();
            foreach (IDBEntity e in _placeService.Get())
            {
                temp.Add(e as Place);
            }

            return temp;
        }

        [HttpGet("{id}", Name = "GetPlace")]
        public ActionResult<Place> Get(string id)
        {
            var place = _placeService.Get(int.Parse(id));

            if (place == null)
            {
                return NotFound();
            }

            return place as Place;
        }

        [HttpPost]
        public ActionResult<Place> Create(Place place)
        {
            _placeService.Create(place);

            return CreatedAtRoute("GetPlace", new { id = place.Id.ToString() }, place);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Place placeIn)
        {
            var place = _placeService.Get(int.Parse(id));

            if (place == null)
            {
                return NotFound();
            }

            _placeService.Update(int.Parse(id), placeIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var place = _placeService.Get(int.Parse(id));

            if (place == null)
            {
                return NotFound();
            }

            _placeService.Remove(place.Id);

            return NoContent();
        }
    }
}
