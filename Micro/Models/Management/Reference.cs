using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models
{
    public class Reference
    {
        public int ID { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public DateTime date { get; set; }

        public virtual Customer Customer { get; set; }  
    }
}