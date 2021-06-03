using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Transactions;

namespace WebApplication1.Controllers
{
    public class SANPHAMsController : Controller
    {
        private CT25Team15Entities db = new CT25Team15Entities();

        // GET: SANPHAMs
        public async Task<ActionResult> Index()
        {
            return View(await db.SANPHAMs.ToListAsync());
        }
        public async Task<ActionResult> Index2()
        {
            return View(await db.SANPHAMs.ToListAsync());
        }
        // GET: SANPHAMs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = await db.SANPHAMs.FindAsync(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // GET: SANPHAMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SANPHAMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SANPHAM model, HttpPostedFileBase Picture)
        {
            ValidateProduct(model);
            if (ModelState.IsValid)
            {
                if (Picture != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        db.SANPHAMs.Add(model);
                        db.SaveChanges();

                        var path = Server.MapPath(PICTURE_PATH);
                        Picture.SaveAs(path + model.MaSP);

                        scope.Complete();
                        return RedirectToAction("Index");
                    }
                }
                else ModelState.AddModelError("", "Picture not found!!! ");

            }

            return View(model);
        }
        private const string PICTURE_PATH = "~/img/Products/";
        public ActionResult picture(int MaSP)
        {
            var path = Server.MapPath(PICTURE_PATH);
            return File(path + MaSP, "images");
        }
        private void ValidateProduct(SANPHAM model)
        {
            if (model.Gia < 0)
                ModelState.AddModelError("Gia", "Price is less than Zero");
        }

        // GET: SANPHAMs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = await db.SANPHAMs.FindAsync(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // POST: SANPHAMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaSP,TenSP,TacGia,NamSX,NhaSX,LoaiSP,Gia,Noidung,NgayThem")] SANPHAM sANPHAM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sANPHAM).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sANPHAM);
        }

        // GET: SANPHAMs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = await db.SANPHAMs.FindAsync(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // POST: SANPHAMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SANPHAM sANPHAM = await db.SANPHAMs.FindAsync(id);
            db.SANPHAMs.Remove(sANPHAM);
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
