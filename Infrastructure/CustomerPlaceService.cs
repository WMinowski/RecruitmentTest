using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class CustomerPlaceService : ICRUD
    {
        
        public CustomerPlaceService()
        {
            
        }

        public void Remove(int id)
        {
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.customersPlaces WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.customersPlaces.Remove(StaticService.customersPlaces.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            CustomerPlace customerPlaceIn = (CustomerPlace)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.customersPlaces WHERE (Id = " + customerPlaceIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.customersPlaces.Remove(customerPlaceIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            CustomerPlace customerPlaceIn = (CustomerPlace)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE customersPlaces SET Id =" + customerPlaceIn.Id + " ,UpdateTime = '" + customerPlaceIn.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' ,CustomerId = " + customerPlaceIn.CustomerId + " ,PlaceId = " + customerPlaceIn.PlaceId +
                            @" WHERE (" +
                            @"Id = " + customerPlaceIn.Id + @")";

                MySqlDataAdapter localAadapter = new MySqlDataAdapter();

                localAadapter.UpdateCommand = new MySqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            StaticService.customersPlaces.Remove(StaticService.customersPlaces.Find(city => city.Id == id));
            StaticService.customersPlaces.Add(customerPlaceIn);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            CustomerPlace customerPlace = (CustomerPlace)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.customersPlaces " +
                                      "VALUES(@Id,@UpdateTime,@CustomerId,@PlaceId);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = customerPlace.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@UpdateTime", MySqlDbType.Timestamp).Value = customerPlace.UpdateTime; // no ToString("FormatString")
                StaticService.adapter.InsertCommand.Parameters.Add("@CustomerId", MySqlDbType.Int32).Value = customerPlace.CustomerId;
                StaticService.adapter.InsertCommand.Parameters.Add("@PlaceId", MySqlDbType.Int32).Value = customerPlace.PlaceId;

                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService.customersPlaces.Add(customerPlace);
            return customerPlace;
        }

        public IDBEntity Get(int id)
        {
            return StaticService.customersPlaces.Find(customerPlace => customerPlace.Id == id);
        }

        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService.customersPlaces.FindAll(customerPlace => true));
        }
    }
}
