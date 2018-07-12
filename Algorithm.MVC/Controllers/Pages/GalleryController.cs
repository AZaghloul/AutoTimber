using AutoTimber.DB.ViewModels;
using AutoTimber.MVC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoTimber.MVC.Controllers.Pages
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            var galleryVm = new List<GalleryVM>();
            UnitOfWork uow = new UnitOfWork();
            var projects = uow.Projects.FindBy(e => e.AddToGallery == true);

            foreach (var proj in projects)
            {
                var glryVM = new GalleryVM();
                var user = uow.Users.FindById(proj.UserId);

                glryVM.UserName = user.FirstName + " " + user.LastName;
                glryVM.Id = proj.Id;
                glryVM.Title = proj.Title;
                glryVM.Description = proj.Description;
                glryVM.Thumbnail = proj.Thumbnail;
                galleryVm.Add(glryVM);
            }

            return View(galleryVm);
        }
    }
}