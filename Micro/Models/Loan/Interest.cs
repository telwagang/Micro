using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models.Loan
{
    public class Interest
    {
        public int ID { get; set; }
        public int? CompanyId { get; set; }
        public int StaffId { get; set; }
        public int duration { get; set; }
        public double rate { get; set; }



        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }
}