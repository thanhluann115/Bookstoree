using System;
using System.Collections;
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
    public class ShoppingCartController : Controller
    {
        private CT25Team15Entities db = new CT25Team15Entities();
        private List<CHITIETGIOHANG> ShoppingCart = null;

        public ShoppingCartController()
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

        // GET: ShoppingCart 
        public ActionResult Index()

        {
            var hashtable = new Hashtable(); // cho phép tăng số lượng có cùng ID
            foreach (var CHITIETGIOHANG in ShoppingCart)
            {
                if (hashtable[CHITIETGIOHANG.SANPHAM.MaSP] != null)
                {
                    (hashtable[CHITIETGIOHANG.SANPHAM.MaSP] as CHITIETGIOHANG).SoLuong += CHITIETGIOHANG.SoLuong;
                }
                else hashtable[CHITIETGIOHANG.SANPHAM.MaSP] = CHITIETGIOHANG;
            }
            ShoppingCart.Clear();
            foreach (CHITIETGIOHANG cHITIETGIOHANG in hashtable.Values)
                ShoppingCart.Add(cHITIETGIOHANG);

            return View(ShoppingCart);
        }



        // GET: ShoppingCart/Create
        [HttpPost]

        public ActionResult Create(int MaSP, int SoLuong)
        {
            var sANPHAM = db.SANPHAMs.Find(MaSP);
            ShoppingCart.Add(new CHITIETGIOHANG
            {
                SANPHAM = sANPHAM,
                SoLuong = SoLuong

            });
            return RedirectToAction("Index");
        }
        public ActionResult Created(FormCollection form)
        {
            int MaSP = int.Parse(form["MaSP"]);
            var sANPHAM = db.SANPHAMs.Find(MaSP);
            ShoppingCart.Add(new CHITIETGIOHANG
            {
                SANPHAM = sANPHAM,
                SoLuong = 1
            });
            return RedirectToAction("Index");
        }

        // GET: ShoppingCart/Edit/5
        [HttpPost]
        public ActionResult Edit(int[] SANPHAM_MaSP, int[] SoLuong)
        {
            ShoppingCart.Clear();
            if (SANPHAM_MaSP != null)
                for (int i = 0; i < SANPHAM_MaSP.Length; i++)
                    if (SoLuong[i] > 0)
                    {
                        var SANPHAMs = db.SANPHAMs.Find(SANPHAM_MaSP[i]);
                        ShoppingCart.Add(new CHITIETGIOHANG
                        {
                            SANPHAM = SANPHAMs,
                            SoLuong = SoLuong[i]

                        });
                        Session["ShoppingCart"] = ShoppingCart;
                    }
            return RedirectToAction("Index");
        }



        // GET: ShoppingCart/Delete/5
        public ActionResult Delete()
        {

            ShoppingCart.Clear();
            Session["ShoppingCart"] = ShoppingCart;
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