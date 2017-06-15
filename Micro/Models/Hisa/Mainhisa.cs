using Micro.DataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.Models.Hisa
{
    [Table(nameof(MicroContext.Mainhisa))]
    public class MainHisa
    {
        public MainHisa()
        {
            HisaHistory = new List<HisaHistory>();
        }

     
        [Key,ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public int NoHisa { get; set; }
        public int amount { get; set; }
        public DateTime date { get; set; }

        public  Customer Customer { get; set; }
        public virtual ICollection<HisaHistory> HisaHistory { get; set; }
    }
}