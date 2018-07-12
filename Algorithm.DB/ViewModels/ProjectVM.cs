
using AutoTimber.DB.Models;
using Bim.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTimber.DB.ViewModels
{
   public class ProjectVM
    {
        public Project Project { get; set; }
        public ProjectVM(Project project)
        {
            Project = project;
        }
        public static List<ProjectVM> Load(IEnumerable<Project> projects)
        {
            var vm = new List<ProjectVM>();
            foreach (var project in projects)
            {
               vm.Add(new ProjectVM(project));
            }

            return vm;
        }
    }
}
