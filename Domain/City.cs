using System;
using System.Collections.Generic;
using System.Text;

namespace DomainStandard
{
    public class City : IDBEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public City() { }
        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
