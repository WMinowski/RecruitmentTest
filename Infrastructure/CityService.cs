using DomainStandard;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    public class CityService : ICRUD
    {
        public CityService()
        {

        }

        public void Remove(int id)
        {
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SqlCommand("DELETE FROM mydb.cities WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.cities.Remove(StaticService.cities.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            City cityIn = (City)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SqlCommand("DELETE FROM mydb.cities WHERE (Id = " + cityIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.cities.Remove(cityIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            City cityIn = (City)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE mydb.cities SET City = '" + cityIn.Name +
                            @" WHERE (" +
                            @"Id = " + cityIn.Id + @")";

                SqlDataAdapter localAadapter = new SqlDataAdapter();

                localAadapter.UpdateCommand = new SqlCommand(query, connection);
                connection.Open();
                localAadapter.UpdateCommand.ExecuteNonQuery();
            }
            //
            StaticService.cities.Remove(StaticService.cities.Find(city => city.Id == id));
            StaticService.cities.Add(cityIn);
        }

        public IDBEntity Create(IDBEntity entity)
        {
            City city = (City)entity;
            using (SqlConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new SqlCommand(
                                      "INSERT INTO mydb.cities " +
                                      "VALUES(@City);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@City", SqlDbType.VarChar).Value = city.Name;


                connection.Open();
                StaticService.adapter.InsertCommand.ExecuteNonQuery();
            }
            StaticService.cities.Add(city);
            return city;
        }

        public IDBEntity Get(int id)
        {
            return StaticService.cities.Find(city => city.Id == id);
        }

        public List<IDBEntity> Get()
        {
            return new List<IDBEntity>(StaticService.cities.FindAll(city => true));
        }
    }
}
