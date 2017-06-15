using System;
using System.Collections.Generic;
using Micro.Models.Loan;
using Micro.Models.Hisa;
using Micro.Models.akiba;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.Models
{
    public class Staff
    {

        public Staff()
        {
            loan = new List<Loan.Loan>();
            StartAkiba = new List<StartAkiba>();
            LoanApplicantion = new List<LoanApplicantion>();
            Customer = new List<Customer>();
            Interest = new List<Interest>();

        }
        public int ID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public int CompanyId { get; set; }
        public string  First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Position { get; set; }
        public DateTime birthdate { get; set; }
        public string email { get; set; }
        public int Mobile_Number { get; set; }
        public DateTime date { get; set; }

        
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public  virtual ICollection<Loan.Loan> loan { get; set; }
        public virtual ICollection<StartAkiba> StartAkiba { get; set; }
        public virtual ICollection<LoanApplicantion> LoanApplicantion { get; set; }
        public virtual ICollection<Reference> reference { get; set; }
        public virtual ICollection<HisaHistory> HisaHistory { get; set; }
        public virtual ICollection<Payment>  Payment { get; set; }
        public virtual ICollection<Interest> Interest { get; set; }
        public virtual ICollection<AkibaCT> AkibaCT { get; set; }
        public virtual ICollection<AkibaDT> AkibaDT { get; set; }
    }
}