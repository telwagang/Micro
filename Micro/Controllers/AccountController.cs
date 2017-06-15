using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Micro.Models;
using Micro.Models.Management;
using Micro.DataLayer;
using System.Data.Entity;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Micro.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private MicroContext db = new MicroContext();



        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(returnUrl);
                //case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [HttpGet]
        public ActionResult mRegister()
        {
            return View();
        }

        // GET: /Account/Register
       
        [Authorize(Roles ="admin , developer")]
        public ActionResult Register()
        {
            string[] aroles = { "admin", "staff", "developer", "customer" };
            ViewBag.roles = aroles;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Onion() {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Onion(MasterUser mu ) {
            if (ModelState.IsValid)
            {
                if(mu.extystgha == StringRescource.MasterUser)
                {
                    var user = new ApplicationUser { UserName = mu.Username, Email = mu.email };
                    var result = await UserManager.CreateAsync(user, mu.Password);

                    if (result.Succeeded)
                    {
                        await UserManager.AddToRolesAsync(user.Id, "developer");
                        using (var db = new MicroContext())
                        {
                            db.Staff.Add(
                                   new Staff
                                   {First_Name = "joseph",
                                   UserID = user.Id,
                                       Middle_Name = "Deo",
                                       Last_Name = "telwa",
                                       birthdate = DateTime.Today,
                                       CompanyId = 1,
                                       Mobile_Number = 071478524,
                                       email = mu.email,
                                       Position = "developer",
                                       date = DateTime.Now
                                   });

                            await db.SaveChangesAsync();
                            return RedirectToAction("", "", "");
                        }
                       
                    }
                }
                
            }
            return View("Onion",mu);
        }
        // POST: /Account/Register staff
        [HttpPost]
        [Authorize(Roles = "admin , developer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = string.Format(model.First_name + " " + model.Last_name), Email = model.Email,PhoneNumber = model.Mobile_number.ToString() };

                var compid = await LogicalModel.getId(User.Identity.GetUserId());

                var staff = new Staff()
                {
                    First_Name = model.First_name,
                    Middle_Name = model.Middle_name,
                    Last_Name = model.Last_name,
                    email = model.Email,
                    Mobile_Number = model.Mobile_number,
                    Position = model.position,
                    date = DateTime.Now,
                    UserID = user.Id,
                    CompanyId = compid.Item2
                };



                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await UserManager.AddToRolesAsync(user.Id, model.position);
                    using (db)
                    {
                        db.Staff.Add(staff);
                        await db.SaveChangesAsync();
                    }

                    return RedirectToAction("", "", "");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
       

        // GET: /Account/Register
        [HttpGet]
        // [Authorize(Roles = "staff, admin, developer")]
        public ActionResult RegisterClient()
        {
            return View();
        }

        //post: /account/register/client
        [HttpPost]
       [Authorize(Roles = "staff, admin, developer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registerclient(RegisterClientViewModel cvm)
        {
            var ids = Guid.NewGuid().ToString("N").Substring(5, 6).ToUpper();
            if (ModelState.IsValid)
            {
                using (var db = new MicroContext())
                {
                    
                    var stafID = await LogicalModel.getId(User.Identity.GetUserId());

                    var compyID = stafID.Item2;

                    Customer cm = new Customer();
                    cm.CustomerId = ids;
                    cm.First_Name = cvm.First_name;
                    cm.Middle_Name = cvm.Middle_name;
                    cm.Last_Name = cvm.Last_name;
                    cm.national_id = cvm.national_id;
                    cm.Email = cvm.Email;
                    cm.Mobile_Number = cvm.Mobile_number;
                    cm.Nationality = cvm.Nationality;
                    cm.Street = cvm.Street;
                    cm.Ward = cvm.Ward;
                    cm.Division = cvm.Division;
                    cm.StaffId = stafID.Item1;
                    cm.date = cvm.date;
                    cm.CompanyId = compyID;
                    cm.Birthdate = cvm.Birthdate;

                    try
                    {
                        db.Customer.Add(cm);
                        db.SaveChanges();


                        return RedirectToAction("Akiba", "Transaction", new { key = ids });

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }

            }
            return View(cvm);
        }
        [HttpGet]
       [Authorize(Roles = "staff, admin, developer")]
        public ActionResult RegisterCompany()
        {
            int num;
            Random nu = new Random();
            num = nu.Next();
            RegisterCompanyViewModel rcm = new RegisterCompanyViewModel()
            {
                keyvalue = num
            };

            return View(rcm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
      [Authorize(Roles = "staff, admin, developer")]
        public ActionResult RegisterCompany(RegisterCompanyViewModel rcv)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            using (db)
            {
                Company cm = new Company()
                {
                    Name = rcv.name,
                    Email = rcv.Email,
                    Address = rcv.Address,
                    Tin_no = rcv.Tin_no,
                    KeyValue = rcv.keyvalue,
                    location = rcv.Location,
                    MobileNumber = rcv.Mobile_number,
                    date = DateTime.Now
                   

                };

                db.Company.Add(cm);
                db.SaveChanges();

                return RedirectToAction("mRegister");
            }


        }

      
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //GET:   /ACCOUNT/MANAGE USERS 
        [HttpGet]
        public ActionResult ManageUsers()
        {

            return View();
        }
        //GET: /ACCOUNT/VIEW STAFF
        [HttpGet]
        public async Task< ActionResult> ViewStaff()
        {
            var data = await LogicalModel.getId(User.Identity.GetUserId());

            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<Staff, RegisterStaffViewModel>()
                  .ForMember(c => c.StaffId, map => map.MapFrom(p => p.ID.ToString()));
            });

            var list = await new LogicalModel(config).TableToList(data.Item2.ToString(), 2);

            return View(list);
        }

        // GET: /ACCOUNT/VIEW CUSTOMER 
        [HttpGet]
        public async Task<ActionResult> ViewCustomer()
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<Customer, RegisterClientViewModel>();
            });

            var data = await LogicalModel.getId(User.Identity.GetUserId());
            var list = await  new MicroContext().Customer.Where(c => c.CompanyId == data.Item2).ProjectTo<RegisterClientViewModel>(config).ToListAsync();

            return View(list);
        } 
        
        //GET: /Account.EditCustumer
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> EditCustomer(string key)
        {
            if (key != null & key.Length != 1)
            {
                
              
                var full_user = await new MicroContext().Customer.Where(c => c.CustomerId == key).FirstOrDefaultAsync();

                return View("EditCustomer", full_user);
                
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "staff, admin, developer")]
        public async Task<ActionResult> EditCustomer(Customer cvm)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MicroContext())
                {
                    db.Entry(cvm).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return RedirectToAction(nameof(AccountController.ManageUsers), "Account");


            }

            return View("EditCustomer", cvm);
        }

        //POST: /ACCOUNT/DELETE CUSTOMER
        [HttpGet]
        [Authorize(Roles = "staff, admin, developer")]
        public async Task<ActionResult> DeleteCustomer(string key)
        {
            if (key != null & key.Length != 1)
            {
                using (var db = new MicroContext())
                {
                    db.Entry(new Customer() { CustomerId = key }).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                }
                
            }
            return RedirectToAction(nameof(ViewCustomer));
        }
        [HttpGet]
        public async Task<ActionResult> EditStaff(string key)
        {
            if (key != null & key.Length != 1)
            {

                var keys = (Int32)Convert.ToInt32(key);

                var full_user = await new MicroContext().Staff.Where(c => c.ID == keys).FirstOrDefaultAsync();

                return View("EditStaff", full_user);

            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        //POST: /ACCOUNT/EDIT STAFF
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStaff(Staff st)
        {

            if (ModelState.IsValid)
            {
                using (var db = new MicroContext())
                {
                    db.Entry(st).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return RedirectToAction(nameof(ViewStaff));
                
            }

            return View("Register",st);
        }

        //POST: /ACCOUNT/DELETE STAFF
        [HttpGet]
        [Authorize(Roles = "admin, developer")]
        public async Task<ActionResult> DeleteStaff(string key)
        {

            if (key != null & key.Length != 1)
            {
                int CVM = (int)Convert.ChangeType(key ?? "0", typeof(int));

                using (var db = new MicroContext())
                {
                    db.Entry(new Staff() { ID = CVM }).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                }

            }
            return RedirectToAction(nameof(ViewStaff));
          
        }
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}