using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class TarantuleSpecieDao : DaoBase<TarantuleSpecie>
    {
        public TarantuleSpecieDao() : base()
        {

        }

        public IList<TarantuleSpecie> GetByCountry(string country)
        {
            return session.CreateCriteria<TarantuleSpecie>()
                .Add(Restrictions.Eq("Country", country))
                .List<TarantuleSpecie>();

        }


        public IList<TarantuleSpecie> GetTarantulaPaged(int count, int page, out int totalTarantulaSpecies) 
        {
            totalTarantulaSpecies = session.CreateCriteria<TarantuleSpecie>()
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>();

            return session.CreateCriteria<TarantuleSpecie>()
                .AddOrder(Order.Asc("Name"))
                .SetFirstResult((page - 1) * count)
                .SetMaxResults(count)
                .List<TarantuleSpecie>();
        }

        public IList<TarantuleSpecie> SearchTarantulaSpecies(string phrase)
        {
            return session.CreateCriteria<TarantuleSpecie>()
                .Add(Restrictions.Like("Name", string.Format("%{0}%", phrase)))
                .List<TarantuleSpecie>();
        }       
    }
}
