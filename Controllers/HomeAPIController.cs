using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Muoi_Tam.Models;

namespace Muoi_Tam.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HomeAPIController : ControllerBase
	{
		QlgiaiBongDaContext db = new QlgiaiBongDaContext();
		//[HttpGet("{SanVanDongID}")]
		//// Hiển thị trận đấu theo sân vận động(menu là tên sân vận động) , menu là tên sân vận động, hiển thị trận đấu theo sân vận động
		//public ActionResult<IEnumerable<Trandau>> LayTranDauTheoCauThu(string SanVanDongID)
		//{
		//	var list_tranDau = from s in db.Trandaus
		//					   join svd in db.Sanvandongs
		//					   on s.SanVanDongId equals svd.SanVanDongId
		//					   where s.SanVanDongId == SanVanDongID
		//					   select s;
		//	var results = list_tranDau.ToList();
		//	return results;
		//}
		
	// Menu là 7 tên đội bóng , nếu mà ấn vào thì nó sẽ ra cầu thủ của đội bóng đó 
		[HttpGet("{caulacboID}")] 
		public ActionResult<IEnumerable<Cauthu>> LayCauThuTheoCauLacBo (string caulacboID)
		{
			var listCauThu = (from s in db.Cauthus
							  where  s.CauLacBoId == caulacboID
							  select s
							  ).ToList();
			return listCauThu;							  
		}
	 
	}
}

