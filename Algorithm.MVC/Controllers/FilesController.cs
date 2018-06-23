using Bim.IO;

using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Algorithm.DB.ViewModels;
using System.Web;
using System.Collections.Generic;
using Algorithm.MVC.DAL;
using Algorithm.DB;
using Algorithm.DB.Models;
using Microsoft.AspNet.Identity;
using Algorithm.MVC.Helper;
using System.Linq;
using System.IO.Compression;

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
            var file = files.FirstOrDefault();

            if (file != null && file.ContentLength > 0)
            {

                //create FileData object to hold all file paths
                fileData = new FileData(file.FileName);

                //write the file to the desk
                file.SaveAs(fileData.InputPath);
                //Convert Arc file to wexBim;
                IfcHandler.ToWexBim(fileData.InputPath, fileData.WexBIMPathArc);

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
                Response.StatusCode = (int)HttpStatusCode.OK;

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return RedirectToAction("Index", "Dashboard");
            }

        }

        #endregion

        #region Download

        public FileContentResult Excel(Guid Id)
        {
            return null;
        }
        public FileContentResult Archive(Guid? Id)
        {
            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);
            var file = new FileData(proj.FileName);
            //To Archive
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    ziparchive.CreateEntryFromFile(file.InputPath, file.FileName);
                    ziparchive.CreateEntryFromFile(file.InputPath, file.FileName);

                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
            ///

        }
        public ActionResult Download(Guid? Id)
        {
            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);
            var file = new FileData(proj.FileName);
            //To Archive
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    ziparchive.CreateEntryFromFile(file.InputPath, file.FileName);
                    ziparchive.CreateEntryFromFile(file.InputPath, file.FileName);
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
            ///

        }

        public ActionResult Boq(Guid? Id)
        {
            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);
            var file = new FileData(proj.FileName);
            //To Archive
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    ziparchive.CreateEntryFromFile(file.InputPath, file.FileName);
                    ziparchive.CreateEntryFromFile(file.InputPath, file.FileName);
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
            ///

        }
        #endregion

        #region Show Models Methods

        public ActionResult Show(Guid? Id)
        {
            
            if (Id == null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            UnitOfWork uow = new UnitOfWork();
            var project = uow.Projects.FindById(Id);

            var fileData = new FileData(project.FileName);
            //check wexBim files Exists
            if (!fileData.Exists(FileType.WexBIMPathArc))
            {
                IfcHandler.ToWexBim(fileData.InputPath, fileData.WexBIMPathArc);
            }
            else if (!fileData.Exists(FileType.WexBIMPAthStr))
            {
                IfcHandler.ToWexBim(fileData.InputPath, fileData.WexBIMPathStr);
            }

            
            return View(fileData);
        }

        public ActionResult Viewer(string FileName)
        {
            if (TempData["Structure"]==null)
            {
                TempData["Structure"] = true;
            }
            
           
            var m = new FileData(FileName);
           
            if ((bool)TempData["Structure"]==true)
            {
                TempData["Structure"] = false;
                return File(m.WexBIMPathStr, "application/octet-stream", m.InputName);
               
            }
            // return str files
            else
            {
                ////if str file found retunrn it
                //if (file.Exists(FileType.WexBIMPAthStr))
                //{

                //}


                ////if str file found retunrn null
                //else
                //{
                //    return null;
                //}
                TempData["Structure"] = null;
                return File(m.WexBIMPathArc, "application/octet-stream", m.InputName);
               
            }
           

        }


        #endregion

        #region Design Methods

        public ActionResult Design()
        {
            return View();
        }
        #endregion
    }
}