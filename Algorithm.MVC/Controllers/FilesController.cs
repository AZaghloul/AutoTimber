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


            //if found the same file redirect to the show Action
            var file = files.FirstOrDefault();

            if (file != null && file.ContentLength > 0)
            {
                var userId = User.Identity.GetUserId();

                // check if directoryExists;
                FileData.CheckDirectory(userId);

                //create FileData object to hold all file paths
                var fileData = new FileData(file.FileName, userId);

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


        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public FileContentResult Archive(Guid? Id)
        {
            var userId = User.Identity.GetUserId();
            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);

            var file = new FileData(proj.FileName, userId);
            //To Archive
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    ziparchive.CreateEntryFromFile(file.InputPath, file.FileName);
                    ziparchive.CreateEntryFromFile(file.OutputPath, file.OutputName);
                    ziparchive.CreateEntryFromFile(file.BoqPath, file.BoqName);

                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
            ///

        }
        public ActionResult Download(Guid? Id)
        {


            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);
            var userId = User.Identity.GetUserId();
            var file = new FileData(proj.FileName, userId);
            //To Archive
            var stream = System.IO.File.Open(file.OutputPath, FileMode.Open);
            byte[] fileContent = ReadFully(stream);
            stream.Close();
            return File(fileContent, FileData.TextContentType, file.OutputName);

          
            ///

        }

        public FileContentResult Boq(Guid? Id)
        {
            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);
            var file = new FileData(proj.FileName, proj.UserId);
            var stream = System.IO.File.Open(file.BoqPath, FileMode.Open);
            byte[] fileContent =   ReadFully(stream);
            stream.Close();
            return File(fileContent, FileData.ExcelContentType, file.BoqName);
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

            var userId = User.Identity.GetUserId();
            var file = new FileData(project.FileName, userId);
            //check wexBim files Exists
            if (!file.Exists(FileType.WexBIMPathArc))
            {
                IfcHandler.ToWexBim(file.InputPath, file.WexBIMPathArc);
            }
            else if (!file.Exists(FileType.WexBIMPAthStr))
            {
                IfcHandler.ToWexBim(file.InputPath, file.WexBIMPathStr);
            }


            return View(file);
        }

        public ActionResult Viewer(string FileName)
        {
            if (TempData["Structure"] == null)
            {
                TempData["Structure"] = true;
            }


            var userId = User.Identity.GetUserId();
            var file = new FileData(FileName, userId);

            if ((bool)TempData["Structure"] == true)
            {
                TempData["Structure"] = false;
                return File(file.WexBIMPathStr, "application/octet-stream", file.FileName);

            }
            // return str files
            else
            {

                TempData["Structure"] = null;
                return File(file.WexBIMPathArc, "application/octet-stream", file.FileName);
            }
        }


        #endregion


    }
}