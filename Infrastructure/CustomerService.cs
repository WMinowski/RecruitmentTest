using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DomainStandard;

namespace Infrastructure
{
    public class CustomerService : ICRUD
    {
       
        public CustomerService ()
        {
            
        }



        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService.customers.FindAll(customer => true));
        }

        public IDBEntity Get(int id)
        {
            return StaticService.customers.Find(customer => customer.Id == id);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            Customer customer = (Customer)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {

                StaticService.adapter.InsertCommand = new SqlCommand(
                                      "INSERT INTO mydb.customers " +
                                      "VALUES(@Name,@FirstName,@DateOfBirth);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = customer.Name;
                StaticService.adapter.InsertCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = customer.Name;
                StaticService.adapter.InsertCommand.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = customer.DateOfBirth.ToString("yyyy-MM-dd");



                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService.customers.Add(customer);
            return customer;
        }

        public void Update(int id, IDBEntity entity)
        {
            Customer customerIn = (Customer)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE mydb.customers SET Name = '" + customerIn.Name +
                            @"',FirstName = '" + customerIn.FirstName +
                            @"',DateOfBirth = '" + customerIn.DateOfBirth.ToString("yyyy-MM-dd") + 
                            @"' WHERE (" +
                            @"Id = " + id + @")";

                SqlDataAdapter localAadapter = new SqlDataAdapter();

                localAadapter.UpdateCommand = new SqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            StaticService.customers.Remove(StaticService.customers.Find(customer => customer.Id==id));
            StaticService.customers.Add(customerIn);
        }

        public void Remove(IDBEntity entity)
        {
            Customer customerIn = (Customer)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SqlCommand("DELETE FROM mydb.customers WHERE (Name = '" + customerIn.Name + "') AND (FirstName = '"
                                      + customerIn.FirstName + "');", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.customers.Remove(customerIn);
        }

        public void Remove(int id)
        {
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SqlCommand("DELETE FROM mydb.customers WHERE (Id = '" + id.ToString() + "');", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.customers.Remove(StaticService.customers.Find(x=>x.Id==id));
        }
    }
}

