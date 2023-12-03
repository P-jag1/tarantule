using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;


namespace DataAccess.Model
{
    public class TarantuleCommentOfUser : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Zadejte svoji přezdívku")]
        public virtual string Name { get; set; }
        
        public virtual string Time { get; set; }

        [AllowHtml]
        public virtual string Description{ get; set; }

        public virtual TarantuleOfUser UserTar_id { get; set; }

        public virtual int Extra { get; set; }

    }
}
