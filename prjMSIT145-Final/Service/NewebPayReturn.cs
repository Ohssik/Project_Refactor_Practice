using System;

namespace prjMSIT145_Final.Service
{
	public class NewebPayReturn<T>
	{
		public string Status { get; set; }
		public string Message { get; set; }
		public T Result { get; set; }
	}

	public class NewebPayQueryResult
	{
		public string MerchantID { get; set; }
		public string MerchantOrderNo { get; set; }
		public string TradeNo { get; set; }
		public int Amt { get; set; }
		public string TradeStatus { get; set; }
		public string PaymentType { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime PayTime { get; set; }
		public string CheckCode { get; set; }
		public DateTime FundTime { get; set; }
		public string ShopMerchantID { get; set; }

	}
}
