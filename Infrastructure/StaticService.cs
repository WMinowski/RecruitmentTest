using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public static class StaticService
    {
        public static List<Customer> customers = new List<Customer>();
        public static List<City> cities = new List<City>();
        public static List<Place> places = new List<Place>();
        public static List<CustomerPlace> customersPlaces = new List<CustomerPlace>();
        public static string ConnectionString { get; set; }
        public static MySqlDataAdapter adapter = new MySqlDataAdapter();

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
                        cities.Add(new City()
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
                        var cities = StaticService.cities;
                        places.Add(new Place()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            City = StaticService.cities.Find(x => x.Id == Convert.ToInt32(reader["CityId"])),
                            Street = reader["Street"].ToString()
                        });
                    }
                }
            }

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM mydb.customers", conn); 
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        customers.Add(new Customer()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Place = places.Find(x => x.Id == Convert.ToInt32(reader["PlaceId"]))
                        });

                    }
                }

            }
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM mydb.customersPlaces", conn); 
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        customersPlaces.Add(new CustomerPlace()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            UpdateTime = Convert.ToDateTime(reader["UpdateTime"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            PlaceId = Convert.ToInt32(reader["PlaceId"])
                            
                        });

                    }
                }

            }
        }
    }
}
