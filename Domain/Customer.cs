using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Street { get; set; }
        public City City { get; set; }

        public Customer() { }

        public Customer(int id, string name, string firstName, DateTime dateOfBirth, string street, City city)
        {
            Id = id;
            Name = name;
            FirstName = firstName;
            DateOfBirth = dateOfBirth;
            Street = street;
            City = city;
        }
    }

}
