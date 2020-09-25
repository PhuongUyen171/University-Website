using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class ChuongTrinhController : Controller
    {
        //Chương trình: thêm, xóa, sửa, chi tiết: done(chưa kiểm tra khóa ngoại)
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var tc = from tt in data.CHUONGTRINHs select tt;
            //if (string.IsNullOrEmpty(tim))
            //    tc = tc.Where(m => m.TenCT.Contains(tim));
            //List<CHUONGTRINH> ds = new List<CHUONGTRINH>();
            //foreach (var item in tc)
            //{
            //    CHUONGTRINH t = new CHUONGTRINH();
            //    t.TenCT = item.TenCT;
            //    t.MaCT = item.MaCT;
            //    ds.Add(t);
            //}
            
            return View(tc);
        }
        public ActionResult ChiTietChuongTrinh(string id)
        {
            var ct = data.CHUONGTRINHs.First(m => m.MaCT == id);
            return View(ct);
        }
        public ActionResult ThemChuongTrinh()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemChuongTrinh(CHUONGTRINH ct, FormCollection c)
        {
            var ten = c["tenCT"];
            var ma = c["maCT"];
            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(ma))
                ViewData["Loi1"] = "Thông tin không được để trống.";
            else if (data.CHUONGTRINHs.Where(t=>t.MaCT==ma).Count()!=0)
                ViewData["Loi1"] = "Mã chương trình đã tồn tại.";
            else
            {
                ct.MaCT = ma;
                ct.TenCT = ten;
                data.CHUONGTRINHs.InsertOnSubmit(ct);
                data.SubmitChanges();
                return RedirectToAction("Index", "ChuongTrinh");
            }
            return this.ThemChuongTrinh();
        }
        public ActionResult XoaChuongTrinh(string id)
        {
            CHUONGTRINH ct = data.CHUONGTRINHs.SingleOrDefault(n => n.MaCT == id);
            ViewBag.Mact = ct.MaCT;
            return View(ct);
        }
        [HttpPost, ActionName("XoaChuongTrinh")]
        public ActionResult XacNhanXoa(string id)
        {
            try
            {
                CHUONGTRINH ct = data.CHUONGTRINHs.SingleOrDefault(n => n.MaCT == id);
                ViewBag.Mact = ct.MaCT;
                if (ct == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.CHUONGTRINHs.DeleteOnSubmit(ct);
                data.SubmitChanges();
                return RedirectToAction("Index", "ChuongTrinh");
            }
            catch (Exception)
            {
                Response.StatusCode=404;
                return null;
            }
            
        }
        public ActionResult SuaChuongTrinh(string id)
        {
            //ViewBag.MaMonHoc = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
            var sua = data.CHUONGTRINHs.First(m => m.MaCT == id);
            return View(sua);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaChuongTrinh(string id, FormCollection c)
        {
            //ViewBag.MaMonHoc = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
            var sua = data.CHUONGTRINHs.First(m => m.MaCT == id);
            var ten = c["tenCT"];
            sua.MaCT = id;
            if (string.IsNullOrEmpty(ten))
                ViewData["Loi1"] = "Tên chương trình không được để trống.";
            else
            {
                sua.TenCT= ten;
                UpdateModel(sua);
                data.SubmitChanges();
                return RedirectToAction("Index", "ChuongTrinh");
            }
            return this.SuaChuongTrinh(id);
        }
    }
}
