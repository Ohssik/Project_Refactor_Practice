using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.ViewModel
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
