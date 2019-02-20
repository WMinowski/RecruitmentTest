using DomainStandard;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infrastructure
{
    public static class StaticService
    {
        public static List<Customer> customers = new List<Customer>();
        public static List<City> cities = new List<City>();
        public static List<Place> places = new List<Place>();
        public static List<CustomerPlace> customersPlaces = new List<CustomerPlace>();
        public static string ConnectionString { get; set; }
        public static SqlDataAdapter adapter = new SqlDataAdapter();

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static void GetData(string connectionString)
        {
            ConnectionString = connectionString;
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM mydb.cities", conn);
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

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM mydb.places", conn);
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

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM mydb.customers", conn); 
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
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM mydb.customersPlaces", conn); 
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
