using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using P.Proyect.Models;

namespace P.Proyect.Controllers
{
    public class CARGOSController : Controller
    {
        private SISTEMA_RECURSOS_HUMANOSEntities2 db = new SISTEMA_RECURSOS_HUMANOSEntities2();





        
        // GET: CARGOS
        public ActionResult Index()
        {
            return View(db.CARGOS.ToList());
        }

        // GET: CARGOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARGOS cARGOS = db.CARGOS.Find(id);
            if (cARGOS == null)
            {
                return HttpNotFound();
            }
            return View(cARGOS);
        }

        // GET: CARGOS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CARGOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Cargos,Codigo_Cargo,Cargo")] CARGOS cARGOS)
        {
            if (ModelState.IsValid)
            {
                db.CARGOS.Add(cARGOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cARGOS);
        }

        // GET: CARGOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARGOS cARGOS = db.CARGOS.Find(id);
            if (cARGOS == null)
            {
                return HttpNotFound();
            }
            return View(cARGOS);
        }

        // POST: CARGOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Cargos,Codigo_Cargo,Cargo")] CARGOS cARGOS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cARGOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cARGOS);
        }

        // GET: CARGOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARGOS cARGOS = db.CARGOS.Find(id);
            if (cARGOS == null)
            {
                return HttpNotFound();
            }
            return View(cARGOS);
        }

        // POST: CARGOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CARGOS cARGOS = db.CARGOS.Find(id);
            db.CARGOS.Remove(cARGOS);
            db.SaveChanges();
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



        private SISTEMA_RECURSOS_HUMANOSEntities context = new SISTEMA_RECURSOS_HUMANOSEntities();

        

        public ActionResult ExportCargo()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "CargoReport.rpt"));

            var lista = from data in context.CARGOS

                        select data;

            rd.SetDataSource(lista.ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Cargo.pdf");
        }

    }

}
