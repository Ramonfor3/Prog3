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
    public class PERMISOSController : Controller
    {
        private SISTEMA_RECURSOS_HUMANOSEntities2 db = new SISTEMA_RECURSOS_HUMANOSEntities2();

        // GET: PERMISOS
        public ActionResult Index()
        {
            var pERMISOS = db.PERMISOS.Include(p => p.EMPLEADOS);
            return View(pERMISOS.ToList());
        }

        // GET: PERMISOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PERMISOS pERMISOS = db.PERMISOS.Find(id);
            if (pERMISOS == null)
            {
                return HttpNotFound();
            }
            return View(pERMISOS);
        }

        // GET: PERMISOS/Create
        public ActionResult Create()
        {
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre");
            return View();
        }

        // POST: PERMISOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Permiso,Id_Empleado,Desde,Hasta,Correspondiente,Comentarios")] PERMISOS pERMISOS)
        {
            if (ModelState.IsValid)
            {
                db.PERMISOS.Add(pERMISOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", pERMISOS.Id_Empleado);
            return View(pERMISOS);
        }

        // GET: PERMISOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PERMISOS pERMISOS = db.PERMISOS.Find(id);
            if (pERMISOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", pERMISOS.Id_Empleado);
            return View(pERMISOS);
        }

        // POST: PERMISOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Permiso,Id_Empleado,Desde,Hasta,Correspondiente,Comentarios")] PERMISOS pERMISOS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pERMISOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", pERMISOS.Id_Empleado);
            return View(pERMISOS);
        }

        // GET: PERMISOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PERMISOS pERMISOS = db.PERMISOS.Find(id);
            if (pERMISOS == null)
            {
                return HttpNotFound();
            }
            return View(pERMISOS);
        }

        // POST: PERMISOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PERMISOS pERMISOS = db.PERMISOS.Find(id);
            db.PERMISOS.Remove(pERMISOS);
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



        public ActionResult ExportPermiso()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "PermisosReport.rpt"));

            var lista = from data in context.PERMISOS

                        select data;

            rd.SetDataSource(lista.ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Permisos.pdf");
        }
    }
}
