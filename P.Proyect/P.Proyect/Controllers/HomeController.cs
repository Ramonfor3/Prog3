using CrystalDecisions.CrystalReports.Engine;
using P.Proyect.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P.Proyect.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {

            return View();
        }


    }
}