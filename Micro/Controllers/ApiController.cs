using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Web;
using System.Web.Mvc;
using Micro.Models;
using System.Dynamic;
using Micro.DataLayer;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Micro.Controllers
{
    // [Authorize]
     
    public class ApiController : Controller
    {
        // GET: Staff

        [HttpGet]
        public JsonResult GetLoanerStatus(string data)
      {
            using (MicroContext db = new MicroContext() )
            {
                var list = (from a in db.Customer join b in db.Loan
                           on a.CustomerId equals b.CustomerId where
                           a.First_Name.Contains(data)
                           select   new searchloanviewmodel
                           {
                               loanid = b.LoanId,
                               First_name = a.First_Name,
                               Middle_name = a.Middle_Name,
                               Last_name = a.Last_Name, 
                               CustomerId = a.CustomerId,
                               amount = b.Amount,
                               returnAmount = b.returnAmount,
                               duration = b.Duration,
                               date = b.datesumbit
                           }).Take(10).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }

           
            }
        [HttpGet]
        public async Task< JsonResult> GetCustomerName(string data)
        {
            using (MicroContext db = new MicroContext())
            {
                var namelist = await (from a in db.Customer
                                     where String.Concat(a.First_Name,a.Middle_Name,a.Last_Name).Contains(data)
                                     select new name
                                     {
                                        Fullname = (a.First_Name+" "+a.Middle_Name+" "+a.Last_Name)
                                     }).Take(10).ToListAsync();

                return  Json(namelist, JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpGet]
        public async Task<JsonResult> GetUserSummary(string data)
        {
            return Json(await new LogicalModel().getSummary(data), JsonRequestBehavior.AllowGet);
        }

    }
}