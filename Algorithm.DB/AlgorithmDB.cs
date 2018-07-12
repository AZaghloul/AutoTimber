using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AutoTimber.DB.Models;
using Bim.Domain.General;

namespace AutoTimber.DB
{
    public class AlgorithmDB : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<DesignOptions> DesignOptions { get; set; }
        public DbSet<User> Users { get; set; }
        public AlgorithmDB() : base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


        }

    }
    }

