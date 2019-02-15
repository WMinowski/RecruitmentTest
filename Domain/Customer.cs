using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DomainStandard
{
    public class Customer : IDBEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }


        public Customer() { }

        public Customer(int id, string name, string firstName, DateTime dateOfBirth)
        {
            Id = id;
            Name = name;
            FirstName = firstName;
            DateOfBirth = dateOfBirth;

        }
    }

}
