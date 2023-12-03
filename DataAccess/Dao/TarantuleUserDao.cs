using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class TarantuleUserDao : DaoBase<TarantuleUser>
    {
        public TarantuleUser GetByLoginAndPassword(string login, string password)
        {
           return session.CreateCriteria<TarantuleUser>()
                .Add(Restrictions.Eq("Login", login))
                .Add(Restrictions.Eq("Password", password))
                .UniqueResult<TarantuleUser>();
            
        }

        public TarantuleUser GetByLogin(string login)
        {
            return session.CreateCriteria<TarantuleUser>()
                 .Add(Restrictions.Eq("Login", login))
                 .UniqueResult<TarantuleUser>();

        }

        public IList<TarantuleUser> GetUserInRoleId(int id)
        {
            return session.CreateCriteria<TarantuleUser>()
                .CreateAlias("Role", "rol")
                .Add(Restrictions.Eq("rol.Id", id))
                .List<TarantuleUser>();
        }

        public IList<TarantuleUser> GetUserPaged(int count, int page, out int totalUsers)
        {
            totalUsers = session.CreateCriteria<TarantuleUser>()
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>();

            return session.CreateCriteria<TarantuleUser>()
                .AddOrder(Order.Asc("Role"))
                .SetFirstResult((page - 1) * count)
                .SetMaxResults(count)
                .List<TarantuleUser>();
        }

        public IList<TarantuleUser> SearchUsers(string phrase)
        {
            return session.CreateCriteria<TarantuleUser>()
                .Add(Restrictions.Like("Login", string.Format("%{0}%", phrase)))
                .List<TarantuleUser>();
        }   
    }
}
