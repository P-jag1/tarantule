using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class TarantuleCommentOfUserDao :DaoBase<TarantuleCommentOfUser>
    {
        public TarantuleCommentOfUserDao() : base()
        {

        }

        public IList<TarantuleCommentOfUser> GetTarantulaCommentOfUserPaged(int count, int page, out int totalTarantuleUser)
        {
            totalTarantuleUser = session.CreateCriteria<TarantuleCommentOfUser>()
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>();

            return session.CreateCriteria<TarantuleCommentOfUser>()
                .SetFirstResult((page - 1) * count)
                .SetMaxResults(count)
                .List<TarantuleCommentOfUser>();
        }

        public IList<TarantuleCommentOfUser> SearchTarantulaCommentOfUsers(string phrase)
        {
            return session.CreateCriteria<TarantuleCommentOfUser>()
                .Add(Restrictions.Like("Name", string.Format("%{0}%", phrase)))
                .List<TarantuleCommentOfUser>();
        } 
    }
}
