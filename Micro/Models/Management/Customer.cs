using System.Collections.Generic;
using Micro.Models.Loan;
using Micro.Models;
using Micro.Models.Hisa;
using Micro.Models.akiba;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Micro.Models.Management;

namespace Micro.Models

{
    public class Customer
    {
        public Customer() {

            loan = new List<Loan.Loan>();
            Reference = new List<Reference>();
            StartAkiba = new List<StartAkiba>();
            memberaddmission = new List<MemberAddmission>();

        }

       [Key]
        public string CustomerId { get; set; }
        public int national_id { get; set; }
        public int StaffId { get; set; }
        public int? CompanyId { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public int Mobile_Number { get; set; }
        public DateTime Birthdate { get; set; }
        public string occuption { get; set; }
        public string Email { get; set; }
        public string Nationality  { get; set; }
        public string Ward { get; set; }
        public string Division { get; set; }
        public string  Street { get; set; }
        public DateTime date { get; set; }


        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        public virtual ICollection<Loan.Loan> loan { get; set; }
        public  ICollection< StartAkiba> StartAkiba { get; set; }
        public virtual ICollection<Reference> Reference { get; set; }
        public virtual ICollection<MemberAddmission> memberaddmission { get; set; }
        public  hisa Hisa { get; set; }
    }
}