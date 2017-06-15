using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.Models.Loan
{
    public class LoanStatus
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Loan")]
        public string LoanId { get; set; }
        public int monthly { get; set; }
        public DateTime Nextpayday { get; set; }


        public virtual Loan Loan { get; set; }
    }
}