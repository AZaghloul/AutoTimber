using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Algorithm.DB;
using Bim.Domain.General;

namespace Algorithm.MVC.Controllers
{
    public class DesignOptionsController : Controller
    {
        private AlgorithmDB db = new AlgorithmDB();

        // GET: DesignOptions
        public async Task<ActionResult> Index()
        {
            return View(await db.DesignOptions.ToListAsync());
        }

        // GET: DesignOptions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignOptions designOptions = await db.DesignOptions.FindAsync(id);
            if (designOptions == null)
            {
                return HttpNotFound();
            }
            return View(designOptions);
        }

        // GET: DesignOptions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DesignOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DesignOptions designOptions)
        {
            if (ModelState.IsValid)
            {
                designOptions.Id = Guid.NewGuid();
                db.DesignOptions.Add(designOptions);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(designOptions);
        }

        // GET: DesignOptions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignOptions designOptions = await db.DesignOptions.FindAsync(id);
            if (designOptions == null)
            {
                return HttpNotFound();
            }
            return View(designOptions);
        }

        // POST: DesignOptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( DesignOptions designOptions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(designOptions).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(designOptions);
        }

        // GET: DesignOptions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignOptions designOptions = await db.DesignOptions.FindAsync(id);
            if (designOptions == null)
            {
                return HttpNotFound();
            }
            return View(designOptions);
        }

        // POST: DesignOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            DesignOptions designOptions = await db.DesignOptions.FindAsync(id);
            db.DesignOptions.Remove(designOptions);
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
