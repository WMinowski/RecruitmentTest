using DomainStandard;
using System.Data.SQLite;
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
        public static SQLiteDataAdapter adapter = new SQLiteDataAdapter();

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        public static void GetData(string connectionString)
        {
            ConnectionString = connectionString;
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM cities", conn);
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

            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM places", conn);
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

            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM customers", conn); 
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        customers.Add(new Customer()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"])
                        });

                    }
                }

            }
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM customersPlaces", conn); 
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
