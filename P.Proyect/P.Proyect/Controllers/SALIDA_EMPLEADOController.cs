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
    public class SALIDA_EMPLEADOController : Controller
    {
        private SISTEMA_RECURSOS_HUMANOSEntities2 db = new SISTEMA_RECURSOS_HUMANOSEntities2();

        // GET: SALIDA_EMPLEADO
        public ActionResult Index()
        {
            var sALIDA_EMPLEADO = db.SALIDA_EMPLEADO.Include(s => s.EMPLEADOS);
            return View(sALIDA_EMPLEADO.ToList());
        }

        // GET: SALIDA_EMPLEADO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALIDA_EMPLEADO sALIDA_EMPLEADO = db.SALIDA_EMPLEADO.Find(id);
            if (sALIDA_EMPLEADO == null)
            {
                return HttpNotFound();
            }
            return View(sALIDA_EMPLEADO);
        }

        // GET: SALIDA_EMPLEADO/Create
        public ActionResult Create()
        {
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre");
            return View();
        }

        // POST: SALIDA_EMPLEADO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Salida,Id_Empleado,Tipo_Salida,Motivo,Fecha_Salida")] SALIDA_EMPLEADO sALIDA_EMPLEADO)
        {
            if (ModelState.IsValid)
            {
                db.SALIDA_EMPLEADO.Add(sALIDA_EMPLEADO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", sALIDA_EMPLEADO.Id_Empleado);
            return View(sALIDA_EMPLEADO);
        }

        // GET: SALIDA_EMPLEADO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALIDA_EMPLEADO sALIDA_EMPLEADO = db.SALIDA_EMPLEADO.Find(id);
            if (sALIDA_EMPLEADO == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", sALIDA_EMPLEADO.Id_Empleado);
            return View(sALIDA_EMPLEADO);
        }

        // POST: SALIDA_EMPLEADO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Salida,Id_Empleado,Tipo_Salida,Motivo,Fecha_Salida")] SALIDA_EMPLEADO sALIDA_EMPLEADO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sALIDA_EMPLEADO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", sALIDA_EMPLEADO.Id_Empleado);
            return View(sALIDA_EMPLEADO);
        }

        // GET: SALIDA_EMPLEADO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALIDA_EMPLEADO sALIDA_EMPLEADO = db.SALIDA_EMPLEADO.Find(id);
            if (sALIDA_EMPLEADO == null)
            {
                return HttpNotFound();
            }
            return View(sALIDA_EMPLEADO);
        }

        // POST: SALIDA_EMPLEADO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SALIDA_EMPLEADO sALIDA_EMPLEADO = db.SALIDA_EMPLEADO.Find(id);
            db.SALIDA_EMPLEADO.Remove(sALIDA_EMPLEADO);
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



        public ActionResult ExportSalidadEmpleado()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "SalidaEmpleadoReport.rpt"));

            var lista = from data in context.CARGOS

                        select data;

            rd.SetDataSource(lista.ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Salida de empleado.pdf");
        }
    }
}
