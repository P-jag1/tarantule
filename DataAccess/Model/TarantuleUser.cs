using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using DataAccess.Interface;
using NHibernate.Util;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
    public class TarantuleUser : MembershipUser, IEntity
    {

        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Zadejte jméno uživatele")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Zadejte příjmení uživatele")]
        public virtual string Surname { get; set; }

        [Required(ErrorMessage = "Zadejte login uživatele")]
        public virtual string Login { get; set; }

        [Required(ErrorMessage = "Zadejte heslo uživatele")]
        public virtual string Password { get; set; }

        [Required(ErrorMessage = "Zadejte email uživatele")]
        public virtual string Email { get; set; }

        
        public virtual int Telefon { get; set; }

        public virtual TarantuleRole Role { get; set; }

    }
}
