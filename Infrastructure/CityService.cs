using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure
{
    public class CityService : ICRUD
    {
        public CityService()
        {

        }

        public void Remove(int id)
        {
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.cities WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService._cities.Remove(StaticService._cities.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            City cityIn = (City)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new MySqlCommand("DELETE FROM mydb.cities WHERE (Id = " + cityIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService._cities.Remove(cityIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            City cityIn = (City)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE cities SET Id =" + cityIn.Id + " ,City = '" + cityIn.Name +
                            @" WHERE (" +
                            @"Id = " + cityIn.Id + @")";

                MySqlDataAdapter localAadapter = new MySqlDataAdapter();

                localAadapter.UpdateCommand = new MySqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            StaticService._cities.Remove(StaticService._cities.Find(city => city.Id == id));
            StaticService._cities.Add(cityIn);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            City city = (City)entity;
            using (MySqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new MySqlCommand(
                                      "INSERT INTO mydb.cities " +
                                      "VALUES(@Id,@City);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", MySqlDbType.Int32).Value = city.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@City", MySqlDbType.VarChar).Value = city.Name;


                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService._cities.Add(city);
            return city;
        }

        public IDBEntity Get(int id)
        {
            return StaticService._cities.Find(city => city.Id == id);
        }

        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService._cities.FindAll(city => true));
        }
    }
}
