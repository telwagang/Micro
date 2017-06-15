using Micro.DataLayer;
using Micro.Models;
using Micro.Models.akiba;
using Micro.Models.Hisa;
using Micro.Models.Loan;
using Micro.Models.Management;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Micro.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        LogicalModel lm = new LogicalModel();
        
      

        //GET : Akiba main
        public ActionResult AkibaMain()
        {
           
            return View();
        }
        //GET : open akiba account
        [HttpGet]
        public ActionResult Akiba(string key)
        {
            if (key != null)
            {
                return View("AkibaCreat", new StartAkiba() { CustomerId = key });
            }

            return View("AkibaCreat");
        }
        //GET :    deposite
        [HttpGet]
        [Authorize(Roles = "staff, developer")]
        public ActionResult ToAkiba(string key, string name)
        {
            if (key != null )
            {
                return View("Akibatransca",new akibaViewModel { AkibaId = key ,customer_name = name});
            }
            return View("Akibatransca");
        }

        //POST : Open account details
        // [Authorize(Roles ="staff, developer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Akiba(StartAkiba A)
        {

            A.Insert_date = DateTime.Now;
            var item = await LogicalModel.getId(User.Identity.GetUserId());
            A.StaffId = item.Item1;

            A.StartAkibaId = Guid.NewGuid().ToString("D").Substring(8, 8).ToUpper();

            if (ModelState.IsValid)
            {
                if (LogicalModel.newAkiba(A))
                {
                    return RedirectToAction("AkibaMain");
                }
            }
            return View(A);
        }

        // POST: to deposite 
        [Authorize(Roles = "staff, developer")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> ToAkiba(akibaViewModel ct)
        {

            if (ModelState.IsValid)
            {
                var item = await LogicalModel.getId(User.Identity.GetUserId());
                if (LogicalModel.topDispote(ct, item.Item1))
                {
                    return RedirectToAction("AkibaMain");
                }
            }
            return View("Akibatransca",ct);
        }
        public async Task<ActionResult> Activity(string key,string name )
        {
            var data = await LogicalModel.getActivity(key, User.Identity.GetUserId(),name);

            if (data.Item2)
            {
                //UpdateModel()
                return View("akiba", data.Item1);
            }//UpdateModel()
                return View("akiba", data.Item1);
           

        }



        //.............................................................
        //GET : loan
        [HttpGet]
        public ActionResult Loan()
        {
            return View("MainLoan");
        }
        // GET : approve loan 
        [HttpGet]
        public async Task<ActionResult> approveLoan(string id)
        {
            if (Request.IsAjaxRequest())
            {

                var dataCustomer = await new LogicalModel().TableToList(id, 1);
                return PartialView("SmallView/customerDetails", dataCustomer);
            }
            var item = await LogicalModel.getId(User.Identity.GetUserId());
            var list = await new LogicalModel().listToApprove(item.Item2);

            return View("applyLoan", list);
        }
        // GET : approve loan 
        [HttpGet]
        public  async Task<ActionResult> Responsent(string id, string xtr)
        {
            if (id != null & xtr != null)
            {
                if (Request.IsAuthenticated)
                {
                    if (await lm.putInLoan(id, xtr, User.Identity.GetUserId()))
                    {
                       
                    }
                }
                else
                {
                   return RedirectToAction("Login", "Account", new { returnUrl = "/Transaction/approveLoan" });
                }
            }
            var item = await LogicalModel.getId(User.Identity.GetUserId());
            var list = await new LogicalModel().listToApprove(item.Item2);

            return View("applyLoan",list);
        }



        // GET : apply for loan 
        [HttpGet]
        [Authorize(Roles = "admin, developer")]
        public ActionResult applyForloan(string key,string keyvalue)
        {
            if (key != null && key.Length !=( 1 & 0) )
            {
                return View("Loan", new createLoginView() { CustomerId = key, customerName = keyvalue});
            }
            return View("Loan");
        }
        // POST : apply for loan 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> applyForloan(createLoginView l)
        {

            if(l.date == null) { l.date = DateTime.Now; }
               
            if (ModelState.IsValid)
            {
                var StaffId = await LogicalModel.getId(User.Identity.GetUserId());

                if (LogicalModel.applyForLoan(l,StaffId.Item1,StaffId.Item2))
                {
                    return RedirectToAction("Loan");
                }


            }
            return View("Loan", l);
        }
        // GET : check loan status 
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> checkStatus(string key)
        {
            var staff = await LogicalModel.getId(User.Identity.GetUserId());

            if (Request.IsAjaxRequest())
            {
                if (key != null && key.Length == 3)
                {
                    var lists = await LogicalModel.checkLoanStatus(key, staff.Item1, staff.Item2);

                    return PartialView("SmallView/createloanview", lists);
                }
            }
          
          

            var list = await LogicalModel.checkLoanStatus(staff.Item1,staff.Item2);

           return View(list);
        }
        // GET : pay loan 
        [HttpGet]
        [Authorize(Roles = "staff, developer")]
        public ActionResult payLoan(string key,string name)
        {
            if (key == null )
            {
                return View("payment");
            }
            
            return View("payment",new paymentViewModel() { loanId = key ,customer_name = name});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> payLoan(paymentViewModel pay)
        {
            var item  = await LogicalModel.getId(User.Identity.GetUserId());
            pay.staffId = item.Item1;

            if (ModelState.IsValid)
            {


                var results = await lm.PaymentEntity(pay);
                switch (results.Item2)
                {
                    case StringRescource.no_loan:
                        ModelState.AddModelError("", StringRescource.no_loan);
                        break;
                    case StringRescource.no_loan_:
                        ModelState.AddModelError("", StringRescource.no_loan_);
                        break;
                    case StringRescource.loan_paid_exceed:
                        ModelState.AddModelError("", StringRescource.loan_paid_exceed);
                        break;
                    case StringRescource.loan_complete:
                        ModelState.AddModelError("", StringRescource.loan_complete);
                        break;
                    case StringRescource.loan_paid:
                        return RedirectToAction(nameof(HomeController.Index),nameof(HomeController));
                        break;
                    default:
                        break;
                }
            }
            return View("payment", pay);
        }
        // GET : monthly
        [HttpGet]
        public async Task<ActionResult> Monthly()
        {
            var list = await new LogicalModel().nextpayday(User.Identity.GetUserId());
            return View("Nextpayday", list);
        }

        // GET : check loan balance
        [HttpGet]
        public ActionResult checkLoan()
        {
            return View();
        }

        //GET :   payment
        [HttpGet]
        //[Authorize(Roles = "staff, developer")]
        public ActionResult ToHisa()
        {
            return View("hisaView");
        }
        
        //POST : updating hisa information
        public ActionResult ToHisa(MainHisa hs)
        {
            if (ModelState.IsValid)
            {

                if (lm.AddHisa(hs, User.Identity.GetUserId()))
                {
                    return RedirectToAction("", "");
                }
            }
            return RedirectToAction("", "", hs);
        }



    }
}