using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class MonHocController : Controller
    {
        //Môn học: thêm, xóa, sửa, chi tiết: done
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var tc = from tt in data.MONHOCs select tt;
            return View(tc);
        }
        public ActionResult ChiTietMonHoc(string id)
        {
            MONHOC ct = data.MONHOCs.First(m => m.MaMH == id);
            return View(ct);
        }
        public ActionResult ThemMonHoc()
        {
            ViewBag.Makhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            return View();
        }
        [HttpPost]
        public ActionResult ThemMonHoc(MONHOC k, FormCollection c)
        {
            ViewBag.Makhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            var ten = c["tenMH"];
            var ma = c["maMH"];
            var makhoa = c["maKhoa"];
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên môn học không được để trống";
            else if (string.IsNullOrEmpty(ma))
                ViewData["Loi2"] = "Mã môn học không được để trống";
            else
            {
                k.MaMH = ma;
                k.TenMH = ten;
                k.MaKhoa = makhoa;
                data.MONHOCs.InsertOnSubmit(k);
                data.SubmitChanges();
                return RedirectToAction("Index", "MonHoc");
            }
            return this.ThemMonHoc();
        }
        public ActionResult XoaMonHoc(string id)
        {
            MONHOC mh = data.MONHOCs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.Mamh = mh.MaMH;
            return View(mh);
        }
        [HttpPost, ActionName("XoaMonHoc")]
        public ActionResult XacNhanXoa(string id)
        {
            MONHOC mh = data.MONHOCs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.Mamh = mh.MaMH;
            if (mh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.MONHOCs.DeleteOnSubmit(mh);
            data.SubmitChanges();
            return RedirectToAction("Index", "MonHoc");
        }
        public ActionResult SuaMonHoc(string id)
        {
            ViewBag.Makhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            var sua = data.MONHOCs.First(m => m.MaMH == id);
            return View(sua);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaMonHoc(string id, FormCollection c)
        {
            ViewBag.Makhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            var sua = data.MONHOCs.First(m => m.MaMH == id);
            var ten = c["TenMH"];
            var makhoa = c["maKhoa"];
            sua.MaMH = id;
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên môn học không được để trống.";
            else
            {
                sua.TenMH = ten;
                sua.MaKhoa = makhoa;
                UpdateModel(sua);
                data.SubmitChanges();
                return RedirectToAction("Index", "MonHoc");
            }
            return this.SuaMonHoc(id);
        }
    }
}
