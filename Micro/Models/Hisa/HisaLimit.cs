using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models.Hisa
{
    public class HisaLimit
    {
        [Key, ForeignKey("Company")]
        public int CompanyId { get; set; }
        public int Hisa { get; set; }
        public int amount { get; set; }

        public virtual Company Company { get; set; }
    }
}