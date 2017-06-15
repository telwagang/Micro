using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models.Loan
{
    public class Payment
    {
        
        public int ID { get; set; }
        [ForeignKey("Loan")]
        public string LoanId { get; set; }
        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public int AmountPaid  { get; set; }
        public DateTime date { get; set; }


        public virtual Staff Staff { get; set; }
        public virtual Loan Loan { get; set; }
    }
}