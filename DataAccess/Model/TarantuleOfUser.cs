﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;
namespace DataAccess.Model
{
    public class TarantuleOfUser : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Zadejte název sklípkana.")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Zadejte přezdívku sklípkana")]
        public virtual string Nickname { get; set; }

        [AllowHtml]
        public virtual string Description { get; set; }

        public virtual TarantuleUser User_id { get; set; }

        public virtual string ImageName { get; set; }

    }
}
