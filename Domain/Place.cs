using System;
using System.Collections.Generic;
using System.Text;

namespace DomainStandard
{
    public class Place : IDBEntity
    {
        public int Id { get; set; }
        public City City { get; set; }
        public string Street { get; set; }

        public Place() { }

        public Place(int id, City city, string street)
        {
            Id = id;
            City = city;
            Street = street;
        }
        public override string ToString()
        {
            return City.Name + ", " + Street;
        }
    }
}
