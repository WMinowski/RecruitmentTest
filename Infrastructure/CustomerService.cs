using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainStandard;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Infrastructure
{
    public class CustomerService : ICRUD
    {
       
        public CustomerService ()
        {
            
        }



        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService._customers.FindAll(customer => true));
        }

        public IDBEntity Get(int id)
        {
            return StaticService._customers.Find(customer => customer.Id == id);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            Customer customer = (Customer)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {

                StaticService.adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.customers " +
                                      "VALUES(@Id,@Name,@FirstName,@DateOfBirth,@Place);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = customer.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar).Value = customer.Name;
                StaticService.adapter.InsertCommand.Parameters.Add("@FirstName", MySqlDbType.VarChar).Value = customer.Name;
                StaticService.adapter.InsertCommand.Parameters.Add("@DateOfBirth", MySqlDbType.Date).Value = customer.DateOfBirth.ToString("yyyy-MM-dd");
                StaticService.adapter.InsertCommand.Parameters.Add("@Place", MySqlDbType.Int32).Value = customer.Place.Id;


                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService._customers.Add(customer);
            return customer;
        }

        public void Update(int id, IDBEntity entity)
        {
            Customer customerIn = (Customer)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE customers SET Id = " + customerIn.Id + " ,Name = '" + customerIn.Name +
                            @"',FirstName = '" + customerIn.FirstName +
                            @"',DateOfBirth = '" + customerIn.DateOfBirth.ToString("yyyy-MM-dd") + 
                            @"',PlaceId = " + customerIn.Place.Id.ToString() +
                            @" WHERE (" +
                            @"Id = " + id + @")";

                MySqlDataAdapter localAadapter = new MySqlDataAdapter();

                localAadapter.UpdateCommand = new MySqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            StaticService._customers.Remove(StaticService._customers.Find(customer => customer.Id==id));
            StaticService._customers.Add(customerIn);
        }

        public void Remove(IDBEntity entity)
        {
            Customer customerIn = (Customer)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.customers WHERE (Name = '" + customerIn.Name + "') AND (FirstName = '"
                                      + customerIn.FirstName + "');", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService._customers.Remove(customerIn);
        }

        public void Remove(int id)
        {
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.customers WHERE (Id = '" + id.ToString() + "');", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService._customers.Remove(StaticService._customers.Find(x=>x.Id==id));
        }
    }
}

