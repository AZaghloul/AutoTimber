using Bim.Application.IRCWood.Common;
using Bim.Application.IRCWood.IRC;
using Bim.Application.IRCWood.Physical;
using Bim.Domain.Ifc;
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
        // GET: DashBoard
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Upload(string data)
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        // and optionally write the file to disk
                        var fileName = fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/Users/input-files/"), fileName);
                        fileContent.SaveAs(path);
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");

        }


        public void Design(string fileName)
        {
            StudTable.FilePath = Server.MapPath(@"~\App_Data\Tables\StudSpacingTable.csv");
            Table502_3_1.FilePath = Server.MapPath(@"~\App_Data\Tables\table502.3.1(1).txt");
            fileName = fileName ?? "home-2floor-ft.ifc";
            var filePath = Server.MapPath($"~/Users/input-files/{fileName}");
            var outputFile = Server.MapPath($"~/Users/output-files/{fileName}-Structure");
            IfModel model = IfModel.Open(filePath);
            IfStartup.Configuration(model);
            model.Delete<IfcBeam>();
            model.Delete<IfcColumn>();
            WoodFrame wf = new WoodFrame(model);

            wf.FrameWalls();
            // model.Delete<IfcWall>();
            model.Save(outputFile);
            // OpenWindow(fileName);

        }
    }
}