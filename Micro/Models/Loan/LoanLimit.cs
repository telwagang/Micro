using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models.Loan
{
    public class LoanLimit
    {
        [Key, ForeignKey("Company")]
        public int CompanyId { get; set; }
        public int limitAmount { get; set; }


        public virtual Company Company { get; set; }

       
    }
}