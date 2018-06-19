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
    public class FilesController : Controller
    {
        // GET: Files
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Upload(string files)
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
        public ActionResult ViewFile(string name)
        {
            return View();
        }


        public ActionResult Design()
        {
            return View();
        }


        [HttpPost]
        public void Design(string fileName)
        {
            StudTable.FilePath = Server.MapPath(@"~\App_Data\Tables\StudSpacingTable.csv");
            Table502_3_1.FilePath = Server.MapPath(@"~\App_Data\Tables\table502.3.1(1).txt");
            fileName = fileName ?? "home-2floor-ft.ifc";
            var filePath = Server.MapPath($"~/Users/input-files/{fileName}");
            var outputFile = Server.MapPath($"~/Users/output-files/{fileName}-Structure");
            

            var startup = new IfStartup();
            IfModel model = IfModel.Open(filePath);
            WoodFrame wf = new WoodFrame(model);
            startup.Configure(model, wf);
            startup.Configuration(model);
            wf.GetPolygons();
            wf.Optimize();
            wf.Write();
            model.Save(outputFile);
          
            

        }

        public void Show(string fileName)
        {
           



        }


        public void ToWexBim(string fileName )
        {

        }

    }
}