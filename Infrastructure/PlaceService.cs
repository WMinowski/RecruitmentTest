using DomainStandard;
using MySql.Data.MySqlClient;
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
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.places WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService._places.Remove(StaticService._places.Find(x => x.Id == id));
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
            StaticService._places.Remove(placeIn);
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
            StaticService._places.Remove(StaticService._places.Find(city => city.Id == id));
            StaticService._places.Add(placeIn);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            Place place = (Place)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.places " +
                                      "VALUES(@Id,@City,@Street);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = place.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@CityId", MySqlDbType.Int32).Value = place.City.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@Street", MySqlDbType.VarChar).Value = place.Street;


                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService._places.Add(place);
            return place;
        }

        public IDBEntity Get(int id)
        {
            return StaticService._places.Find(place => place.Id == id);
        }

        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService._places.FindAll(place => true));
        }
    }
}
