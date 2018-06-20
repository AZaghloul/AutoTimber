using Algorithm.DB.DAL;
using Bim.Domain.General;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.Managers
{
    class DesignOptionsManager:Repository<DesignOptions>
    {
        public DesignOptionsManager(DbContext ctx):base(ctx)
        {

        }
    }
}
