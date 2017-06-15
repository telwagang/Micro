using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models.akiba
{
    public class StartAkiba
    {
        [Key]  
        public string StartAkibaId { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public int Amount { get; set; }
        public DateTime Insert_date { get; set; }


        public  Customer Customer { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Akiba Akiba { get; set; }
    }
}