﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class GiangVienController : Controller
    {
        //Giảng viên: xóa, sửa, thêm: done
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("DangNhap", "GiangVien");
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            Session["admin"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var usr = f["tendn"];
            var pass = f["pass"];
            if (String.IsNullOrEmpty(usr))
                ViewData["Loi1"] = "Tên đăng nhập không được để trống.";
            else if (String.IsNullOrEmpty(pass))
                ViewData["Loi2"] = "Mật khẩu không được để trống.";
            else
            {
                GIAOVIEN ad = data.GIAOVIENs.SingleOrDefault(n => n.MaGV == usr && n.MatKhau == pass);
                if (ad != null)
                {
                    Session["admin"] = ad;
                    return RedirectToAction("Index", "GiangVien");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult GiaoVien()
        {
            var tc = from tt in data.GIAOVIENs select tt;
            return View(tc);
        }
        public ActionResult ChiTietGV(string id)
        {
            var ct = data.GIAOVIENs.First(m => m.MaGV == id);
            return View(ct);
        }
        public ActionResult ThemGV()
        {
            ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            return View();
        }
        [HttpPost]
        public ActionResult ThemGV(GIAOVIEN gv, FormCollection c)
        {
            ViewBag.MaMonHoc = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
            var ten = c["tenGV"];
            var magv = c["maGV"];
            var makh = c["MaKhoa"];
            var hv = c["hocVi"];
            if (string.IsNullOrEmpty(ten)|| string.IsNullOrEmpty(magv)|| string.IsNullOrEmpty(makh)|| string.IsNullOrEmpty(hv))
                ViewData["Loi1"] = "Thông tin giáo viên không được để trống";
            else if (data.GIAOVIENs.Where(t=>t.MaGV==magv).Count()!=0)
                ViewData["Loi1"] = "Mã giáo viên đã tồn tại";
            else
            {
                gv.MaGV = magv;
                gv.TenGV = ten;
                gv.ChuyenNganh = makh;
                gv.HocVi = hv;
                data.GIAOVIENs.InsertOnSubmit(gv);
                data.SubmitChanges();
                return RedirectToAction("GiaoVien", "GiangVien");
            }
            return this.ThemGV();
        }
        public ActionResult XoaGV(string id)
        {
            GIAOVIEN gv = data.GIAOVIENs.SingleOrDefault(n => n.MaGV == id);
            ViewBag.Magv = gv.MaGV;
            return View(gv);
        }
        [HttpPost, ActionName("XoaGV")]
        public ActionResult XacNhanXoa(string id)
        {
            GIAOVIEN gv = data.GIAOVIENs.SingleOrDefault(n => n.MaGV == id);
            ViewBag.Magv = gv.MaGV;
            if (gv == null)
            {
                return RedirectToAction("KhongTheXoa", "Home");
            }
            data.GIAOVIENs.DeleteOnSubmit(gv);
            data.SubmitChanges();
            return RedirectToAction("GiaoVien", "GiangVien");
        }
        public ActionResult SuaGV(string id)
        {
            ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            var sua = data.GIAOVIENs.First(m => m.MaGV == id);
            return View(sua);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaGV(string id, FormCollection c)
        {
            ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.TenKhoa), "MaKhoa", "TenKhoa");
            var sua = data.GIAOVIENs.First(m => m.MaGV == id);
            var ten = c["tenGV"];
            var makh = c["MaKhoa"];
            var hv = c["hocVi"];
            sua.MaGV = id;
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Thông tin giáo viên không được để trống.";
            else
            {
                sua.TenGV = ten;
                sua.HocVi = hv;
                sua.ChuyenNganh = makh;
                UpdateModel(sua);
                data.SubmitChanges();
                return RedirectToAction("GiaoVien", "GiangVien");
            }
            return this.SuaGV(id);
        }
    }
}
