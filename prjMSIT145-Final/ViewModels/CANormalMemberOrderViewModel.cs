using prjMSIT145_Final.Models;
using System.ComponentModel;

namespace prjMSIT145_Final.ViewModels
{
    public class CANormalMemberOrderViewModel
    {
        
        public List<CANormalMemberOrderDetailViewModel> details;
        
        public CANormalMemberOrderViewModel()
        {
            details=new List<CANormalMemberOrderDetailViewModel>();
            
        }
        
        public string? businessImgFile
        {
            get
            {
                ispanMsit145shibaContext db = new ispanMsit145shibaContext();
                BusinessImg bi = db.BusinessImgs.FirstOrDefault(i => i.BFid == BFid);

                string imgFileName = "";
                if (bi != null)
                    imgFileName = string.IsNullOrEmpty(bi.LogoImgFileName) ? "" : bi.LogoImgFileName.ToString();

                return imgFileName;
            }
        }
        
        public string? businessAddress
        {
            get
            {
                ispanMsit145shibaContext db = new ispanMsit145shibaContext();
                BusinessMember b = db.BusinessMembers.FirstOrDefault(b => b.Fid == BFid);

                string address = "";
                if (b != null)
                    address = string.IsNullOrEmpty(b.Address) ? "" : b.Address.ToString();

                return address;
            }
        }

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
                string result = "";

                if (PickUpDate != null)
                    result += PickUpDate.ToString() +" ";
                if (PickUpTime != null)
                    result += PickUpTime.ToString();

                return result;
            }
        
        }
        [DisplayName("取餐方式")]
        public string? PickUpType { get; set; }
        [DisplayName("取餐人")]
        public string? PickUpPerson { get; set; }
        [DisplayName("取餐人電話")]
        public string? PickUpPersonPhone { get; set; }
        public int? PayTermCatId { get; set; }
        [DisplayName("付款方式")]
        public string? PayTermCatName { 
            get
            {
                string payTerm = "";
                
                if (PayTermCatId != null)
                {
                    ispanMsit145shibaContext db = new ispanMsit145shibaContext();
                    var p = db.PaymentTermCategories.FirstOrDefault(p => p.Fid == (int)PayTermCatId);
                    payTerm = p.PaymentType;
                }
                return payTerm;
            }
        }
        
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
