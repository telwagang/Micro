using Micro.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Micro.Models
{
    public class InterfaceModel
    {

        public void Update<T>(T obj, params Expression<Func<T, object>>[] properies) where T : class
        {
            using (var db = new MicroContext())
            {
                db.Set<T>().Attach(obj);

                foreach (var p in properies)
                {
                    db.Entry(obj).Property(p).IsModified = true;
                }
            }






        }
    }
}