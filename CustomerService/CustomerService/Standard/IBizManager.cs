using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Standard
{
    public interface IBizManager<TEntity>
        where TEntity:class
    {
        IList<TEntity> GetAll();
        TEntity GetById(string id);
        void Add(TEntity entity);
        bool DeleteById(string id);
        bool UpdateById(String id, TEntity entity);
    }
}
