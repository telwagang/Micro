using Micro.Models.akiba;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Models
{
    public class Akiba
    {
        public Akiba()
        {
            AkibaCT = new List<AkibaCT>();
            AkibaDT = new List<AkibaDT>();
        }
        
        public int ID { get; set; }
        [Key,ForeignKey("StartAkiba")]
        public string AkibaId { get; set; }
      
        public int Amount { get; set; }
        
        public  StartAkiba StartAkiba { get; set; }
        public virtual ICollection<AkibaCT> AkibaCT { get; set; }
        public virtual ICollection<AkibaDT> AkibaDT { get; set; }

        
    }
}