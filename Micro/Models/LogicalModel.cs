using Micro.Models.Loan;
using System;
using System.Linq;
using Micro.DataLayer;
using System.Data.Entity;
using Micro.Models.Hisa;
using System.Collections.Generic;
using Micro.Models.akiba;
using System.Collections;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Helpers;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using System.Linq.Expressions;
using Micro.Models.Management;
using AutoMapper.QueryableExtensions;

namespace Micro.Models
{

    public class LogicalModel : InterfaceModel
    {
        protected const string d = "diposit";
        protected static string w = "withdraw";
        protected static string yes = "up";
        protected static string no = "down";

        
        public LogicalModel()
        {

        }
        public LogicalModel(MapperConfiguration config)
        {
            
            this.config = config;
        }
        private  MapperConfiguration config;

        private static bool Dispote(AkibaCT ct)
        {
            using (var db = new MicroContext())
            {
                //1st for Credit 
                if (ct.StaffId > 0)
                {

                    //add the log data to akiba credit table 
                    db.AkibaCT.Add(ct);


                    //find the current balance so as to add it the new amount
                    Akiba prebalance = (from bal in db.Akiba
                                        where bal.AkibaId == ct.AkibaId
                                        select bal).First();

                    //chech if the cuurent balance is less than the amount requested 
                    if (prebalance.Amount > ct.CT_Amount)
                    {
                        //taking current balance and reduce the amount 
                        int balance = (prebalance.Amount -
                            ct.CT_Amount);

                        if (balance < 0)
                        {
                            return false;
                        }

                        //add the new balance in the akiba class 
                        //for updating the database
                        prebalance.Amount = balance;

                        //updating the database 
                        db.Entry(prebalance).State = EntityState.Modified;
                        db.SaveChanges();

                        return true;
                    }


                }
            }
            return false;
        }
        private static bool Dispote(AkibaDT dt)
        {
            using (var db = new MicroContext())
            {
                if (dt.StaffId > 0)
                {

                    try
                    {
                        db.AkibaDT.Add(dt);



                        //find the current balance so as to add it the new amount
                        Akiba prebalance = (from bal in db.Akiba
                                            where bal.AkibaId == dt.AkibaId
                                            select bal).First();

                        //taking current balance + add new amount 
                        int balance = (prebalance.Amount
                                        + dt.DT_Amount);



                        //add the new balance in the akiba class 
                        //for updating the database
                        prebalance.Amount = balance;

                        //updating the database 
                        db.Entry(prebalance).State = EntityState.Modified;
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            }
            return false;
        }

        public static bool newAkiba(StartAkiba A)
        {
            if (A != null)
            {
                try
                {

                    using (var db = new MicroContext())
                    {
                        try
                        {

                            db.StartAkiba.Add(A);
                            var ak = new Akiba() { AkibaId = A.StartAkibaId, Amount = A.Amount };

                            db.Akiba.Add(ak);
                            db.SaveChanges();

                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                    throw;

                }
            }
            return false;
        }

        public static bool topDispote(akibaViewModel ct, int staff)
        {
            if (ct != null)
            {
                if (ct.AkibaId.Length != 8)
                    return false;

                if (ct.ctORdt == w)
                {
                    if (Dispote(new AkibaCT()
                    {
                        AkibaId = ct.AkibaId,
                        CT_Amount = ct.Amount,
                        StaffId = staff,
                        date = DateTime.Now
                    }))
                    {
                        return true;
                    }
                }
                if (ct.ctORdt == d)
                {
                    if (Dispote(new AkibaDT()
                    {
                        AkibaId = ct.AkibaId,
                        DT_Amount = ct.Amount,
                        StaffId = staff,
                        date = DateTime.Now
                    }))
                    {
                        return true;
                    }
                }

            }


            return false;
        }

        public static Boolean applyForLoan(createLoginView l, int StaffId,int compyid)
        {

            var LoanId = Guid.NewGuid().ToString("D").Substring(14, 14).ToUpper();
        

            if (memberSince(l.CustomerId))
            {
                using (var db = new MicroContext())
                {
                    try
                    {

                        var data = (from a in db.Interest
                                    where a.duration == l.duration
                                    where a.CompanyId == compyid
                                    select a).FirstOrDefault();

                        var intamount = Math.Round(((data.rate / 100 * l.amount) + l.amount) * data.duration);


                        db.Loan.Add(new Loan.Loan()
                        {
                            Amount = l.amount,
                            CustomerId = l.CustomerId,
                            datesumbit = l.date,
                            Duration = l.duration,
                            LoanId = LoanId,
                            StaffId = StaffId,
                            returnAmount = (int)intamount, 
                            LoanApplication = new LoanApplicantion()
                            {
                                StaffId = StaffId,
                                Approved = false,
                                date = l.date

                            }
                        });

                        db.SaveChanges();

                        return true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            }
            return false;
        }

        public async Task<Tuple<bool,string>> PaymentEntity(paymentViewModel pay)
        {


            string yes = string.Empty;

            using (var db = new MicroContext())
            {
                string id = pay.loanId;
                //checking how much is suppose to be paid on every installement 
                var balance = await (from cust in db.LoanStatus
                                     join
                                    tht in db.loanBalance
                                    on cust.LoanId equals tht.LoanId
                                    join c in db.Loan
                                    on cust.LoanId equals c.LoanId
                                    join d in db.StartAkiba
                                    on c.CustomerId equals d.CustomerId 
                                     where cust.LoanId == id
                                     orderby cust.Nextpayday ascending
                                     select new { cust.LoanId, cust.Nextpayday, tht.balance, d.StartAkibaId })
                              .FirstAsync();

                        if (balance == null)
                {

                    if (!IfUserHasLoan(id))
                    {

                        return new Tuple<bool, string>(false, StringRescource.no_loan);
                    }

                    return new Tuple<bool,string>(false,StringRescource.no_loan_);

                }

                int loanBalanc = balance.balance;

                //add data to payment table 
                db.LoanPayment.Add(new Payment() { LoanId = pay.loanId, date = DateTime.Now, AmountPaid = pay.amountPaid, StaffId = pay.staffId });

                int newbalance;

                if (loanBalanc == 0)
                {
                    return new Tuple<bool, string>(false, StringRescource.loan_complete);

                }

                if (loanBalanc > pay.amountPaid)
                {
                    //add data to loan balance table with a new balance
                    newbalance = loanBalanc - pay.amountPaid;

                }
                else
                {
                    newbalance = pay.amountPaid - loanBalanc;
                    
                }

                if (newbalance == 0)
                {
                    //loan is done 
                    db.Entry(new LoanBalance() { LoanId = balance.LoanId }).State = EntityState.Deleted;
                    db.Entry(new LoanStatus() { LoanId = balance.LoanId }).State = EntityState.Deleted;
                    db.Entry(new LoanApplicantion() { LoanId = balance.LoanId }).State = EntityState.Deleted;
                    db.LoanDone.Add(new LoanDone() { LoanId = balance.LoanId, Done = true });

                    await db.SaveChangesAsync();

                    return new Tuple<bool, string>(true, StringRescource.loan_complete);
                    
                }
                else if(newbalance > 0 )
                {
                    //update laonbalance table 
                    db.Entry(new LoanBalance() { LoanId = balance.LoanId, balance = newbalance }).State = EntityState.Modified;

                    //updating the next payday 
                    db.LoanStatus.Add(new LoanStatus()
                    {
                        LoanId = balance.LoanId,
                        Nextpayday = balance.Nextpayday.AddDays(30)
                    });

                    await db.SaveChangesAsync();
                    return new Tuple<bool, string>(true, StringRescource.loan_paid);

                }else if (newbalance < 0)
                {
                    int str = 0 - newbalance;

                    db.AkibaDT.Add(new AkibaDT() { AkibaId = balance.StartAkibaId, DT_Amount = str, StaffId = pay.staffId, date = DateTime.Now });

                    var akibaBalance = db.Akiba.FirstOrDefault(c => c.AkibaId == balance.StartAkibaId);

                    akibaBalance.Amount = akibaBalance.Amount + str;

                    db.Entry(akibaBalance).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return new Tuple<bool, string>(true, StringRescource.loan_paid_exceed);
                }


                return new Tuple<bool, string>(false, StringRescource.loan_failed);
            }
            
        }

        public static async Task<object> checkLoanStatus(int staff, int compny)
        {


            try
            {
                using (var db = new MicroContext())
                {
                    var data = await (from x in db.Customer
                                      join c in db.Loan on
               x.CustomerId equals c.CustomerId
                                      join b in db.LoanApplication on c.LoanId equals b.LoanId
                                      where
            x.CompanyId == compny & b.Approved == true
                                      select new searchloanviewmodel
                                      {
                                          loanid = c.LoanId,
                                          First_name = x.First_Name,
                                          Middle_name = x.Middle_Name,
                                          Last_name = x.Last_Name,
                                          CustomerId = c.CustomerId,
                                          amount = c.Amount,
                                          returnAmount = c.returnAmount,
                                          duration = c.Duration,
                                          date = c.datesumbit
                                      }).Take(20).ToListAsync();

                    searchViewModel svm = new searchViewModel()
                    {
                        loanlist = data
                    };

                    return svm;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static async Task<object> checkLoanStatus(string item, int staff, int compny)
        {


            try
            {
                using (var db = new MicroContext())
                {
                    var data = await (from x in db.Customer
                                      join c in db.Loan on
               x.CustomerId equals c.CustomerId
                                      where
            x.CompanyId == compny & String.Concat(x.First_Name, x.Middle_Name, x.Last_Name).Contains(item)
                                      select new searchloanviewmodel
                                      {
                                          loanid = c.LoanId,
                                          First_name = x.First_Name,
                                          Middle_name = x.Middle_Name,
                                          Last_name = x.Last_Name,
                                          CustomerId = c.CustomerId,
                                          amount = c.Amount,
                                          returnAmount = c.returnAmount,
                                          duration = c.Duration,
                                          date = c.datesumbit
                                      }).Take(20).ToListAsync();

                    searchViewModel svm = new searchViewModel()
                    {
                        loanlist = data
                    };

                    return svm;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
       
        public async Task<bool> putInLoan(string id, string xtr, string userid)
        {
            try
            {
                var staffId = await getId(userid);
                int companyId = staffId.Item2;

                using (var db = new MicroContext())
                {

                    var data = await (from a in db.Loan
                                      join b in db.LoanApplication
                                      on a.LoanId equals b.LoanId
                                      join w in db.StartAkiba
                                      on a.CustomerId equals w.CustomerId
                                      join s in db.Akiba 
                                      on w.StartAkibaId equals s.AkibaId
                                      join c in db.Interest
                                      on a.Duration equals c.duration
                                      where a.LoanId == id & c.CompanyId == companyId
                                      select new proccesloan
                                      {
                                          bank_amount = s.Amount,
                                          rate = c.rate,
                                          loan_amount = a.Amount,
                                          duration = a.Duration,
                                          returnamount =  a.returnAmount,
                                          loanid = a.LoanId
                                      }).FirstOrDefaultAsync();

                    if (xtr == yes)
                    {


                        LoanBalance lb = new LoanBalance
                        {
                            LoanId = data.loanid,
                            balance = data.returnamount
                        };
                        LoanStatus ls =
                            new LoanStatus
                            {
                                LoanId = data.loanid,
                                monthly = (data.returnamount /data.duration),
                                Nextpayday = DateTime.Now.AddDays(37)

                            };

                        LoanApplicantion ap = db.LoanApplication.Find(data.loanid);
                        Loan.Loan ln = db.Loan.Find(data.loanid);

                        ap.Approved = true;
                        ln.returnAmount = data.returnamount;

                        db.loanBalance.Add(lb);
                        db.LoanStatus.Add(ls);

                        db.Entry(ln).State = EntityState.Modified;
                        db.Entry(ap).State = EntityState.Modified;

                        await db.SaveChangesAsync();

                    }
                    else if (xtr == no)
                    {
                        db.Entry(new LoanApplicantion() { LoanId = data.loanid, Approved = false }).State = EntityState.Modified;
                    }


                    return true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public async Task<object> listToApprove(int id)
        {

            try
            {
                var compyid = id;

                using (var db = new MicroContext())
                {

                    var test = await (from x in db.LoanApplication
                                      join
             a in db.Loan on x.LoanId equals a.LoanId
                                      join
      b in db.Customer on a.CustomerId equals b.CustomerId
                                      join
      c in db.Staff on a.StaffId equals c.ID
                                      join i in db.Interest on b.CompanyId equals i.CompanyId
                                      where x.Approved == false & c.CompanyId == compyid & i.duration == a.Duration
                                      orderby a.datesumbit ascending
                                      select new LoanViewModel
                                      {
                                          customerName = (b.First_Name + " " + b.Middle_Name + " " + b.Last_Name),
                                          loanId = x.LoanId,
                                          By = (c.First_Name + " " + c.Last_Name),
                                          amount = a.Amount,
                                          duration = a.Duration,
                                          interest = i.rate,
                                          DateIn = a.datesumbit,
                                          CustomerId = b.CustomerId
                                      }
                                ).Take(20).ToListAsync();



                    ViewHelperModel list = new ViewHelperModel()
                    {
                        lv = test,
                        customer = null

                    };

                    return list;
                }
            }
            catch (Exception)
            {

                throw;
            }




        }

        public async Task<object> nextpayday(string userid)
        {
            object datalist = null;

            if (userid.IsNullOrWhiteSpace()) return datalist;
            var sysdata = await getId(userid);
            var now = DateTime.Now;
            var lastday = DateTime.DaysInMonth(now.Year, now.Month);
            var startDate  = new DateTime(now.Year, now.Month, 1);
            var lastDate = new DateTime(now.Year, now.Month, lastday);
           

            using (var db = new MicroContext())
            {
                try
                {
                    var list = await (from a in db.Loan
                                      join c in db.LoanStatus on a.LoanId equals c.LoanId 
                                      join b in db.LoanPayment on a.LoanId equals b.LoanId into x
                                      from xr in x.DefaultIfEmpty()
                                      join e in db.Customer on a.CustomerId equals e.CustomerId
                                     
                                      join d in db.Interest on a.Duration equals d.duration
                                      
                                      where d.CompanyId == sysdata.Item2
                                      && DbFunctions.TruncateTime( c.Nextpayday.Date) >= DbFunctions.TruncateTime( startDate.Date) && DbFunctions.TruncateTime(c.Nextpayday.Date) <= DbFunctions.TruncateTime( lastDate.Date)
                                      select new nextpaydayModel
                                      {
                                          Customer_name = string.Concat(e.First_Name, " ", e.Middle_Name, " ", e.Last_Name),
                                          LDD = xr.date,
                                          LDA = xr.AmountPaid,
                                          NDD = c.Nextpayday,
                                          NDA = getamout(a.Duration, d.rate, a.Amount)

                                      }
                           ).ToListAsync();

                    return list;
                }
                catch (Exception e)
                {

                    throw (e.InnerException);
                }
               
            }

          
        }

        private int getamout(int duration, double rate, int amount)
        {
            var percentRate = rate / 100;
            var percentofamount = percentRate * amount;
            var returnamount = amount + percentofamount;
            int monthlydeposite = (Int32)returnamount / duration;

            return monthlydeposite;
        }

        public async Task<object> getSummary(string name/*,int compnayid*/)
        {
            //company filter is yet to be implememented

            using (MicroContext db = new MicroContext())
            {
                try
                {

                    var data = await (from a in db.Customer
                                      join b in db.StartAkiba
                on a.CustomerId equals b.CustomerId
                                      join c in db.Akiba
                on b.StartAkibaId equals c.AkibaId
                                      join d in db.Loan
              on a.CustomerId equals d.CustomerId into t
                                      from tr in t.DefaultIfEmpty()
                                      join e in db.loanBalance
                on tr.LoanId equals e.LoanId into o
                                      from og in
o.DefaultIfEmpty()
                                      where
                    String.Concat(a.First_Name, a.Middle_Name, a.Last_Name).Contains(name)
                                      select new usersummarymodel
                                      {
                                          customerid = a.CustomerId,
                                          username = (a.First_Name + " " + a.Middle_Name + " " + a.Last_Name),
                                          address = (a.Street + ", " + a.Ward + ", " + a.Division),
                                          birthday = a.Birthdate,
                                          nationality = a.Nationality,
                                          phone_number = a.Mobile_Number,
                                          date = a.date,
                                          akibaid = b.StartAkibaId,
                                          Akiba_balance = c.Amount,
                                          loanid = tr.LoanId,
                                          loan_balance = og.balance,
                                          amount_taken = tr.Amount,
                                          return_amount = tr.returnAmount
                                      }).Take(1).FirstOrDefaultAsync();

                    if (data != null)
                    {
                        data.age = getAge(data.birthday);
                    }

                    return data;
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }

        public async Task<object> getUserDetails(string id, string key)
        {
            var comid = await getId(id);

            using (var db = new MicroContext())
            {
                var full_user = await (from a in db.Customer
                                       where a.CustomerId == key & a.CompanyId == comid.Item2
                                       select a
                                       ).FirstOrDefaultAsync();


                return full_user;
            }

        }
     
        public static async Task<Tuple<object, bool>> getActivity(string key, string id, string customer)
        {

            var compid = await getId(id);
            var akbvm = new AkibaMultipleViewModel();
            IEnumerable<activityViewModel> dataC, dataD;

            if (key != null)
            {
                using (var db = new MicroContext())
                {


                    dataC = await (from cust in db.AkibaCT
                                   join a in db.StartAkiba on
                                   cust.AkibaId equals a.StartAkibaId
                                   join c in db.Customer on 
                                   a.CustomerId equals c.CustomerId 
                                   join d in db.Staff on cust.StaffId equals d.ID
                                   where cust.AkibaId == key && d.CompanyId == compid.Item2
                                   orderby cust.date ascending
                                   select new activityViewModel
                                   {
                                       Customer_name = String.Concat(c.First_Name," ",c.Last_Name),
                                       akiba_id = cust.AkibaId,
                                       amount = cust.CT_Amount,
                                       staff = String.Concat(d.First_Name, " ", d.Middle_Name, " ", d.Last_Name),
                                       Date = cust.date

                                   }
                                 )
                                 .Take(20)
                                 .ToListAsync();


                    dataD = await (from cust in db.AkibaDT
                                   join a in db.StartAkiba on
                                  cust.AkibaId equals a.StartAkibaId
                                   join c in db.Customer on
                                   a.CustomerId equals c.CustomerId
                                   join d in db.Staff on cust.StaffId equals d.ID
                                   where cust.AkibaId == key && d.CompanyId == compid.Item2
                                   orderby cust.date ascending
                                   select new activityViewModel
                                   {
                                       Customer_name = String.Concat(c.First_Name, " ", c.Last_Name),
                                       akiba_id = cust.AkibaId,
                                       amount = cust.DT_Amount,
                                       staff = String.Concat(d.First_Name, " ", d.Middle_Name, " ", d.Last_Name),
                                       Date = cust.date

                                   }
                               )
                               .Take(20)
                               .ToListAsync();
                }
                akbvm.Customer_name = customer;
                akbvm.AkibaCT = dataC;
                akbvm.AkibaDT = dataD;

                return new Tuple<object, bool>(akbvm, true);
            }
            else
            {
                using (var db = new MicroContext())
                {


                    dataC = await (from cust in db.AkibaCT
                                   join a in db.StartAkiba on
                                  cust.AkibaId equals a.StartAkibaId
                                   join c in db.Customer on
                                   a.CustomerId equals c.CustomerId
                                   join d in db.Staff on cust.StaffId equals d.ID
                                   where d.CompanyId == compid.Item2
                                   orderby cust.date ascending
                                   select new activityViewModel
                                   {
                                       Customer_name = String.Concat(c.First_Name, " ", c.Last_Name),
                                       akiba_id = cust.AkibaId,
                                       amount = cust.CT_Amount,
                                       staff = String.Concat(d.First_Name, " ", d.Middle_Name, " ", d.Last_Name),
                                       Date = cust.date

                                   }
                                 )
                                 .Take(20)
                                 .ToListAsync();


                    dataD = await (from cust in db.AkibaDT
                                   join a in db.StartAkiba on
                                  cust.AkibaId equals a.StartAkibaId
                                   join c in db.Customer on
                                   a.CustomerId equals c.CustomerId
                                   join d in db.Staff on cust.StaffId equals d.ID
                                   where d.CompanyId == compid.Item2
                                   orderby cust.date ascending
                                   select new activityViewModel
                                   {
                                       Customer_name = String.Concat(c.First_Name, " ", c.Last_Name), 
                                       akiba_id = cust.AkibaId,
                                       amount = cust.DT_Amount,
                                       staff = String.Concat(d.First_Name, " ", d.Middle_Name, " ", d.Last_Name),
                                       Date = cust.date

                                   }
                               )
                               .Take(20)
                               .ToListAsync();
                }
                akbvm.AkibaCT = dataC;
                akbvm.AkibaDT = dataD;

                return new Tuple<object, bool>(akbvm, false);




            }


        }
        public static async Task<Tuple<int, int>> getId(string id)
        {
            using (var db = new MicroContext())
            {

                var ID = await (from cust in db.Staff
                                where cust.UserID == id
                                select new Tempdata
                                {
                                    staffId = cust.ID,
                                    companyid = cust.CompanyId
                                }).FirstOrDefaultAsync();


                return new Tuple<int, int>(ID.staffId, ID.companyid);
            }
        }

        public static int getAge(DateTime? then)
        {
            if (!then.HasValue)
            {
                return 0;
            }
            return (DateTime.Now.Year - then.Value.Year);

        }

        public Boolean AddHisa(MainHisa hs, string id)
        {
            if (hs != null & id != null)
            {
                using (var db = new MicroContext())
                {
                    var compnyID = (from s in db.Staff
                                    where s.UserID == id
                                    select s.CompanyId).Take(1);

                    var hisaCal = (from item in db.HisaLimit
                                   where item.CompanyId == compnyID.First()
                                   select item).Take(1);

                    var values = (from cust in db.Mainhisa
                                  where cust.CustomerId == hs.CustomerId
                                  select cust).Take(1);

                    var defaultamount = values.FirstOrDefault().amount;
                    var defaultnohis = values.FirstOrDefault().NoHisa;
                    var inputamount = hs.amount;
                    var inputhisa = hs.NoHisa;

                    var definitiveamount = hisaCal.FirstOrDefault().amount;

                    if (defaultamount + inputamount
                         > definitiveamount)
                    {
                        return false;
                    }
                    else
                    {
                        //am not quite sure whats goin in down here but amount flied
                        // has to be updated in the database 
                        var newamount = defaultamount + inputamount;
                        var newhisa = defaultnohis + inputhisa;

                        Hisa.MainHisa Hs = new Hisa.MainHisa() { amount = newamount, NoHisa = newhisa };
                        db.Mainhisa.Attach(Hs);
                        db.Entry(Hs).Property(c => c.amount & c.NoHisa).IsModified = true;
                        db.SaveChanges();

                        HisaHistory hh = new HisaHistory()
                        {
                            Hisa = newhisa,
                            amount = newamount,
                            date = hs.date


                        };
                        db.HisaHistory.Add(hh);
                        db.SaveChanges();
                    }
                }
            }

            return false;
        }

        public async Task<object> TableToList(string id, int num)
        {
            int nothing = 0;
            if (id != null & num > 0)
            {
                try
                {
                    using (var db = new MicroContext())
                    {
                        switch (num)
                        {
                            case 1:
                                if (id.IsNullOrWhiteSpace()) { break; }

                                Customer some = await db.Customer.Where(s => s.CustomerId == id).FirstAsync();
                                if (some.CustomerId != null)
                                {
                                    return some;
                                }
                                break;

                            case 2:
                                if (id.IsNullOrWhiteSpace()) { break; }

                                int ids = (int)Convert.ChangeType(id ?? "0", typeof(int));

                                var list = await db.Staff.Where(s => s.CompanyId == ids).ProjectTo<RegisterStaffViewModel>(config).ToListAsync();

                                return list;
                                break;
                            case 3:
                                if (id.IsNullOrWhiteSpace()) { break; }

                                int idStaf = (int)Convert.ChangeType(id ?? "0", typeof(int));

                                var slist = await db.Staff.Where(s => s.ID == idStaf).ProjectTo<RegisterStaffViewModel>(config).FirstOrDefaultAsync();
                                return slist;
                                break;
                          
                                
                            default:
                                
                                break;
                        }

                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }




            return nothing;
        }
        private Boolean IfUserHasLoan(string id)
        {
            using (var db = new MicroContext())
            {
                var vaule = (from user in db.Loan
                             where user.LoanId == id
                             select user).Take(1);

                if (vaule == null)
                {
                    return false;
                }

                return true;
            }


            throw new NotImplementedException();
        }

        private static Boolean memberSince(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return false;
            }
            using (var db = new MicroContext())
            {
                var user = (from c in db.Customer
                            where c.CustomerId == id
                            select c.date).First();

                if (user != null)
                {
                    DateTime date;
                    date = DateTime.UtcNow;

                    DateTime adduser = user.AddMonths(3);
                    if (adduser >= date)
                    {
                        return false;
                    }
                    else if (adduser < date)
                    {
                        return true;
                    }



                }

            }

            return false;
        }

        // 
        private static bool loanRules(int amountlaon , int balance)
        {
            double ofloan = (25 / 100 * amountlaon);
            if (ofloan >= balance)
            {
                return true;

            }
            return false;
        }
    }


}