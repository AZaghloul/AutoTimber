using AutoTimber.DB.DAL;
using Bim.Domain.General;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTimber.DB.Managers
{
    public class DesignOptionsManager : Repository<DesignOptions>
    {
        public DesignOptionsManager(DbContext ctx) : base(ctx)
        {

        }
    }
}
