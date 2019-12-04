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
    public class LICENCIAsController : Controller
    {
        private SISTEMA_RECURSOS_HUMANOSEntities2 db = new SISTEMA_RECURSOS_HUMANOSEntities2();

        // GET: LICENCIAs
        public ActionResult Index()
        {
            var lICENCIA = db.LICENCIA.Include(l => l.EMPLEADOS);
            return View(lICENCIA.ToList());
        }

        // GET: LICENCIAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LICENCIA lICENCIA = db.LICENCIA.Find(id);
            if (lICENCIA == null)
            {
                return HttpNotFound();
            }
            return View(lICENCIA);
        }

        // GET: LICENCIAs/Create
        public ActionResult Create()
        {
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre");
            return View();
        }

        // POST: LICENCIAs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Licencia,Id_Empleado,Desde,Hasta,motivo,Comentarios")] LICENCIA lICENCIA)
        {
            if (ModelState.IsValid)
            {
                db.LICENCIA.Add(lICENCIA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", lICENCIA.Id_Empleado);
            return View(lICENCIA);
        }

        // GET: LICENCIAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LICENCIA lICENCIA = db.LICENCIA.Find(id);
            if (lICENCIA == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", lICENCIA.Id_Empleado);
            return View(lICENCIA);
        }

        // POST: LICENCIAs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Licencia,Id_Empleado,Desde,Hasta,motivo,Comentarios")] LICENCIA lICENCIA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lICENCIA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Empleado = new SelectList(db.EMPLEADOS, "Id_Empleado", "Nombre", lICENCIA.Id_Empleado);
            return View(lICENCIA);
        }

        // GET: LICENCIAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LICENCIA lICENCIA = db.LICENCIA.Find(id);
            if (lICENCIA == null)
            {
                return HttpNotFound();
            }
            return View(lICENCIA);
        }

        // POST: LICENCIAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LICENCIA lICENCIA = db.LICENCIA.Find(id);
            db.LICENCIA.Remove(lICENCIA);
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



        public ActionResult ExportLicencia()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "LicenciaReport.rpt"));

            var lista = from data in context.LICENCIA

                        select data;

            rd.SetDataSource(lista.ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Licencia.pdf");
        }
    }
}
