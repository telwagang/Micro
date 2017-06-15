using Micro.Models.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Models
{
    public interface Iinterest: IDisposable
    { 
        void addInterest(Interest inter);
       
        void deleteInterest(Interest inter);
        void updateInterest(Interest inter);
        Interest selectById(int id);
        IQueryable<Interest> getList();
        void save();
    
        }
    

}
