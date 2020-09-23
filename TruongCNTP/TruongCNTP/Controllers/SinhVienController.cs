using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class SinhVienController : Controller
    {
        //
        // GET: /SinhVien/
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var tc = from tt in data.SINHVIENs select tt;
            return View(tc);
        }
        public ActionResult ChiTietSinhVien(string id)
        {
            var s = data.SINHVIENs.Where(n => n.MaSV == id).First();
            return View(s);
        }
        public ActionResult ThemSinhVien()
        {
            ViewBag.MaLop = new SelectList(data.LOPs.ToList().OrderBy(n => n.TenLop), "MaLop", "TenLop");
            return View();
        }
        [HttpPost]
        public ActionResult ThemSinhVien(SINHVIEN  ct, FormCollection c)
        {
            ViewBag.MaLop = new SelectList(data.LOPs.ToList().OrderBy(n => n.TenLop), "MaLop", "TenLop");
            var ten = c["tenSV"];
            var ma = c["maSV"];
            var ns = c["namSinh"];
            var diaChi = c["diaChi"];
            var email = c["email"];
            var sdt = c["sdt"];
            var malop = c["MaLop"];
            var pass1 = c["mk"];
            var pass2 = c["mk2"];

            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên chương trình không được để trống";
            else if (string.IsNullOrEmpty(ma))
                ViewData["Loi2"] = "Mã chương trình không được để trống";
            else
            {
                ct.MaSV = ma;
                ct.TenSV = ten;
                data.SINHVIENs.InsertOnSubmit(ct);
                data.SubmitChanges();
                return RedirectToAction("Index", "SinhVien");
            }
            return this.ThemSinhVien();
        }
        public ActionResult XoaSinhVien(string id)
        {
            SINHVIEN ct = data.SINHVIENs.SingleOrDefault(n => n.MaSV == id);
            ViewBag.Masv = ct.MaSV;
            return View(ct);
        }
        [HttpPost, ActionName("XoaSinhVien")]
        public ActionResult XacNhanXoa(string id)
        {
            SINHVIEN ct = data.SINHVIENs.SingleOrDefault(n => n.MaSV == id);
            ViewBag.Masv = ct.MaSV;
            if (ct == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SINHVIENs.DeleteOnSubmit(ct);
            data.SubmitChanges();
            return RedirectToAction("Index", "SinhVien");
        }
        public ActionResult SuaSinhVien(string id)
        {
            ViewBag.MaLop = new SelectList(data.LOPs.ToList().OrderBy(n => n.TenLop), "MaLop", "TenLop");
            var sua = data.SINHVIENs.First(m => m.MaSV == id);
            return View(sua);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSinhVien(string id, FormCollection c)
        {
            ViewBag.MaLop = new SelectList(data.LOPs.ToList().OrderBy(n => n.TenLop), "MaLop", "TenLop");
            var sua = data.SINHVIENs.First(m => m.MaSV == id);
            var ten = c["tenSV"];
            var ns = c["namSinh"];
            var diaChi = c["diaChi"];
            var email = c["email"];
            var sdt = c["sdt"];
            var malop = c["MaLop"];
            var pass1 = c["mk"];
            sua.MaSV = id;
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên chương trình không được để trống.";
            else
            {
                sua.TenSV = ten;
                sua.NamSinh = Convert.ToInt32(ns);
                sua.SDT = sdt;
                sua.Email = email;
                sua.MaLop = malop;
                //hình,mật khẩu
                UpdateModel(sua);
                data.SubmitChanges();
                return RedirectToAction("Index", "SinhVien");
            }
            return this.SuaSinhVien(id);
        }
    }
}
