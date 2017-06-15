using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Micro.Models;
using Micro.Models.Loan;
using System.Data.Entity.ModelConfiguration.Conventions;
using Micro.Models.Management;
using Micro.Models.Hisa;
using Microsoft.AspNet.Identity.EntityFramework;
using Micro.Models.akiba;

namespace Micro.DataLayer
{
    public class MicroContext : IdentityDbContext<ApplicationUser>
    {
        public MicroContext() : base("name=SqlMicro")
        {
            Database.Log = Console.WriteLine;
        }
        public static MicroContext Create()
        {
            return new MicroContext();
        }


        public DbSet<AkibaCT> AkibaCT { get; set; }
        public DbSet<AkibaDT> AkibaDT { get; set; }
        public DbSet<Akiba> Akiba { get; set; }
        public DbSet<StartAkiba> StartAkiba { get; set; }
     

        public DbSet<Staff> Staff { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Reference> Reference { get; set; }


        public DbSet<Loan> Loan { get; set; }
        public DbSet<LoanBalance> loanBalance { get; set; }
        public DbSet<LoanApplicantion> LoanApplication { get; set; }
        public DbSet<LoanLimit> LoanLimit { get; set; }
        public DbSet<LoanStatus> LoanStatus { get; set; }
        public DbSet<Payment> LoanPayment { get; set; }
        public DbSet<Interest> Interest { get; set; }
        public DbSet<LoanDone> LoanDone { get; set; }


        public DbSet<MainHisa> Mainhisa { get; set; }
        public DbSet<HisaHistory> HisaHistory { get; set; }
        public DbSet<HisaLimit> HisaLimit { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
          

            //   modelBuilder.Entity<IdentityUserClaim>().ToTable("webpages_claims");

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });









        }

    }
}