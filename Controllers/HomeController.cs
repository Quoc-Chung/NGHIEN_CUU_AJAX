using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Muoi_Tam.Models;
using System.Diagnostics;

namespace Muoi_Tam.Controllers
{
    public class HomeController : Controller
    {
        public QlgiaiBongDaContext db = new QlgiaiBongDaContext();
        public IActionResult Index()
        {
            ViewBag.TenSanVanDong = db.Sanvandongs.Take(6).ToList();
			ViewBag.TenCauLacBo  = db.Caulacbos.Take(6).ToList();
            var list_TranDau = db.Trandaus.ToList();
            return View(list_TranDau);
        }

        public IActionResult Privacy()
        {
            return View();
        }

		[Route("ThemTranDauMoi")]
		[HttpGet]
		public IActionResult ThemTranDauMoi()
		{
			ViewBag.TenCauLacBo = db.Caulacbos.Take(6).ToList();
			ViewBag.TenSanVanDong = db.Sanvandongs.Take(6).ToList();
			// Lấy danh sách CLB từ bảng CAULACBO cho CLB khách và CLB nhà
			ViewBag.Clbkhach = new SelectList(db.Caulacbos.ToList(), "CauLacBoId", "TenClb");
			ViewBag.Clbnha = new SelectList(db.Caulacbos.ToList(), "CauLacBoId", "TenClb");

			// Lấy danh sách sân vận động từ bảng SANVANDONG
			ViewBag.SanVanDongId = new SelectList(db.Sanvandongs.ToList(), "SanVanDongId", "TenSan");
			ViewBag.TenCauThu = db.Cauthus.Take(7).ToList();
			return View();
		}

		[Route("ThemTranDauMoi")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ThemTranDauMoi(Trandau TranDau)
		{
			ViewBag.TenCauLacBo = db.Caulacbos.Take(6).ToList();
			ViewBag.TenSanVanDong = db.Sanvandongs.Take(6).ToList();
			/*- == true thì chứng tỏ dứ liệu truyền từ form vào hợp lệ -*/
			if (ModelState.IsValid)
			{
				db.Add(TranDau);
				db.SaveChanges();
				return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
			}
			ViewBag.TenCauThu = db.Cauthus.Take(7).ToList();
			return View();
		}

		[Route("XoaTranDau")]
		[HttpGet]
		public IActionResult XoaTranDau(string trandauID)
		{
			ViewBag.TenCauLacBo = db.Caulacbos.Take(6).ToList();
			ViewBag.TenSanVanDong = db.Sanvandongs.Take(6).ToList();
			TempData["Message"] = "";
			var list_TranDau = db.Trandaus.ToList();
			ViewBag.TenCauThu = db.Cauthus.Take(7).ToList();
			// Kiểm tra chi tiết sản phẩm trước khi xóa
			var trandaughiban = db.TrandauGhibans.Any(x => x.TranDauId == trandauID);


			if (trandaughiban)
			{
				TempData["Message"] = "Không xóa được sản phẩm này vì có chi tiết sản phẩm liên quan.";
				return RedirectToAction("Index", "Home");
			}

			// Xóa các ảnh liên quan đến sản phẩm
			var trongtai_trandau = db.TrongtaiTrandaus.Where(x => x.TranDauId == trandauID);
			if (trongtai_trandau.Any())
			{
				db.RemoveRange(trongtai_trandau);
			}

			var cauthu_trandau = db.TrandauCauthus.Where(x => x.TranDauId == trandauID);
			if (cauthu_trandau.Any())
			{
				db.RemoveRange(cauthu_trandau);
			}

			// Xóa sản phẩm trong bảng TDanhMucSps
			var TranDau = db.Trandaus.Find(trandauID);
			if (TranDau != null)
			{
				db.Remove(TranDau);
				db.SaveChanges();
				TempData["Message"] = "Sản phẩm đã được xóa.";
			}
			else
			{
				TempData["Message"] = "Sản phẩm không tồn tại.";
			}
			return RedirectToAction("Index", "Home");
		}
	}
}
