using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Micro.Models;
using Micro.DataLayer;
using Micro.Models.Loan;
using System.Data.Entity;

namespace Micro.Interfaces
{
    public class IRepositoryinterest : Iinterest
    {
        private MicroContext db = null;
        public IRepositoryinterest()
        {
            this.db = new MicroContext();
        }


       public void addInterest(Interest inter)
        {
            db.Interest.Add(inter);
        }

       public void deleteInterest(Interest inter)
        {
            db.Entry(inter).State = EntityState.Deleted;
        }
        

        public void updateInterest(Interest inter)
        {
            db.Entry(inter).State = EntityState.Modified;
        }

        public Interest selectById(int id)
        {
            return db.Interest.Find(id);
        }

        public void save()
        {
            db.SaveChanges();
        }
        public IQueryable<Interest> getList()
        {
            return db.Interest.ToList().AsQueryable();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~IRepositoryinterest() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}