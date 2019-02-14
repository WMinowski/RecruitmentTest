using System;
using System.Collections.Generic;
using System.Text;

namespace DomainStandard
{
    public class PlaceUpdate : IDBEntity
    {
        public int Id { get; set; }
        public DateTime UpdateTime { get; set; }
        public int CustomerId { get; set; }
        public int PlaceId { get; set; }

        public PlaceUpdate() { }

        public PlaceUpdate(int id, DateTime updateTime, int customerId, int placeId)
        {
            Id = id;
            UpdateTime = updateTime;
            CustomerId = customerId;
            PlaceId = placeId;
        }
    }
}
