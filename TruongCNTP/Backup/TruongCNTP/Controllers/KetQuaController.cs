using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruongCNTP.Models;

namespace TruongCNTP.Controllers
{
    public class KetQuaController : Controller
    {
        //Kết quả

        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var tc = from tt in data.KETQUAs select tt;
            return View(tc);
        }
        public ActionResult ChiTietKetQua(string id)
        {
            return View();
        }


    }
}
