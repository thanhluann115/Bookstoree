using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class THONGTINCANHANsController : Controller
    {
        private CT25Team15Entities db = new CT25Team15Entities();

        // GET: THONGTINCANHANs
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {

            return View(db.THONGTINCANHANs.ToList());
        }
        public ActionResult Index2(string id)
        {
            var user = db.THONGTINCANHANs.Find(id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index2(THONGTINCANHAN tHONGTINCANHAN)
        {
            if (ModelState.IsValid)
            {
                var updatettcanhan = db.THONGTINCANHANs.Find(tHONGTINCANHAN.id);
                updatettcanhan.NgaySinh = tHONGTINCANHAN.NgaySinh;
                updatettcanhan.SDT = tHONGTINCANHAN.SDT;
                updatettcanhan.Ten = tHONGTINCANHAN.Ten;
                updatettcanhan.GioiTinh = tHONGTINCANHAN.GioiTinh;
                var user = db.AspNetUsers.Find(tHONGTINCANHAN.id);
                user.PhoneNumber = tHONGTINCANHAN.SDT;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", tHONGTINCANHAN.id);
            return View(tHONGTINCANHAN);
        }
        // GET: THONGTINCANHANs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGTINCANHAN tHONGTINCANHAN = db.THONGTINCANHANs.Find(id);
            if (tHONGTINCANHAN == null)
            {
                return HttpNotFound();
            }
            return View(tHONGTINCANHAN);
        }

        // GET: THONGTINCANHANs/Creat

        // GET: THONGTINCANHANs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGTINCANHAN tHONGTINCANHAN = db.THONGTINCANHANs.Find(id);
            if (tHONGTINCANHAN == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", tHONGTINCANHAN.id);
            return View(tHONGTINCANHAN);
        }

        // POST: THONGTINCANHANs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(THONGTINCANHAN tHONGTINCANHAN)
        {
            checktextbox(tHONGTINCANHAN);
            if (ModelState.IsValid)
            {
                db.Entry(tHONGTINCANHAN).State = EntityState.Modified;
                var user = db.AspNetUsers.Find(tHONGTINCANHAN.id);
                user.PhoneNumber = tHONGTINCANHAN.SDT;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", tHONGTINCANHAN.id);
            return View(tHONGTINCANHAN);
        }
        public void checktextbox(THONGTINCANHAN tHONGTINCANHAN)
        {
            var regexItem = new Regex("^[aAàÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬbBcCdDđĐeEèÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆfFgGhHiIìÌỉỈĩĨíÍịỊjJkKlLmMnNoOòÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢpPqQrRsStTuUùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰvVwWxXyYỳỲỷỶỹỸýÝỵỴzZ]");
            var diachi = new Regex("^[aAàÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬbBcCdDđĐeEèÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆfFgGhHiIìÌỉỈĩĨíÍịỊjJkKlLmMnNoOòÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢpPqQrRsStTuUùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰvVwWxXyYỳỲỷỶỹỸýÝỵỴzZ]");
            var sdt = new Regex("^[0-9]*$");
            if (tHONGTINCANHAN.Ten == null || tHONGTINCANHAN.SDT == null || tHONGTINCANHAN.GioiTinh == null || tHONGTINCANHAN.NgaySinh == null)
            {
                ModelState.AddModelError("", "Thông tin chưa nhập đầy đủ");
            }
            else if(regexItem.IsMatch(tHONGTINCANHAN.Ten)==false)
            {
                ModelState.AddModelError("", "Tên có chứ số và kí tự đặc biệt");
            }
            else if(tHONGTINCANHAN.Ten.Trim().Length == 0)
            {
                ModelState.AddModelError("", "Tên chứa khoảng trắng");
            }
            else if(regexItem.IsMatch(tHONGTINCANHAN.GioiTinh)==null)
            {
                ModelState.AddModelError("", "Giới tính có chứa số và kí tự đặc biệt");
            }
            else if(tHONGTINCANHAN.GioiTinh.Trim().Length==0)
            {
                ModelState.AddModelError("", "Giới tính chứa khoảng trắng");
            }




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
