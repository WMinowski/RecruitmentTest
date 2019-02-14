using System.Collections.Generic;
using DomainStandard;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace RecruitmentTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceUpdateController : ControllerBase
    {
        private readonly PlaceUpdateService _placeUpdateService;

        public PlaceUpdateController(PlaceUpdateService placeUpdateService)
        {
            _placeUpdateService = placeUpdateService;
        }

        [HttpGet]
        public ActionResult<List<PlaceUpdate>> Get()
        {
            var temp = new List<PlaceUpdate>();
            foreach (IDBEntity e in _placeUpdateService.Get())
            {
                temp.Add(e as PlaceUpdate);
            }

            return temp;
        }

        [HttpGet("{id}", Name = "GetPlaceUpdate")]
        public ActionResult<PlaceUpdate> Get(string id)
        {
            var placeUpdate = _placeUpdateService.Get(int.Parse(id));

            if (placeUpdate == null)
            {
                return NotFound();
            }

            return placeUpdate as PlaceUpdate;
        }

        [HttpPost]
        public ActionResult<Place> Create(PlaceUpdate placeUpdate)
        {
            _placeUpdateService.Create(placeUpdate);

            return CreatedAtRoute("GetPlaceUpdate", new { id = placeUpdate.Id.ToString() }, placeUpdate);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, PlaceUpdate placeUpdateIn)
        {
            var place = _placeUpdateService.Get(int.Parse(id));

            if (place == null)
            {
                return NotFound();
            }

            _placeUpdateService.Update(int.Parse(id), placeUpdateIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var placeUpdate = _placeUpdateService.Get(int.Parse(id));

            if (placeUpdate == null)
            {
                return NotFound();
            }

            _placeUpdateService.Remove(placeUpdate.Id);

            return NoContent();
        }
    }
}