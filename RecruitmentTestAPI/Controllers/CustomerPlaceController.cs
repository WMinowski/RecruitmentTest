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
        private readonly CustomerPlaceService _customerPlaceService;

        public CustomerPlaceController(CustomerPlaceService placeUpdateService)
        {
            _customerPlaceService = placeUpdateService;
        }

        [HttpGet]
        public ActionResult<List<CustomerPlace>> Get()
        {
            var temp = new List<CustomerPlace>();
            foreach (IDBEntity e in _customerPlaceService.Get())
            {
                temp.Add(e as CustomerPlace);
            }

            return temp;
        }

        [HttpGet("{id}", Name = "GetPlaceUpdate")]
        public ActionResult<CustomerPlace> Get(string id)
        {
            var customerPlace = _customerPlaceService.Get(int.Parse(id));

            if (customerPlace == null)
            {
                return NotFound();
            }

            return customerPlace as CustomerPlace;
        }

        [HttpPost]
        public ActionResult<Place> Create(CustomerPlace customerPlace)
        {
            _customerPlaceService.Create(customerPlace);

            return CreatedAtRoute("GetPlaceUpdate", new { id = customerPlace.Id.ToString() }, customerPlace);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, CustomerPlace customerPlaceIn)
        {
            var customerPlace = _customerPlaceService.Get(int.Parse(id));

            if (customerPlace == null)
            {
                return NotFound();
            }

            _customerPlaceService.Update(int.Parse(id), customerPlaceIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var customerPlace = _customerPlaceService.Get(int.Parse(id));

            if (customerPlace == null)
            {
                return NotFound();
            }

            _customerPlaceService.Remove(customerPlace.Id);

            return NoContent();
        }
    }
}