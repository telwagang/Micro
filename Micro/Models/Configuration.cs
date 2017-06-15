namespace Micro.Migrations
{
    using Models.Hisa;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DataLayer;
    using Controllers;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<Micro.DataLayer.MicroContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MicroContext context)
        {
            context.Company.AddOrUpdate(
               c => c.ID,
               new Company
               {
                   ID = 1,
                   Name = "Amani Saccos",
                   Address = "kunduchi,mbuyuni ",
                   Email = "saccos.amani@saccos.com",
                   KeyValue = 142576841,
                   location = "kunduchi,mbuyuni",
                   MobileNumber = 071414527,
                   date = DateTime.Now
               });
            context.Company.AddOrUpdate(
               c => c.ID,
               new Company
               {
                   ID = 2,
                   Name = "blah blah Saccos",
                   Address = "Mwenge, Dar es salaam",
                   Email = "saccos.blah@saccos.com",
                   KeyValue = 4785421,
                   location = "tra,mwenge",
                   MobileNumber = 077845121,
                   date = DateTime.Now
               });
            //context.Staff.AddOrUpdate(
            //    c => c.ID,
            //            new Staff
            //            {
            //                Id_employee = 2123,
            //                First_Name = "tony",
            //                Middle_Name = "something",
            //                Last_Name = "kayage",
            //                companyid = 2,
            //                Mobile_Number = 071478524,
            //                email = "throneJames.24@gmail.com",
            //                Position = "admin",
            //                date = DateTime.Now

            //            }
            //            );
            context.HisaLimit.AddOrUpdate(
              p => p.CompanyId,
                     new HisaLimit
                     {
                         Hisa = 80,
                         amount = 800000,
                         CompanyId = 0001
                     });

            try
            {
                // string[] aroles = { "admin", "staff", "developer", "customer" };
                // var account = new AccountController();

                //foreach (var i in aroles)
                //{

                //context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                //{
                //    Name = i

                //});
                //context.SaveChanges();





                // ApplicationUser user = context.Users.FirstOrDefault(c => c.Email == "avax.24@gmail.com");
                //      account.UserManager.AddToRoleAsync(user.Id, "developer");

                //}
                //}
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
