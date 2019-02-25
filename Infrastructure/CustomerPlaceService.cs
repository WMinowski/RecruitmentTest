using DomainStandard;
using System.Data.SQLite;
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
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.customersPlaces WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.customersPlaces.Remove(StaticService.customersPlaces.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            CustomerPlace customerPlaceIn = (CustomerPlace)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.customersPlaces WHERE (Id = " + customerPlaceIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.customersPlaces.Remove(customerPlaceIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            CustomerPlace customerPlaceIn = (CustomerPlace)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE customersPlaces SET Id =" + customerPlaceIn.Id + " ,UpdateTime = '" + customerPlaceIn.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' ,CustomerId = " + customerPlaceIn.CustomerId + " ,PlaceId = " + customerPlaceIn.PlaceId +
                            @" WHERE (" +
                            @"Id = " + customerPlaceIn.Id + @")";

                SQLiteDataAdapter localAadapter = new SQLiteDataAdapter();

                localAadapter.UpdateCommand = new SQLiteCommand(query, connection);
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
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new SQLiteCommand(
                                      "INSERT INTO mydb.customersPlaces " +
                                      "VALUES(@Id,@UpdateTime,@CustomerId,@PlaceId);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", System.Data.DbType.Int32).Value = customerPlace.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@UpdateTime", System.Data.DbType.DateTime).Value = customerPlace.UpdateTime; // no ToString("FormatString")
                StaticService.adapter.InsertCommand.Parameters.Add("@CustomerId", System.Data.DbType.Int32).Value = customerPlace.CustomerId;
                StaticService.adapter.InsertCommand.Parameters.Add("@PlaceId", System.Data.DbType.Int32).Value = customerPlace.PlaceId;

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
