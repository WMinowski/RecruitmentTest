using System;
using System.Collections.Generic;
using System.Text;
using DomainStandard;

namespace Infrastructure
{
    interface ICommands
    {
        void Remove(int id);
        void Remove(IDBEntity entity);
        void Update(int id, IDBEntity entity);
        IDBEntity Create(IDBEntity entity);
    }
}
