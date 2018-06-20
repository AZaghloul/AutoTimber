using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Algorithm.DB.Models;

namespace Algorithm.DB
{
    public class AlgorithmDB : DbContext
    {
        DbSet<Project> Projects { get; set; }

        public AlgorithmDB() : base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


        }
    }
}
