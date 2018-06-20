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
        public  ActionResult Upload(UploadVM model, IEnumerable<HttpPostedFileBase> files)
        {
            FileData fileData=new FileData();

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
                           return RedirectToAction("Show", "Files", fileData);
                        }
                        //write the file to the desk
                        file.SaveAs(fileData.InputPath);

                        //send the input data to the database
                        UnitOfWork uow = new UnitOfWork(new AlgorithmDB());
                        var l = Guid.TryParse(User.Identity.GetUserId(), out Guid userid);
                        model.FileName = file.FileName;

                        var proj = new Project()
                        {
                            Id = Guid.NewGuid(),
                            Title = model.Title,
                            Description = model.Desciption,
                            FileName = model.FileName,
                            UserId = userid
                        };
                        uow.Projects.Insert(proj);
                        uow.SaveChanges();
                        //send the data Model to the Show Action
                        return RedirectToAction("Show", "Files", fileData);
                        
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

        #region Show Models Methods

        //public ActionResult Show(FileData fileData)
        //{


        //    //if (! IfcHandler.CheckFileExist(fileData.wexBIMPath))
        //    //{
        //    //    IfcHandler.ToWexBim(fileData.InputPath, fileData.wexBIMPath);
        //    //} 

        //    return View(fileData);
        //}


        public ActionResult Show(List< FileData> fileData)
        {
            fileData = new List<FileData>() { new FileData(), new FileData() };

            //if (! IfcHandler.CheckFileExist(fileData.wexBIMPath))
            //{
            //    IfcHandler.ToWexBim(fileData.InputPath, fileData.wexBIMPath);
            //} 

            return View(fileData);
        }
        public ActionResult Viewer(string FileName)
        {
           
            FileName = "a7la-home.WexBIM";
            return File(new FileData(FileName).wexBIMPath, "application/octet-stream", FileName);
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
        #endregion





    }
}