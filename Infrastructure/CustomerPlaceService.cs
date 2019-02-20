using DomainStandard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SqlCommand("DELETE FROM mydb.customersPlaces WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.customersPlaces.Remove(StaticService.customersPlaces.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            CustomerPlace customerPlaceIn = (CustomerPlace)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SqlCommand("DELETE FROM mydb.customersPlaces WHERE (Id = " + customerPlaceIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.customersPlaces.Remove(customerPlaceIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            CustomerPlace customerPlaceIn = (CustomerPlace)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE customersPlaces SET Id =" + customerPlaceIn.Id + " ,UpdateTime = '" + customerPlaceIn.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' ,CustomerId = " + customerPlaceIn.CustomerId + " ,PlaceId = " + customerPlaceIn.PlaceId +
                            @" WHERE (" +
                            @"Id = " + customerPlaceIn.Id + @")";

                SqlDataAdapter localAadapter = new SqlDataAdapter();

                localAadapter.UpdateCommand = new SqlCommand(query, connection);
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
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new SqlCommand(
                                      "INSERT INTO mydb.customersPlaces " +
                                      "VALUES(@Id,@UpdateTime,@CustomerId,@PlaceId);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int).Value = customerPlace.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@UpdateTime", SqlDbType.Timestamp).Value = customerPlace.UpdateTime; // no ToString("FormatString")
                StaticService.adapter.InsertCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerPlace.CustomerId;
                StaticService.adapter.InsertCommand.Parameters.Add("@PlaceId", SqlDbType.Int).Value = customerPlace.PlaceId;

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
