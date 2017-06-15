using Micro.Models.Hisa;
using Micro.Models.Loan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Micro.Models
{
    public class Company
    {
        public Company()
        {
            Staff = new List<Staff>();
            Customer = new List<Customer>();
            Interest = new List<Interest>();
        }


        [Key]
        public int CompanyId { get; set; }
        public int Tin_no { get; set; }
        public  string Name { get; set; }
        public string location { get; set; }
        public string Address  { get; set; }
        public string Email { get; set; }
        public int MobileNumber { get; set; }
        public DateTime date { get; set; }
        public int  KeyValue { get; set; }


        public virtual ICollection<Staff> Staff { get; set; }
        public virtual ICollection<Customer> Customer{ get; set; }
        public virtual ICollection<Interest> Interest { get; set; }
        public virtual HisaLimit Hisalimit { get; set; }
        public virtual LoanLimit loanLimit {get; set;}
    }
}