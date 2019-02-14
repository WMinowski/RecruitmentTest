using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class StaticService
    {
        public static List<Customer> _customers = new List<Customer>();
        public static List<City> _cities = new List<City>();
        public static List<Place> _places = new List<Place>();
        public static List<PlaceUpdate> _placeUpdates = new List<PlaceUpdate>();
        public static string ConnectionString { get; set; }
        public static MySqlDataAdapter adapter = new MySqlDataAdapter();

        //static StaticService()
        //{
            
        //}
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static void GetData(string connectionString)
        {
            ConnectionString = connectionString;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM mydb.cities", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _cities.Add(new City()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["City"].ToString()
                        });
                    }
                }
            }

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM mydb.places", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _places.Add(new Place()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            City = _cities.Find(x => x.Id == Convert.ToInt32(reader["CityId"])),
                            Street = reader["Street"].ToString()
                        });
                    }
                }
            }

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM mydb.customers", conn); /////// "select customers.Id customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id"
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        _customers.Add(new Customer()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Place = _places.Find(x => x.Id == Convert.ToInt32(reader["PlaceId"]))
                            //Convert.ToInt32(reader["CityId"])
                        });

                    }
                }

            }
            //TODO: placeUpdate read
        }
    }
}
