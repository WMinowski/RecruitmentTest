using DomainStandard;
using System.Data.SQLite;
using System.Collections.Generic;


namespace Infrastructure
{
    public class CityService : ICRUD
    {
        public CityService()
        {

        }

        public void Remove(int id)
        {
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.cities WHERE (Id = " + id.ToString() + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            StaticService.cities.Remove(StaticService.cities.Find(x => x.Id == id));
        }

        public void Remove(IDBEntity entity)
        {
            City cityIn = (City)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.DeleteCommand =
                                      new SQLiteCommand("DELETE FROM mydb.cities WHERE (Id = " + cityIn.Id + ");", connection);
                connection.Open();
                StaticService.adapter.DeleteCommand.ExecuteNonQuery();
            }
            //
            StaticService.cities.Remove(cityIn);
        }

        public void Update(int id, IDBEntity entity)
        {
            City cityIn = (City)entity;
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                string query =
                            @"UPDATE cities SET Id =" + cityIn.Id + " ,City = '" + cityIn.Name +
                            @" WHERE (" +
                            @"Id = " + cityIn.Id + @")";

                SQLiteDataAdapter localAadapter = new SQLiteDataAdapter();

                localAadapter.UpdateCommand = new SQLiteCommand(query, connection);
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
            using (SQLiteConnection connection = StaticService.GetConnection())
            {
                StaticService.adapter.InsertCommand = new SQLiteCommand(
                                      "INSERT INTO mydb.cities " +
                                      "VALUES(@Id,@City);",
                                      connection);
                StaticService.adapter.InsertCommand.Parameters.Add("@Id", System.Data.DbType.Int32).Value = city.Id;
                StaticService.adapter.InsertCommand.Parameters.Add("@City", System.Data.DbType.String).Value = city.Name;


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
