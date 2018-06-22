using Algorithm.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.ViewModels
{
    public class DashBoardVM
    {
        public List<ProjectVM> Projects { get; set; }
        public User User { get; set; }

        public static DashBoardVM Load(User user, IEnumerable<Project> projects)
        {
            var db = new DashBoardVM
            {
                //load User
                User = new User(),
                Projects = new List<ProjectVM>()
            };
            db.User.Id = user.Id;
            db.User.ProfilePic = user.ProfilePic;
            db.User.About = user.About;

            //load projects VM
            foreach (var project in projects)
            {
                db.Projects.Add(new ProjectVM(project));
            }

            return db;

        }
        public string Refresh { get; set; }

    }
}
