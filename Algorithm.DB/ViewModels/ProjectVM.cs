
using Algorithm.DB.Models;
using Bim.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.ViewModels
{
   public class ProjectVM
    {
        public Project Project { get; set; }
        public ProjectVM(Project project)
        {
            Project = project;
        }

    }
}
