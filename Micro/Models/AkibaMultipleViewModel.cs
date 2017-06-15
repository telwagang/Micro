using Micro.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Micro.Models
{
    public class AkibaMultipleViewModel
    {
        [Display(Name ="Customer Name")]
        public string  Customer_name { get; set; }
        public IEnumerable<activityViewModel> AkibaCT { get; set; }
        public IEnumerable<activityViewModel> AkibaDT { get; set; }

    }
    public class akibaViewModel
    {


        [Required]
        [Display(Name = "Customer name")]
        public string customer_name { get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AkibaId { get; set; }
        [Required]
        [Display(Name = "Select One ")]
        public string ctORdt { get; set; }

        [Display(Name = "Amount")]
        public int Amount { get; set; }



    }
    public class hisaViewModel
    {
        [Display(Name = "Amount of shares")]
        [ReadOnly(true)]
        public int NoHisa { get; set; }

        [Display(Name = "Totaly cash in hand")]
        public int amount { get; set; }

        [Timestamp]
        public DateTime date { get; set; }

    }
    public class paymentViewModel
    {
        [Display(Name = "Customer Name")]
        public string customer_name { get; set; }

        [Display(Name = "Loan ID")]
        public string loanId { get; set; }
        public int staffId { get; set;  }

        [Display(Name = "Amount ")]
        public int amountPaid { get; set; }

    }

    public class name
    {
        public string Fullname { get; set; }
    }

    public class activityViewModel
    {
        [Display(Name="No")]
        public int number { get; set; }
        [Display(Name ="Customer Name")]
        public string Customer_name { get; set; }

        [Display(Name ="Akiba ID")]
        public string  akiba_id { get; set; }

        [Display(Name ="Staff Added")]
        public string  staff { get; set; }
        
        [Display(Name ="Amount")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public int amount { get; set; }

        [Display(Name ="Date Add")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
    }

    public class nextpaydayModel
    {
        [Display(Name = "No")]
        public int number { get; set; }

        [Display(Name = "Customer Name")]
        public string Customer_name { get; set; }

        [Display(Name = "Last Desposite Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime LDD { get; set; }

        [Display(Name = "Last Desposite Amount")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public int LDA { get; set; }

        [Display(Name = "Next Desposite Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime NDD { get; set; }

        [Display(Name = "Next Desposite Amount")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public int NDA { get; set; }
    }

    public class interest
    {
        [Display(Name = "Duration in months")]
        public int duration { get; set; }
        [Display(Name = "Rate in %")]
        public double rate { get; set; }
    }
    public class hisa
    {
        [Display(Name = "Customer name")]
        public string Customer_name { get; set; }

        [Display(Name = "User Id ")]
        public string id { get; set; }

        [Display(Name = "Rate in %")]
        public double rate { get; set; }
    }
    public class proccesloan
    {
        public int bank_amount {get; set;}
        public int loan_amount { get; set; }
        public double rate { get; set; }
        public int returnamount { get; set; }
        public int duration { get; set; }
        public string loanid { get; set; }
    }
}