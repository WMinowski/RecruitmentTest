using DomainStandard;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class PlaceService : ICRUD
    {
        public PlaceService()
        {

        }

        public void Remove(int id)
        {
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.places WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.places.Remove(StaticService.places.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            Place placeIn = (Place)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.places WHERE (Id = " + placeIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.places.Remove(placeIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            Place placeIn = (Place)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE places SET Id =" + placeIn.Id + " ,CityId = '" + placeIn.City.Id + "' ,Street = '"+ placeIn.Street +
                            @"' WHERE (" +
                            @"Id = " + placeIn.Id + @")";

                SQLiteDataAdapter localAadapter = new SQLiteDataAdapter();

                localAadapter.UpdateCommand = new SQLiteCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            StaticService.places.Remove(StaticService.places.Find(city => city.Id == id));
            StaticService.places.Add(placeIn);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            Place place = (Place)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new SQLiteCommand(
                                      "INSERT INTO mydb.places " +
                                      "VALUES(@Id,@CityId,@Street);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", System.Data.DbType.Int32).Value = place.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@CityId", System.Data.DbType.Int32).Value = place.City.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@Street", System.Data.DbType.String).Value = place.Street;


                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService.places.Add(place);
            return place;
        }

        public IDBEntity Get(int id)
        {
            return StaticService.places.Find(place => place.Id == id);
        }

        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService.places.FindAll(place => true));
        }
    }
}
