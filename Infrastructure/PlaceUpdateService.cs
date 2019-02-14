using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class PlaceUpdateService : ICRUD
    {
        
        public PlaceUpdateService()
        {
            
        }

        public void Remove(int id)
        {
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.placesUpdates WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService._placeUpdates.Remove(StaticService._placeUpdates.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            PlaceUpdate placeUpdateIn = (PlaceUpdate)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.placesUpdates WHERE (Id = " + placeUpdateIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService._placeUpdates.Remove(placeUpdateIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            PlaceUpdate placeUpdateIn = (PlaceUpdate)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE placesUpdates SET Id =" + placeUpdateIn.Id + " ,UpdateDate = '" + placeUpdateIn.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' ,CustomerId = " + placeUpdateIn.CustomerId + " ,PlaceId = " + placeUpdateIn.PlaceId +
                            @" WHERE (" +
                            @"Id = " + placeUpdateIn.Id + @")";

                MySqlDataAdapter localAadapter = new MySqlDataAdapter();

                localAadapter.UpdateCommand = new MySqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            StaticService._placeUpdates.Remove(StaticService._placeUpdates.Find(city => city.Id == id));
            StaticService._placeUpdates.Add(placeUpdateIn);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            PlaceUpdate placeUpdate = (PlaceUpdate)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.placesUpdates " +
                                      "VALUES(@Id,@City);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = placeUpdate.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@City", MySqlDbType.DateTime).Value = placeUpdate.UpdateTime; // no ToString("FormatString")
                StaticService.adapter.InsertCommand.Parameters.Add("@CustomerId", MySqlDbType.Int32).Value = placeUpdate.CustomerId;
                StaticService.adapter.InsertCommand.Parameters.Add("@PlaceId", MySqlDbType.Int32).Value = placeUpdate.PlaceId;

                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService._placeUpdates.Add(placeUpdate);
            return placeUpdate;
        }

        public IDBEntity Get(int id)
        {
            return StaticService._placeUpdates.Find(placeUpdate => placeUpdate.Id == id);
        }

        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService._placeUpdates.FindAll(placeUpdate => true));
        }
    }
}
