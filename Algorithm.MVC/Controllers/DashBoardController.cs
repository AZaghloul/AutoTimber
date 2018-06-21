using Algorithm.DB;
using Algorithm.DB.Models;
using Algorithm.DB.ViewModels;
using Algorithm.MVC.DAL;
using Bim.Application.IRCWood.Common;
using Bim.Application.IRCWood.IRC;
using Bim.Application.IRCWood.Physical;
using Bim.Domain.Ifc;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xbim.Ifc4.SharedBldgElements;

namespace Algorithm.MVC.Controllers
{
    public class DashBoardController : Controller
    {
        
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            UnitOfWork uow = new UnitOfWork(new AlgorithmDB());
            var user = uow.Users.FindById(userId);
            //if user not found
            if (user==null)
            {
                uow.Users.Insert(new User() {
                    Id = userId
                });
                uow.SaveChanges();
                //get the user from db
                user = uow.Users.FindById(userId);

            }
            var projects = uow.Projects.FindBy(e => e.UserId == userId);

            DashBoardVM dashBoardVM = DashBoardVM.Load(user, projects);

            return View(dashBoardVM);
        }
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        public void Design(string fileName)
        {
            StudTable.FilePath = Server.MapPath(@"~\App_Data\Tables\StudSpacingTable.csv");
            Table502_3_1.FilePath = Server.MapPath(@"~\App_Data\Tables\table502.3.1(1).txt");
            fileName = fileName ?? "home-2floor-ft.ifc";
            var filePath = Server.MapPath($"~/Users/input-files/{fileName}");
            var outputFile = Server.MapPath($"~/Users/output-files/{fileName}-Structure");
            IfModel model = IfModel.Open(filePath);
            //IfStartup.Configuration(model);
            model.Delete<IfcBeam>();
            model.Delete<IfcColumn>();
            WoodFrame wf = new WoodFrame(model);

           // wf.StudTable = IfStartup.LoadTablesAsync.Result;

            wf.JoistTable = Table502_3_1.Load(null);

           // wf.TestAsync();


            // model.Delete<IfcWall>();
            model.Save(outputFile);
            // OpenWindow(fileName);

        }
    }
}