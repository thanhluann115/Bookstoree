using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DonHangController : Controller
    {
        private CT25Team15Entities db = new CT25Team15Entities();
        private List<CHITIETGIOHANG> ShoppingCart = null;
        public void ShoppingCartController()
        {
            var session = System.Web.HttpContext.Current.Session;
            if (session["ShoppingCart"] != null)
                ShoppingCart = session["ShoppingCart"] as List<CHITIETGIOHANG>;
            else
            {
                ShoppingCart = new List<CHITIETGIOHANG>();
                session["ShoppingCart"] = ShoppingCart;
            }
        }

        // GET: DonHang
        public ActionResult Index()
        {
            var model = db.HOADONs.ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            ShoppingCartController();
            ViewBag.Cart = ShoppingCart;
            return View();

        }
        public ActionResult Delete(int mahd)
        {
            HOADON model = db.HOADONs.FirstOrDefault(c => c.MaHD == mahd);
            return View(model);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Deleted(int mahd)
        {
            var chitiethd = db.CHITIETHOADONs.Where(c => c.MaHD == mahd).ToList();
            var hd = db.HOADONs.Find(mahd);
            db.CHITIETHOADONs.RemoveRange(chitiethd);
            db.HOADONs.Remove(hd);
            db.SaveChanges();
            return RedirectToAction("Index", "DONHANG");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HOADON model)
        {
            
            ValidateBill(model);

            int chieudai = ranDom(7);
            model.MaHD = chieudai;
            model.NgayLapHD = DateTime.Now;
           
           
            db.HOADONs.Add(model);
            db.SaveChanges();
            foreach (var item in ShoppingCart)
            {
           
                db.CHITIETHOADONs.Add(new CHITIETHOADON
                {
                    MaHD = chieudai,
                    MaSP = item.SANPHAM.MaSP,
                    SoLuongSP = item.SoLuong,

                });
                
                
                    var updatesoluong1 = db.SANPHAMs.FirstOrDefault(c => c.MaSP == item.SANPHAM.MaSP);
                    updatesoluong1.SoLuong = updatesoluong1.SoLuong.Value - item.SoLuong;
                db.SaveChanges();
            }
            
            Session["ShoppingCart"] = null;
            ShoppingCartController();
            ViewBag.Cart = ShoppingCart;
            return RedirectToAction("Index", "ShoppingCart");

        }
        private void ValidateBill(HOADON model)
        {
            var regex = new Regex("[0-9]{3}");
            ShoppingCartController();
            if (ShoppingCart.Count == 0)
                ModelState.AddModelError("", "there is no item  in shopping cart");
            if (!regex.IsMatch(model.SDT.ToString()))
                ModelState.AddModelError("SDT.ToString", " Wrong phone number ");


        }
        public int ranDom(int chieudai)
        {
            const string valid = "123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < chieudai--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return int.Parse(res.ToString());
        }
    }
}
