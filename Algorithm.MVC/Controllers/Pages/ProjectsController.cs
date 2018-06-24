using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Algorithm.DB;
using Algorithm.DB.Models;
using Algorithm.DB.ViewModels;
using Algorithm.MVC.DAL;
using Microsoft.AspNet.Identity;
using Bim.Domain.General;
using Bim.Application.IRCWood.IRC;
using Bim.Domain.Ifc;
using Xbim.Ifc4.SharedBldgElements;
using Bim.Application.IRCWood.Physical;
using Bim.BOQ;
using Algorithm.MVC.Helper;
using Bim.IO;

namespace Algorithm.MVC.Controllers
{
    public class ProjectsController : Controller
    {
        private AlgorithmDB db = new AlgorithmDB();

        #region Design
        public void Design(Guid? Id)
        {
            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);
            var userId = User.Identity.GetUserId();
            var file = new FileData(proj.FileName, userId);
            ///conteeeeeeeeeent

            StudTable.FilePath = Server.MapPath(@"~/App_Data\Tables\StudSpacingTable.csv");

            Table502_5.HeadersTableExteriorPath = Server.MapPath(@"~/App_Data\Tables\table502.5(1).csv");
            Table502_5.HeadersTableInteriorPath = Server.MapPath(@"~/App_Data\Tables\table502.5(2).csv");

            Table502_3_1.JoistTableLivingAreasPath = Server.MapPath(@"~/App_Data\Tables\table502.3.1(2).csv");
            Table502_3_1.JoistTableSleepingAreasPath = Server.MapPath(@"~/App_Data\Tables\table502.3.1(1).csv");

            using (IfModel model = IfModel.Open(file.InputPath))
            {
                Bim.Domain.Configuration.Startup.Configuration(model);

                model.Delete<IfcBeam>();
                model.Delete<IfcColumn>();
                WoodFrame wf = new WoodFrame(model);
                wf.FrameWalls();
                model.Delete<IfcWall>();
                model.Delete<IfcSlab>();

                model.Save(file.OutputPath);
                // save the Structure WexBim handler.
                IfcHandler.ToWexBim(file.OutputPath, file.WexBIMPathStr);

                ////
                GeometryCollection GC1 = new GeometryCollection();
                GC1.AddToCollection(model.Instances.OfType<IfJoist>());
                GC1.AddToCollection(model.Instances.OfType<IfStud>());
                GC1.AddToCollection(model.Instances.OfType<IfSill>());
                //saving Excel bytes
                GC1.ToExcel(file.BoqPath,file.FileName+"BOQ");

                proj.DesignState = DesignState.Designed;
                uow.SaveChanges();
                ///result
                
            }
        }
        #endregion

        #region Edit Settings
        public void EditSettings(WoodSetup settings)
        {
            UnitOfWork uow = new UnitOfWork();
           var projSettings= uow.Projects.FindById(settings.ProjectId);

            //set new data from user


        }

        public ActionResult Gallery(Guid Id)
        {
            UnitOfWork uow = new UnitOfWork();
            var proj = uow.Projects.FindById(Id);
            proj.AddToGallery = !proj.AddToGallery;
            uow.Projects.Update(proj);
            uow.SaveChanges();
            return RedirectToAction("Index", "Gallery");
        }
        #endregion
        // GET: Projects
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            UnitOfWork uow = new UnitOfWork();
            var projects = uow.Projects.FindBy(e => e.UserId == userId);

            return PartialView(@"~/Views/Projects/Partial/View.cshtml", ProjectVM.Load(projects));
        }


        // GET: Projects/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        // GET: Projects/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,FileName,Title,Description,Thumbnail,DesignState,AddToGallery,DesignOptions")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
