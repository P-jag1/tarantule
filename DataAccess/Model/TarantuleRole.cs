using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Model
{
    public class TarantuleRole : IEntity
    {
        public virtual int Id { get; set; }

        public virtual string Identificator { get; set; }

        public virtual string RoleDescription { get; set; }
    }
}
