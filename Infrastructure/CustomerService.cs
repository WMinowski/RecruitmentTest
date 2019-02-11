using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Infrastructure
{
    public class CustomerService
    {
        List<Customer> _customers = new List<Customer>();
        List<City> _cities = new List<City>();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public string ConnectionString { get; set; }
        public CustomerService (string connectionString)
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
                            Street = reader["Street"].ToString(),
                            City = _cities.Find(x => x.Id == Convert.ToInt32(reader["CityId"]))
                            //Convert.ToInt32(reader["CityId"])
                        });
                        
                    }
                }
                
            }
        }

        public void RemoveCity(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.cities WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            _cities.RemoveAt(id);
        }

        public void RemoveCity(City cityIn)
        {
            using (MySqlConnection connection = GetConnection())
            {
                adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.cities WHERE (Id = " + cityIn.Id + ");", connection);
                connection.Open();
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            _cities.Remove(cityIn);
        }

        public void UpdateCity(int id, City cityIn)
        {
            using (MySqlConnection connection = GetConnection())
            {
                string query =
                            @"UPDATE cities SET Id =" + cityIn.Id + " ,City = '" + cityIn.Name +
                            @" WHERE (" +
                            @"Id = " + cityIn.Id + @")";

                MySqlDataAdapter localAadapter = new MySqlDataAdapter();

                localAadapter.UpdateCommand = new MySqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            _cities.Remove(_cities.Find(city => city.Id == id));
            _cities.Add(cityIn);
        }

        public City CreateCity(City city)
        {
            using (MySqlConnection connection = GetConnection())
            {
                adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.cities " +
                                      "VALUES(@Id,@City);",
                                      connection);
                adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = city.Id;
                adapter.InsertCommand.Parameters.Add("@City", MySqlDbType.VarChar).Value = city.Name;



                adapter.InsertCommand.ExecuteNonQuery();
            }
            _cities.Add(city);
            return city;
        }

        public City GetCity(int id)
        {
            return _cities.Find(city => city.Id == id);
        }

        public List<City> GetCities()
        {
            return _cities.FindAll(city => true);
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        


        public List<Customer> Get()
        {
            return _customers.FindAll(customer => true);
        }

        public Customer Get(int id)
        {
            return _customers.Find(customer => customer.Id == id);
        }

        public Customer Create(Customer customer)
        {
            using (MySqlConnection connection = GetConnection())
            {
                adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.customers " +
                                      "VALUES(@Id,@Name,@FirstName,@DateOfBirth,@Street,@City);",
                                      connection);
                adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = customer.Id;
                adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar).Value = customer.Name;
                adapter.InsertCommand.Parameters.Add("@FirstName", MySqlDbType.VarChar).Value = customer.Name;
                adapter.InsertCommand.Parameters.Add("@DateOfBirth", MySqlDbType.Date).Value = customer.DateOfBirth.ToString("yyyy-MM-dd");
                adapter.InsertCommand.Parameters.Add("@Street", MySqlDbType.VarChar).Value = customer.Street;
                adapter.InsertCommand.Parameters.Add("@City", MySqlDbType.Int32).Value = customer.City.Id;



                adapter.InsertCommand.ExecuteNonQuery();
            }
            _customers.Add(customer);
            return customer;
        }

        public void Update(int id, Customer customerIn)
        {
            using (MySqlConnection connection = GetConnection())
            {
                string query =
                            @"UPDATE customers SET Id =" + customerIn.Id + " ,Name = '" + customerIn.Name +
                            @"',FirstName = '" + customerIn.FirstName +
                            @"',DateOfBirth = '" + customerIn.DateOfBirth.ToString("yyyy-MM-dd") + 
                            @"' ,Street = '" + customerIn.Street +
                            @"',CityId = " + customerIn.Id.ToString() +
                            @" WHERE (" +
                            @"Name = '" + customerIn.Name +
                            @"') AND (FirstName = '" + customerIn.FirstName + @"')";

                MySqlDataAdapter localAadapter = new MySqlDataAdapter();

                localAadapter.UpdateCommand = new MySqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            _customers.Remove(_customers.Find(customer => customer.Id==id));
            _customers.Add(customerIn);
        }

        public void Remove(Customer customerIn)
        {
            using (MySqlConnection connection = GetConnection())
            {
                adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.customers WHERE (Name = '" + customerIn.Name + "') AND (FirstName = '"
                                      + customerIn.FirstName + "');", connection);
                connection.Open();
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            _customers.Remove(customerIn);
        }

        public void Remove(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.customers WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            _customers.RemoveAt(id);
        }
    }
}

