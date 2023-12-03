using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class TarantuleSellDao : DaoBase<TarantuleSell>
    {
        public TarantuleSellDao() : base()
        {
        }


        public IList<TarantuleSell> GetTarantulaSellPaged(int count, int page, out int totalTarantulaSells) 
        {
            totalTarantulaSells = session.CreateCriteria<TarantuleSell>()
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>();

            return session.CreateCriteria<TarantuleSell>()
                .AddOrder(Order.Asc("Name"))
                .SetFirstResult((page - 1) * count)
                .SetMaxResults(count)
                .List<TarantuleSell>();
        }

        public IList<TarantuleSell> SearchTarantulaSells(string phrase)
        {
            return session.CreateCriteria<TarantuleSell>()
                .Add(Restrictions.Like("Name", string.Format("%{0}%", phrase)))
                .List<TarantuleSell>();
        } 

    }
}
