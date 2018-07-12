using AutoTimber.DB.DAL;
using AutoTimber.DB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTimber.DB.Managers
{
   public class UsersManagers:Repository<User>
    {
        public UsersManagers(DbContext ctx):base(ctx)
        {

        }
    }
}
