using Algorithm.DB.DAL;
using Algorithm.DB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.Managers
{
   public class UsersManagers:Repository<User>
    {
        public UsersManagers(DbContext ctx):base(ctx)
        {

        }
    }
}
