using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Micro.Models
{
    public class ViewHelperModel
    {
        public virtual IEnumerable<LoanViewModel> lv { get; set; }
        public virtual IEnumerable<Customer> customer{ get; set; }

    }
    public class searchViewModel
    {
        public virtual IEnumerable<searchloanviewmodel> loanlist { get; set; }
        public virtual IEnumerable<Customer> customer { get; set; }

    }
    public class LoanViewModel
    {
        [Display(Name = "Customer Name")]
        public string customerName { get; set; }
        
        public string CustomerId { get; set; } 

        [Display(Name = "Loan ID")]
        public string loanId { get; set; }

        [Display(Name = "Staff Submitted")]
        public string By { get; set; }

        [Display(Name = "Amount")]
        public int amount { get; set; }

        [Display(Name ="Duration")]
        public int duration { get; set; }

        [Display(Name = "Interest")]
        [DisplayFormat(DataFormatString = "{0:#\\%}")]
        public double interest { get; set; }

        [Display(Name = "Date submitted")]
        [DataType(DataType.Date)]
        public DateTime DateIn { get; set; }

        
    }
    public class createLoginView
    {

        [Display(Name = "Customer Name")]
        public string customerName { get; set; }
        [Display(Name = "Customer ID")]
        public string CustomerId { get; set; }
        [Display(Name ="Duration")]
        public int duration { get; set; }
        [Display(Name = "Amount")]
        public int amount { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }


    }
    public class searchloanviewmodel
    {
        public string loanid { get; set; }
        [Display(Name = "First Name")]
        public string First_name { get; set; }
        [Display(Name = "Middle Name")]
        public string Middle_name { get; set; }
        [Display(Name = "Last Name")]
        public string Last_name { get; set; }
        [Display(Name = "Customer ID")]
        public string CustomerId { get; set; }
        [Display(Name = "Duration")]
        public int duration { get; set; }
        [Display(Name = "Amount Taken")]
        public int amount { get; set; }
        [Display(Name = "Amount return")]
        public int? returnAmount { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
    }

    public class usersummarymodel
    {
        public string customerid { get; set; }
        public string username { get; set; }
        public DateTime? birthday { get; set; }
        public int age { get; set; }
        public string nationality { get; set; }
        public int phone_number { get; set; }
        public string address { get; set; }
        public DateTime date { get; set; }
        public string  akibaid { get; set; }
        public int Akiba_balance { get; set; }
        public string loanid { get; set; }
        public int? loan_balance { get; set; }
        public int? amount_taken { get; set; }
        public int? return_amount { get; set; }
        public int no_hisa { get; set; }
        public int amount_hisa { get; set; }
    }
    class Tempdata
    {
        public int staffId { get; set; }
        public int companyid { get; set; } 
    }
}