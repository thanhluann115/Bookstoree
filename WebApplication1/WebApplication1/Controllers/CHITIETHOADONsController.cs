using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CHITIETHOADONsController : Controller
    {
        private CT25Team15Entities db = new CT25Team15Entities();

        // GET: CHITIETHOADONs
        public ActionResult Details(int id)
        {
            var cHITIETHOADONs = db.CHITIETHOADONs.Where(c=>c.MaHD == id).ToList();
            return View(cHITIETHOADONs);
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
