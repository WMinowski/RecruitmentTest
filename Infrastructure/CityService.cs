using DomainStandard;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace Infrastructure
{
    public class CityService : IQueries
    {
        public CityService()
        {

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
