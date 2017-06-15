using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Micro.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }


    public class MasterUser
    {
        public string email { get; set; }
        public string Username  { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string extystgha { get; set; }
    }
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
       
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterStaffViewModel 
    {

       
        public string StaffId { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string First_name { get; set; }

        [Required]
        [Display(Name = "Middle name")]
        public string Middle_name { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string Last_name { get; set; }

        [Required]
        [Display(Name = "position")]
        public string position { get; set; }
        [Display(Name ="Date Of Birth")]
        public DateTime Birthdate { get; set; }
        
        [Display(Name = "Mobile Number")]
        public int Mobile_number { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6 )]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class RegisterClientViewModel {

        public string CustomerId { get; set; }
        [Display(Name = "National ID")]
        public int national_id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string First_name { get; set; }

        [Required]
        [Display(Name = "Middle name")]
        public string Middle_name { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string Last_name { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        [Display(Name = "Occupution")]
        public string occupution { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public int Mobile_number { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

      
        [Display(Name = "Ward")]
        public string Ward { get; set; }

        [Required]
        [Display(Name = "Division")]
        public string Division { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

    }
    public class RegisterCompanyViewModel
    {
       
        [Display(Name = "Tin Number")]
        public int Tin_no { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

       
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public int Mobile_number { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

       
        [Display(Name = "Key Value")]
        [Editable(false)]
        public int keyvalue { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime  date { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
