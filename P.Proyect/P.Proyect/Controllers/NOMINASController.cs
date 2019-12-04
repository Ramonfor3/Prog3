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
    public class NOMINASController : Controller
    {
        private SISTEMA_RECURSOS_HUMANOSEntities2 db = new SISTEMA_RECURSOS_HUMANOSEntities2();
        private SISTEMA_RECURSOS_HUMANOSEntities db2 = new SISTEMA_RECURSOS_HUMANOSEntities();

        // GET: NOMINAS
        public ActionResult Index()
        {
            var EActivos = from datos in db2.EMPLEADOS where datos.Estatus == "Activos" select datos;
            var nomina = EActivos.Sum(item => item.Salario);
            ViewBag.Nomina = nomina;
            return View(db.NOMINAS.ToList());
        }

        // GET: NOMINAS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOMINAS nOMINAS = db.NOMINAS.Find(id);
            if (nOMINAS == null)
            {
                return HttpNotFound();
            }
            return View(nOMINAS);
        }

        // GET: NOMINAS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NOMINAS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Nomina,Año,Mes,Monto_Total")] NOMINAS nOMINAS)
        {
            if (ModelState.IsValid)
            {
                db.NOMINAS.Add(nOMINAS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nOMINAS);
        }

        // GET: NOMINAS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOMINAS nOMINAS = db.NOMINAS.Find(id);
            if (nOMINAS == null)
            {
                return HttpNotFound();
            }
            return View(nOMINAS);
        }

        // POST: NOMINAS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Nomina,Año,Mes,Monto_Total")] NOMINAS nOMINAS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nOMINAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nOMINAS);
        }

        // GET: NOMINAS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOMINAS nOMINAS = db.NOMINAS.Find(id);
            if (nOMINAS == null)
            {
                return HttpNotFound();
            }
            return View(nOMINAS);
        }

        // POST: NOMINAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOMINAS nOMINAS = db.NOMINAS.Find(id);
            db.NOMINAS.Remove(nOMINAS);
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



        public ActionResult ExportNominas()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "NominaReport.rpt"));

            var lista = from data in context.NOMINAS

                        select data;

            rd.SetDataSource(lista.ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Nomina.pdf");
        }
    }
}
