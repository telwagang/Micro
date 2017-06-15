using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.Models.Loan
{
    public class LoanApplicantion
    {
        [Key, ForeignKey("Loan")]
        public string LoanId { get; set; }
        public int StaffId  { get; set; }
        public bool Approved { get; set; }
        public DateTime date { get; set; }
       
        
        public virtual Loan Loan { get; set; }
        public virtual Staff Staff { get; set; }
    }
}