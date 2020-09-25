using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class KhoaController : Controller
    {
        //Khoa: done(chưa kt khóa ngoại)
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var tc = from tt in data.KHOAs select tt;
            return View(tc);
        }
        public ActionResult ChiTietKhoa(string id)
        {
            KHOA ct = data.KHOAs.First(m => m.MaKhoa == id);
            return View(ct);
        }
        public ActionResult ThemKhoa()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemKhoa(KHOA k,FormCollection c)
        {
            var ten = c["tenKhoa"];
            var ma = c["maKhoa"];
            var nam = c["nam"];
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên khoa không được để trống";
            else if(string.IsNullOrEmpty(ma))
                ViewData["Loi2"] = "Mã không được để trống";
            else if (string.IsNullOrEmpty(ma))
                ViewData["Loi3"] = "Năm thành lập không được để trống";
            else
            {
                k.MaKhoa = ma;
                k.TenKhoa = ten;
                k.NamThanhLap = Convert.ToInt32(nam);
                data.KHOAs.InsertOnSubmit(k);
                data.SubmitChanges();
                return RedirectToAction("Index","Khoa");
            }
            return this.ThemKhoa();
        }
        public ActionResult XoaKhoa(string id)
        {
            KHOA kh = data.KHOAs.SingleOrDefault(n => n.MaKhoa == id);
            ViewBag.Makhoa = kh.MaKhoa;
            return View(kh);
        }
        [HttpPost, ActionName("XoaKhoa")]
        public ActionResult XacNhanXoa(string id)
        {
            try
            {
                KHOA kh = data.KHOAs.SingleOrDefault(n => n.MaKhoa == id);
                ViewBag.Makhoa = kh.MaKhoa;
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.KHOAs.DeleteOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Index", "Khoa");
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                return null;
            }
            
        }
        public ActionResult SuaKhoa(string id)
        {
            var sua = data.KHOAs.First(m => m.MaKhoa == id);
            return View(sua);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaKhoa(string id, FormCollection c)
        {
            var sua = data.KHOAs.First(m => m.MaKhoa == id);
            var ten = c["tenKhoa"];
            var nam=c["NamThanhLap"];
            sua.MaKhoa = id;
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên danh mục không được để trống.";
            else if(string.IsNullOrEmpty(nam))
                ViewData["Loi2"] = "Năm thành lập không được để trống.";
            else
            {
                sua.TenKhoa = ten;
                sua.NamThanhLap = Convert.ToInt32(nam);
                UpdateModel(sua);
                data.SubmitChanges();
                return RedirectToAction("Index", "Khoa");
            }
            return this.SuaKhoa(id);
        }
    }
}
