using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

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
                MySqlCommand cmd = new MySqlCommand("select * from mydb.customers", conn); 
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
                foreach(Customer c in customers)
                {
                    MySqlCommand command = new MySqlCommand("select mydb.customersplaces.PlaceId from mydb.customersplaces where CustomerId = " + c.Id + " order by mydb.customersplaces.UpdateTime desc limit 1", conn);
                    using(var reader1 = command.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            c.Place = places.First(x => x.Id == Convert.ToInt32(reader1["PlaceId"]));
                        }
                    }
                    string lastUpdateTime = string.Empty;
                    MySqlCommand lastUpdate = new MySqlCommand("select mydb.customersplaces.UpdateTime from mydb.customersplaces where mydb.customersplaces.customerId = " + c.Id + " order by mydb.customersplaces.UpdateTime desc limit 1", conn);
                    using (var reader2 = lastUpdate.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            lastUpdateTime = reader2["UpdateTime"].ToString();
                        }
                    }
                    List<CustomerPlace> collisions = new List<CustomerPlace>();
                    MySqlCommand lastCollision = new MySqlCommand("select * from mydb.customersplaces where (mydb.customersplaces.UpdateTime > '" + lastUpdateTime + "'-15) AND (CustomerId = " + c.Id + ")", conn);
                    using (var reader3 = lastCollision.ExecuteReader())
                    {
                        while (reader3.Read())
                        {
                            collisions.Add(new CustomerPlace()
                            {
                                Id = Convert.ToInt32(reader3["Id"]),
                                UpdateTime = Convert.ToDateTime(reader3["UpdateTime"]),
                                CustomerId = Convert.ToInt32(reader3["CustomerId"]),
                                PlaceId = Convert.ToInt32(reader3["PlaceId"])

                            });
                        }
                    }
                    if(collisions.Count > 1) { c.HasCollisions = true; }
                }
            }
            
        }

        public static void LoadCustomersPlaces()
        {
            customersPlaces.Clear();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select * from mydb.customersplaces where mydb.customersplaces.UpdateTime > now()-15", conn);
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
