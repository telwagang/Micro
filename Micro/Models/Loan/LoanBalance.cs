using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models.Loan
{
    public class LoanBalance
    {
        [Key,ForeignKey("Loan")]
        public string LoanId { get; set; }
        public int balance { get; set; }

       
        public virtual Loan Loan { get; set; }
    }
}