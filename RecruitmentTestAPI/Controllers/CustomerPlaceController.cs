using System.Collections.Generic;
using DomainStandard;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace RecruitmentTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPlaceController : ControllerBase
    {
        private readonly CustomerPlaceService _placeUpdateService;

        public CustomerPlaceController(CustomerPlaceService placeUpdateService)
        {
            _placeUpdateService = placeUpdateService;
        }

        [HttpGet]
        public ActionResult<List<CustomerPlace>> Get()
        {
            var temp = new List<CustomerPlace>();
            foreach (IDBEntity e in _placeUpdateService.Get())
            {
                temp.Add(e as CustomerPlace);
            }

            return temp;
        }

        [HttpGet("{id}", Name = "GetPlaceUpdate")]
        public ActionResult<CustomerPlace> Get(string id)
        {
            var placeUpdate = _placeUpdateService.Get(int.Parse(id));

            if (placeUpdate == null)
            {
                return NotFound();
            }

            return placeUpdate as CustomerPlace;
        }

        [HttpPost]
        public ActionResult<Place> Create(CustomerPlace placeUpdate)
        {
            _placeUpdateService.Create(placeUpdate);

            return CreatedAtRoute("GetPlaceUpdate", new { id = placeUpdate.Id.ToString() }, placeUpdate);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, CustomerPlace placeUpdateIn)
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