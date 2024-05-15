using prjMSIT145Final.Infrastructure.Models;
using System.ComponentModel;

namespace prjMSIT145Final.Web.ViewModels
{
    public class CANormalMemberOrderViewModel
    {
        
        public List<CANormalMemberOrderDetailViewModel> details;
        
        public CANormalMemberOrderViewModel()
        {
            details=new List<CANormalMemberOrderDetailViewModel>();
            
        }
        public string? businessImgFile { get; set; }
        public string? normalImgFile { get; set; }
        public string? businessAddress { get; set; }
        
        public int OrderFid { get; set; }                
        public int? BFid { get; set; }
        [DisplayName("商家名稱")]
        public string? BMemberName { get; set; }
        [DisplayName("商家電話")]
        public string? BMemberPhone { get; set; }
        
        public DateTime? PickUpDate { get; set; }
        public TimeSpan? PickUpTime { get; set; }
        [DisplayName("取餐時間")]
        public string? _pickUpTime 
        {
            get
            {
                string now = DateTime.Now.ToString("yyyyMMdd");
                string result = "";

                if (PickUpDate != null)
                    result += Convert.ToDateTime(PickUpDate).ToString("yyyy/MM/dd") +" ";
                if (PickUpTime != null)
                    result += ((TimeSpan)PickUpTime).Hours.ToString()+":"+((TimeSpan)PickUpTime).Minutes.ToString();

                return result;
            }
        
        }
        [DisplayName("取餐方式")]
        public string? PickUpType { get; set; }
        [DisplayName("取餐人")]
        public string? PickUpPerson { get; set; }
        [DisplayName("取餐人電話")]
        public string? PickUpPersonPhone { get; set; }
        
        [DisplayName("付款方式")]
        public string? PayTermCatName{get; set;}
        
        public string? OrderState { get; set; }
        [DisplayName("訂單狀態")]
        public string? _orderState { 
            get
            {
                string state = "";
                if (OrderState != null)
                {
                    switch (OrderState)
                    {
                        case "0":
                            state = "購物車中";
                            break;
                        case "1":
                            state = "未接單";
                            break;
                        case "2":
                            state = "已接單";
                            break;
                        case "3":
                            state = "商家準備中";
                            break;
                        case "4":
                            state = "訂單已完成";
                            break;
                        case "5":
                            state = "商家退單";
                            break;
                        case "6":
                            state = "揪團失敗";
                            break;
                    }
                }
                return state;
            }
        }
        [DisplayName("備註")]
        public string? Memo { get; set; }
        public DateTime? OrderTime { get; set; }
        [DisplayName("付款金額")]
        public decimal? TotalAmount { get; set; }
        [DisplayName("訂單編號")]
        public string? OrderISerialId { get; set; }
        public int? ItemFid { get; set; }
        
    }
}
