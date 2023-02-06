using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;

namespace prjMSIT145_Final.Controllers
{
	public class ProblemController : Controller
	{
		private readonly ispanMsit145shibaContext _context;
		public ProblemController(ispanMsit145shibaContext context)
		{
			_context = context;
		}
		public IActionResult PAnswer(string keyword)
		{
			CProblemViewModel vm = new CProblemViewModel();
			var datas = (_context.ProblemQuestions.Join(_context.ProblemAnswers, q => q.AnswerFid, a => a.Fid, (q, a) => new
			{
				q.Question,
				a.Answer
			})).FirstOrDefault(k => keyword.Contains(k.Question));

			if (datas != null)
				vm.answer = datas.Answer;
			else
				vm.answer = "暫時無相符的答案，請您重新描述或以郵件聯繫客服。";

			return Json(vm);
		}

		public IActionResult PQuestion()
		{
			return View();
		}

	}
}
