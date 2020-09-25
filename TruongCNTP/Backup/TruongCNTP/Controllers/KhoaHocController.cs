using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class KhoaHocController : Controller
    {
        //Khóa học: chi tiết, xóa, thêm,sửa done
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var tc = from tt in data.KHOAHOCs select tt;
            return View(tc);
        }
        public ActionResult ChiTietKhoaHoc(string id)
        {
            var chitiet = data.KHOAHOCs.First(m => m.MaKhoaHoc == id);
            return View(chitiet);
        }
        public ActionResult ThemKhoaHoc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemKhoaHoc(KHOAHOC kh, FormCollection c)
        {
            var nambd = c["nambd"];
            var namkt = c["namkt"];
            var ma = c["maKhoaHoc"];
            if (string.IsNullOrEmpty(ma))
                ViewData["Loi1"] = "Mã khóa học không được để trống";
            else if (string.IsNullOrEmpty(nambd))
                ViewData["Loi2"] = "Năm bắt đầu không được để trống";
            else if (string.IsNullOrEmpty(namkt))
                ViewData["Loi3"] = "Năm kết thúc không được để trống";
            else
            {
                kh.MaKhoaHoc = ma;
                kh.NamBatDau = Convert.ToInt32(nambd);
                kh.NamKetThuc = Convert.ToInt32(namkt);
                data.KHOAHOCs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Index", "KhoaHoc");
            }
            return this.ThemKhoaHoc();
        }
        public ActionResult XoaKhoaHoc(string id)
        {
            KHOAHOC ct = data.KHOAHOCs.SingleOrDefault(n => n.MaKhoaHoc == id);
            ViewBag.Makhoahoc = ct.MaKhoaHoc;
            return View(ct);
        }
        [HttpPost, ActionName("XoaKhoaHoc")]
        public ActionResult XacNhanXoa(string id)
        {
            KHOAHOC ct = data.KHOAHOCs.SingleOrDefault(n => n.MaKhoaHoc == id);
            ViewBag.Makhoahoc = ct.MaKhoaHoc;
            if (ct == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHOAHOCs.DeleteOnSubmit(ct);
            data.SubmitChanges();
            return RedirectToAction("Index", "KhoaHoc");
        }
        public ActionResult SuaKhoaHoc(string id)
        {
            var sua = data.KHOAHOCs.First(m => m.MaKhoaHoc == id);
            return View(sua);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaKhoaHoc(string id, FormCollection c)
        {
            var sua = data.KHOAHOCs.First(m => m.MaKhoaHoc == id);
            var nambd = c["nambd"];
            var namkt = c["namkt"];
            sua.MaKhoaHoc = id;
            //if (string.IsNullOrWhiteSpace(nambd))
            //    ViewData["Loi1"] = "Năm bắt đầu không được để trống.";
            //else if (string.IsNullOrWhiteSpace(namkt))
            //    ViewData["Loi2"] = "Năm kết thúc không được để trống.";
            //else
            //{
                sua.NamBatDau = Convert.ToInt32(nambd);
                sua.NamKetThuc = Convert.ToInt32(namkt);
                UpdateModel(sua);
                data.SubmitChanges();
                return RedirectToAction("Index", "KhoaHoc");
            //}
            return this.SuaKhoaHoc(id);
        }
    }
}
