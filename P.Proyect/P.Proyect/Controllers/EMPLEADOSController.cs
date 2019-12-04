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
    public class EMPLEADOSController : Controller
    {
        private SISTEMA_RECURSOS_HUMANOSEntities2 db = new SISTEMA_RECURSOS_HUMANOSEntities2();

        // GET: EMPLEADOS
        private SISTEMA_RECURSOS_HUMANOSEntities2 context = new SISTEMA_RECURSOS_HUMANOSEntities2();

        public ActionResult Index()
        {
            var EmpleadosList = context.EMPLEADOS.ToList();
            return View(EmpleadosList);
        }

        public ActionResult ExportEmpleados()
        {
            List<EMPLEADOS> allEmpleados = new List<EMPLEADOS>();
            allEmpleados = context.EMPLEADOS.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "EmpleadosReport.rpt"));

            var lista = from data in context.EMPLEADOS

                        select data;

            rd.SetDataSource(lista.ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "EmpleadosList.pdf");
        }


        // GET: EMPLEADOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADOS eMPLEADOS = db.EMPLEADOS.Find(id);
            if (eMPLEADOS == null)
            {
                return HttpNotFound();
            }
            return View(eMPLEADOS);
        }

        // GET: EMPLEADOS/Create
        public ActionResult Create()
        {
            ViewBag.Id_Cargo = new SelectList(db.CARGOS, "Id_Cargos", "Cargo");
            ViewBag.Id_Departamento = new SelectList(db.DEPARTAMENTOS, "Id_Departamento", "Nombre");
            return View();
        }

        // POST: EMPLEADOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Empleado,Codigo_Empleado,Nombre,Apellido,Telefono,Id_Departamento,Id_Cargo,Fecha_Ingreso,Salario,Estatus")] EMPLEADOS eMPLEADOS)
        {
            if (ModelState.IsValid)
            {
                db.EMPLEADOS.Add(eMPLEADOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Cargo = new SelectList(db.CARGOS, "Id_Cargos", "Cargo", eMPLEADOS.Id_Cargo);
            ViewBag.Id_Departamento = new SelectList(db.DEPARTAMENTOS, "Id_Departamento", "Nombre", eMPLEADOS.Id_Departamento);
            return View(eMPLEADOS);
        }

        // GET: EMPLEADOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADOS eMPLEADOS = db.EMPLEADOS.Find(id);
            if (eMPLEADOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Cargo = new SelectList(db.CARGOS, "Id_Cargos", "Cargo", eMPLEADOS.Id_Cargo);
            ViewBag.Id_Departamento = new SelectList(db.DEPARTAMENTOS, "Id_Departamento", "Nombre", eMPLEADOS.Id_Departamento);
            return View(eMPLEADOS);
        }

        // POST: EMPLEADOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Empleado,Codigo_Empleado,Nombre,Apellido,Telefono,Id_Departamento,Id_Cargo,Fecha_Ingreso,Salario,Estatus")] EMPLEADOS eMPLEADOS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eMPLEADOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Cargo = new SelectList(db.CARGOS, "Id_Cargos", "Cargo", eMPLEADOS.Id_Cargo);
            ViewBag.Id_Departamento = new SelectList(db.DEPARTAMENTOS, "Id_Departamento", "Nombre", eMPLEADOS.Id_Departamento);
            return View(eMPLEADOS);
        }

        // GET: EMPLEADOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADOS eMPLEADOS = db.EMPLEADOS.Find(id);
            if (eMPLEADOS == null)
            {
                return HttpNotFound();
            }
            return View(eMPLEADOS);
        }

        // POST: EMPLEADOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EMPLEADOS eMPLEADOS = db.EMPLEADOS.Find(id);
            db.EMPLEADOS.Remove(eMPLEADOS);
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

    }
}
