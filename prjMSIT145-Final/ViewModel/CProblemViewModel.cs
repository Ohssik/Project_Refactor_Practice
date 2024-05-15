using prjMSIT145Final.Infrastructure.Models;

namespace prjMSIT145Final.Web.ViewModel
{
	public class CProblemViewModel
	{
		ProblemAnswer _answer;
		public CProblemViewModel()
		{
			_answer = new ProblemAnswer();
		}
		public string answer
		{
			get { return _answer.Answer; }
			set { _answer.Answer = value; }
		}
	}
}
