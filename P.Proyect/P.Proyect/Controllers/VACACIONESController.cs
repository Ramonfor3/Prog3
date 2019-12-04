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
    public class VACACIONESController : Controller
    {
        private SISTEMA_RECURSOS_HUMANOSEntities2 db = new SISTEMA_RECURSOS_HUMANOSEntities2();

        // GET: VACACIONES
        public ActionResult Index()
        {
            var vACACIONES = db.VACACIONES.Include(v => v.EMPLEADOS);
            return View(vACACIONES.ToList());
        }

        // GET: VACACIONES/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VACACIONES vACACIONES = db.VACACIONES.Find(id);
            if (vACACIONES == null)
            {
                return HttpNotFound();
            }
            return View(vACACIONES);
        }

        // GET: VACACIONES/Create
        public ActionResult Create()
        {
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre");
            return View();
        }

        // POST: VACACIONES/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Vacaciones,Id_Empleado,Desde,Hasta,Correspondiente,Comentarios")] VACACIONES vACACIONES)
        {
            if (ModelState.IsValid)
            {
                db.VACACIONES.Add(vACACIONES);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", vACACIONES.Id_Empleado);
            return View(vACACIONES);
        }

        // GET: VACACIONES/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VACACIONES vACACIONES = db.VACACIONES.Find(id);
            if (vACACIONES == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", vACACIONES.Id_Empleado);
            return View(vACACIONES);
        }

        // POST: VACACIONES/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Vacaciones,Id_Empleado,Desde,Hasta,Correspondiente,Comentarios")] VACACIONES vACACIONES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vACACIONES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", vACACIONES.Id_Empleado);
            return View(vACACIONES);
        }

        // GET: VACACIONES/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VACACIONES vACACIONES = db.VACACIONES.Find(id);
            if (vACACIONES == null)
            {
                return HttpNotFound();
            }
            return View(vACACIONES);
        }

        // POST: VACACIONES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VACACIONES vACACIONES = db.VACACIONES.Find(id);
            db.VACACIONES.Remove(vACACIONES);
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

        private SISTEMA_RECURSOS_HUMANOSEntities2 context = new SISTEMA_RECURSOS_HUMANOSEntities2();



        public ActionResult VacacionesCargo()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "VacacionesReport.rpt"));

            var lista = from data in context.VACACIONES

                        select data;

            rd.SetDataSource(lista.ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Vacaciones.pdf");
        }
    }
}
