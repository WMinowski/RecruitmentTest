using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class City
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
