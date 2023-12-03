using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class TarantuleOfUserDao : DaoBase<TarantuleOfUser>
    {
        public TarantuleOfUserDao() : base()
        {
        }


        public IList<TarantuleOfUser> GetTarantulaOfUserPaged(int count, int page, out int totalTarantuleUser) 
        {
            totalTarantuleUser = session.CreateCriteria<TarantuleOfUser>()
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>();

            return session.CreateCriteria<TarantuleOfUser>()
                .AddOrder(Order.Asc("Name"))
                .SetFirstResult((page - 1) * count)
                .SetMaxResults(count)
                .List<TarantuleOfUser>();
        }

        public IList<TarantuleOfUser> SearchTarantulaOfUsers(string phrase)
        {
            return session.CreateCriteria<TarantuleOfUser>()
                .Add(Restrictions.Like("Name", string.Format("%{0}%", phrase)))
                .List<TarantuleOfUser>();
        } 

    }
    
}
