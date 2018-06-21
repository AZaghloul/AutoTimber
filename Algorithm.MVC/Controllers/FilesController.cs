using Bim.Application.IRCWood.Common;
using Bim.Application.IRCWood.IRC;
using Bim.Application.IRCWood.Physical;
using Bim.Domain.Ifc;
using Bim.IO;

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Algorithm.DB.ViewModels;
using System.Web;
using System.Collections.Generic;
using Algorithm.MVC.DAL;
using Algorithm.DB;
using Algorithm.DB.Models;
using Microsoft.AspNet.Identity;
using Algorithm.MVC.Helper;
using Xbim.Ifc;
using System.Linq;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;
using Bim.Domain.Ifc.Enums;
using Xbim.Ifc4.SharedBldgElements;
using Bim.BOQ;

namespace Algorithm.MVC.Controllers
{
    public class FilesController : Controller
    {

        #region Upload
        public ActionResult Upload()
        {


            return View(new UploadVM());
        }
        [HttpPost]
        public ActionResult Upload(UploadVM model, IEnumerable<HttpPostedFileBase> files)
        {
            FileData fileData = new FileData();

            //if found the same file redirect to the show Action

            try
            {
                foreach (var file in files)
                {

                    if (file != null && file.ContentLength > 0)
                    {

                        //create FileData object to hold all file paths
                        fileData = new FileData(file.FileName);

                        if (IfcHandler.CheckFileExist(fileData.InputPath))
                        {
                            return PartialView(@"~/Views/Files/Show.cshtml", fileData);
                        }
                        //write the file to the desk
                        file.SaveAs(fileData.InputPath);

                        //send the input data to the database
                        UnitOfWork uow = new UnitOfWork(new AlgorithmDB());
                        model.FileName = file.FileName;

                        var proj = new Project()
                        {
                            Id = Guid.NewGuid(),
                            Title = model.Title,
                            Description = model.Desciption,
                            FileName = model.FileName,
                            UserId = User.Identity.GetUserId()
                        };
                        uow.Projects.Insert(proj);
                        uow.SaveChanges();
                        //send the data Model to the Show Action
                        return PartialView(@"~/Views/Files/Show.cshtml", fileData);

                    }
                }

                Response.StatusCode = (int)HttpStatusCode.OK;

            }
            catch (Exception)
            {
                return View();
            }


            return RedirectToAction("Show", "Files", fileData);



        }

        #endregion

        #region Download

        public FileContentResult Download(string Id, string type)
        {
            ///conteeeeeeeeeent
            var fileName = "home-2floor-ft.ifc";
            var filePath = Server.MapPath($"~/Users/input-files/{fileName}");
            var outputFile = Server.MapPath($"~/Users/output-files/{fileName}-Structure");
            StudTable.FilePath = Server.MapPath(@"~/App_Data\Tables\StudSpacingTable.csv");

            Table502_5.HeadersTableExteriorPath = Server.MapPath(@"~/App_Data\Tables\table502.5(1).csv");
            Table502_5.HeadersTableInteriorPath = Server.MapPath(@"~/App_Data\Tables\table502.5(2).csv");

            Table502_3_1.JoistTableLivingAreasPath = Server.MapPath(@"~/App_Data\Tables\table502.3.1(2).csv");
            Table502_3_1.JoistTableSleepingAreasPath = Server.MapPath(@"~/App_Data\Tables\table502.3.1(1).csv");

            using (IfModel model = IfModel.Open(filePath))
            {
                Bim.Domain.Configuration.Startup.Configuration(model);

                model.Delete<IfcBeam>();
                model.Delete<IfcColumn>();
                WoodFrame wf = new WoodFrame(model);
                wf.FrameWalls();
                model.Delete<IfcWall>();
                model.Delete<IfcSlab>();

                // model.Save(saveName);

                GeometryCollection GC1 = new GeometryCollection();
                GC1.AddToCollection(model.Instances.OfType<IfJoist>());
                GC1.AddToCollection(model.Instances.OfType<IfStud>());
                GC1.AddToCollection(model.Instances.OfType<IfSill>());
                byte[] filecontent = GC1.ToExcel(GC1.BOQTable, "Testing Excel", false, "Number", "Collection");

                ///result
                return File(filecontent, GC1.ExcelContentType, "test1.xlsx");
            }
        }
        #endregion

        #region Show Models Methods

        public ActionResult Show(FileData fileData)
        {


            //if (! IfcHandler.CheckFileExist(fileData.wexBIMPath))
            //{
            //    IfcHandler.ToWexBim(fileData.InputPath, fileData.wexBIMPath);
            //} 

            return View(fileData);
        }


        //public ActionResult Show(List<FileData> fileData)
        //{


        //    if (!IfcHandler.CheckFileExist(fileData.wexBIMPath))
        //    {
        //        IfcHandler.ToWexBim(fileData.InputPath, fileData.wexBIMPath);
        //    }

        //    return View(fileData);
        //}
        public ActionResult Viewer(string FileName, bool Structure)
        {
            if (Structure == true)
            {
                FileName = "ITI.Qondos.2-Solved-structure.wexBIM";
                return File(new FileData(FileName).wexBIMPath, "application/octet-stream", FileName);
            }
            else
            {

                FileName = "ITI.Qondos.2-Solved-structure.wexBIM";
                return File(new FileData(FileName).wexBIMPath, "application/octet-stream", FileName);

            }

        }


        #endregion

        #region Design Methods

        public ActionResult Design()
        {
            return View();
        }
        [HttpPost]
        public void Design(string fileName)
        {
            //StudTable.FilePath = Server.MapPath(@"~\App_Data\Tables\StudSpacingTable.csv");
            //Table502_3_1.FilePath = Server.MapPath(@"~\App_Data\Tables\table502.3.1(1).txt");
            fileName = fileName ?? "home-2floor-ft.ifc";
            var filePath = Server.MapPath($"~/Users/input-files/{fileName}");
            var outputFile = Server.MapPath($"~/Users/output-files/{fileName}-Structure");


            var startup = new IfStartup();
            IfModel model = IfModel.Open(filePath);
            WoodFrame wf = new WoodFrame(model);
            startup.Configure(model, wf);
            startup.Configuration(model);
            //wf.GetPolygons();
            //wf.Optimize();
            //wf.Write();
            model.Save(outputFile);



        }

        
        #endregion





    }
}