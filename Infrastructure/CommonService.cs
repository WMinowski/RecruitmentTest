using DomainStandard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{

    public abstract class CommonService : ICommands
    {
        public abstract IDBEntity Create(IDBEntity entity);
        public abstract void Remove(int id);
        public abstract void Remove(IDBEntity entity);
        public abstract void Update(int id, IDBEntity entity);
    }
}
