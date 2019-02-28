using DomainStandard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    interface IQueries
    {
        IDBEntity Get(int id);
        List<IDBEntity> Get();
    }
}
