using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class PlaceService : ICommands, IQueries
    {
        public PlaceService()
        {

        }

        public void Remove(int id)
        {
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.places WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.places.Remove(StaticService.places.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            Place placeIn = (Place)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.places WHERE (Id = " + placeIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.places.Remove(placeIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            Place placeIn = (Place)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE places SET Id =" + placeIn.Id + " ,CityId = '" + placeIn.City.Id + "' ,Street = '"+ placeIn.Street +
                            @"' WHERE (" +
                            @"Id = " + placeIn.Id + @")";

                MySqlDataAdapter localAadapter = new MySqlDataAdapter();

                localAadapter.UpdateCommand = new MySqlCommand(query, connection);
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
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.places " +
                                      "VALUES(@Id,@CityId,@Street);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = place.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@CityId", MySqlDbType.Int32).Value = place.City.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@Street", MySqlDbType.VarChar).Value = place.Street;


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
