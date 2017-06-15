using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.Models.Hisa
{
    public class HisaHistory
    {
        public int ID { get; set; }
        [ForeignKey("hisa")]
        public string hisaId { get; set; }
        public int StaffId { get; set; }
        public int Hisa { get; set; }
        public int amount { get; set; }
        public DateTime date { get; set; }


        public virtual MainHisa hisa { get; set; }
    }
}