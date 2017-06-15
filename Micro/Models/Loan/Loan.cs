using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.Models.Loan
{
    public class Loan
    {
        public Loan()
        {
            LoanStatus = new List<LoanStatus>();
            payments = new List<Payment>();
        }

       [Key]
        public string LoanId { get; set; }
        public string CustomerId { get; set; }
        public int StaffId { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public int returnAmount { get; set; }
        public DateTime datesumbit { get; set; }


        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        public virtual LoanApplicantion LoanApplication { get; set; }
        public virtual ICollection<Payment> payments { get; set; }
        public virtual ICollection<LoanStatus> LoanStatus { get; set; }
        public virtual LoanDone LoanDone { get; set; }
        public virtual LoanBalance LoanBalance { get; set; }
    }
}