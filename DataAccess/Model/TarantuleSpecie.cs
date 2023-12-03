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
    public class TarantuleSpecie : IEntity
    {
        public virtual int Id { get; set; }
        
        [Required(ErrorMessage = "Zadejte název sklípkana.")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Zadejte latinský název sklípkana.")]
        public virtual string LatinName { get; set; }

        [Required(ErrorMessage = "Zadejte průměrnou délku života sklípkana.")]
        [Range(1, 50, ErrorMessage = "Délka života se pohybuje od 1 do 50 let.")]
        public virtual int SpanOfLife { get; set; }

        [Required(ErrorMessage = "Zadejte velikost sklípkana v centimetrech.")]
        [Range(1, 100, ErrorMessage = "Velikost se pohybuje od 1 do 100 centimetrů.")]
        public virtual int Size { get; set; }

        [AllowHtml]
        public virtual string Description { get; set; }

        public virtual string ImageName { get; set; }

        [Required(ErrorMessage = "Zadejte země výskytu sklípkana.")]
        public virtual string Country { get; set; }
        
    }
}
