using Micro.DataLayer;
using Micro.Interfaces;
using Micro.Models;
using Micro.Models.Loan;
using Micro.Models.Management;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Micro.Controllers
{
    public class SetupController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IRepositoryinterest _interest;
        private MicroContext db;
        private int companyID;



        public SetupController()
        {
            this._interest = new IRepositoryinterest();
            this.db = new MicroContext();
        }

        public SetupController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IRepositoryinterest interest, MicroContext Db)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this._interest = interest;
            this.db = Db;
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

        
        // GET: Setup
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="developer")]
        [HttpGet]
        public ActionResult SetupCompany()
        {
            return View("RegisterCompany",new RegisterCompanyViewModel() { keyvalue = new Random().Next() });
        }

        [Authorize(Roles = "developer")]
        [HttpPost]
        public async Task<ActionResult> SetupCompany(RegisterCompanyViewModel rcv)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (db)
            {
                db.Company.Add(new Company()
                {
                    Name = rcv.name,
                    Email = rcv.Email,
                    Address = rcv.Address,
                    Tin_no = rcv.Tin_no,
                    KeyValue = rcv.keyvalue,
                    location = rcv.Location,
                    MobileNumber = rcv.Mobile_number,
                    date = DateTime.Now
                });
                await db.SaveChangesAsync();

                companyID = await (from s in db.Company where s.KeyValue == rcv.keyvalue select s.CompanyId).FirstOrDefaultAsync();
                return View(StringRescource.Regi_company);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult SetupUser()
        {
            return View(StringRescource.Regi_staff);
        }

        [HttpPost]
        [Authorize(Roles = "developer")]
        public async Task<ActionResult> SetupUser(RegisterStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = string.Format(model.First_name + " " + model.Last_name), Email = model.Email, PhoneNumber = model.Mobile_number.ToString() };
                
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
                    CompanyId = companyID
                };
                
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                { 

                    await UserManager.AddToRolesAsync(user.Id, model.position);
                    using ( db )
                    {
                        db.Staff.Add(staff);
                        await db.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(SetInterest));
                }
                AddErrors(result);
            }

            return View(StringRescource.Regi_staff, model);
        }
        [HttpGet]
        [Authorize]
        public ActionResult SetInterest()
        {
            return View(StringRescource.Add_interest);
        }
        [Authorize]
        [HttpPost]
        public ActionResult setInterent(Interest inter)
        {
            if (ModelState.IsValid)
            {
                _interest.addInterest(inter);
                _interest.save();
                return RedirectToAction("","");
            }
            return View(StringRescource.Add_interest, inter);
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}