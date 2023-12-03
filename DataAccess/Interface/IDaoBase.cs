using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IDaoBase<T>
    {
        IList<T> GetAll();
        object Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(int id);
    }
}
