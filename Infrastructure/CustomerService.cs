using System.Collections.Generic;
using DomainStandard;
using System.Data.SQLite;

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
            using (SQLiteConnection connection = StaticService.GetConnection())
            {

                StaticService.adapter.InsertCommand = new SQLiteCommand(
                                      "INSERT INTO mydb.customers " +
                                      "VALUES(@Id,@Name,@FirstName,@DateOfBirth);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", System.Data.DbType.Int32).Value = customer.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@Name", System.Data.DbType.String).Value = customer.Name;
                StaticService.adapter.InsertCommand.Parameters.Add("@FirstName", System.Data.DbType.String).Value = customer.Name;
                StaticService.adapter.InsertCommand.Parameters.Add("@DateOfBirth", System.Data.DbType.Date).Value = customer.DateOfBirth.ToString("yyyy-MM-dd");



                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService.customers.Add(customer);
            return customer;
        }

        public void Update(int id, IDBEntity entity)
        {
            Customer customerIn = (Customer)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE customers SET Id = " + customerIn.Id + " ,Name = '" + customerIn.Name +
                            @"',FirstName = '" + customerIn.FirstName +
                            @"',DateOfBirth = '" + customerIn.DateOfBirth.ToString("yyyy-MM-dd") + 
                            @"' WHERE (" +
                            @"Id = " + id + @")";

                SQLiteDataAdapter localAadapter = new SQLiteDataAdapter();

                localAadapter.UpdateCommand = new SQLiteCommand(query, connection);
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
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.customers WHERE (Name = '" + customerIn.Name + "') AND (FirstName = '"
                                      + customerIn.FirstName + "');", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.customers.Remove(customerIn);
        }

        public void Remove(int id)
        {
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.customers WHERE (Id = '" + id.ToString() + "');", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.customers.Remove(StaticService.customers.Find(x=>x.Id==id));
        }
    }
}

