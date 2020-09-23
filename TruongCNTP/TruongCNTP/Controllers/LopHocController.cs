using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class LopHocController : Controller
    {
        //Lớp học: chi tiết done
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var tc = from tt in data.LOPs select tt;
            return View(tc);
        }
        public ActionResult ChiTietLopHoc(string id)
        {
            var chitiet = data.LOPs.First(m => m.MaLop == id);
            return View(chitiet);
        }
        public ActionResult ThemLopHoc()
        {
            ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            ViewBag.MaKhoaHoc = new SelectList(data.KHOAHOCs.ToList().OrderBy(n => n.MaKhoaHoc), "MaKhoaHoc", "MaKhoaHoc");
            ViewBag.MaCT = new SelectList(data.CHUONGTRINHs.ToList().OrderBy(n => n.MaCT), "MaCT", "TenCT");
            
            return View();
        }
        [HttpPost]
        public ActionResult ThemLopHoc(LOP l, FormCollection c)
        {
            ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            ViewBag.MaKhoaHoc = new SelectList(data.KHOAHOCs.ToList().OrderBy(n => n.MaKhoaHoc), "MaKhoaHoc", "MaKhoaHoc");
            ViewBag.MaCT = new SelectList(data.CHUONGTRINHs.ToList().OrderBy(n => n.MaCT), "MaCT", "TenCT");
            var tenLop = c["tenLop"];
            var maLop = c["maLop"];
            var maCt= c["maCT"];
            var maKhoa=c["maKhoa"];
            var stt = c["STT"];
            var maKH = c["maKH"];
            if (string.IsNullOrEmpty(tenLop))
                ViewData["Loi1"] = "Tên lớp không được để trống";
            else if (string.IsNullOrEmpty(maLop))
                ViewData["Loi2"] = "Mã lớp không được để trống";
            else
            {
                l.MaLop = maLop;
                l.TenLop = tenLop;
                l.MaKhoa = maKhoa;
                l.MaKhoaHoc = maKH;
                l.MaCT = maCt;
                l.STT = Convert.ToInt32(stt);
                data.LOPs.InsertOnSubmit(l);
                data.SubmitChanges();
                return RedirectToAction("Index", "LopHoc");
            }
            return this.ThemLopHoc();
        }
        public ActionResult XoaLopHoc(string id)
        {
            LOP ct = data.LOPs.SingleOrDefault(n => n.MaLop == id);
            ViewBag.MaLop = ct.MaLop;
            return View(ct);
        }
        [HttpPost, ActionName("XoaLopHoc")]
        public ActionResult XacNhanXoa(string id)
        {
            LOP ct = data.LOPs.SingleOrDefault(n => n.MaLop == id);
            ViewBag.MaLop = ct.MaLop;
            if (ct == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.LOPs.DeleteOnSubmit(ct);
            data.SubmitChanges();
            return RedirectToAction("Index", "LopHoc");
        }
        public ActionResult SuaLopHoc(string id)
        {
            ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            ViewBag.MaKhoaHoc = new SelectList(data.KHOAHOCs.ToList().OrderBy(n => n.MaKhoaHoc), "MaKhoaHoc", "MaKhoaHoc");
            ViewBag.MaCT = new SelectList(data.CHUONGTRINHs.ToList().OrderBy(n => n.MaCT), "MaCT", "TenCT");
            
            var sua = data.LOPs.First(m => m.MaLop == id);
            return View(sua);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaLopHoc(string id, FormCollection c)
        {
            ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            ViewBag.MaKhoaHoc = new SelectList(data.KHOAHOCs.ToList().OrderBy(n => n.MaKhoaHoc), "MaKhoaHoc", "MaKhoaHoc");
            ViewBag.MaCT = new SelectList(data.CHUONGTRINHs.ToList().OrderBy(n => n.MaCT), "MaCT", "TenCT");
            
            var sua = data.LOPs.First(m => m.MaLop == id);
            var ten = c["tenLop"];
            var maCT = c["MaCT"];
            var maKhoa = c["MaKhoa"];
            var maKH = c["MaKhoaHoc"];
            sua.MaLop = id;
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên lớp không được để trống.";
            else
            {
                sua.TenLop = ten;
                sua.MaKhoaHoc = maKH;
                sua.MaCT = maCT;
                sua.MaKhoa = maKhoa;
                UpdateModel(sua);
                data.SubmitChanges();
                return RedirectToAction("Index", "LopHoc");
            }
            return this.SuaLopHoc(id);
        }
    }
}
