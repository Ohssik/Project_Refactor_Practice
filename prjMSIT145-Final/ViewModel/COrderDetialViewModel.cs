using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;

namespace prjMSIT145_Final.ViewModels
{
    public class COrderDetialViewModel
    {
        private ViewShowFullOrder _order;
        public COrderDetialViewModel()
        {
            _order = new ViewShowFullOrder();
        }
        public ViewShowFullOrder Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public string? Address
        {
            get;
            set;
        }


        public int OrderFid
        {
            get { return _order.OrderFid; }
            set { _order.OrderFid = value; }
        }
        public int? Fid
        {
            get { return _order.Fid; }
            set { _order.Fid = value; }
        }
        public int? NFid
        {
            get { return _order.NFid; }
            set { _order.NFid = value; }
        }
        public int? BFid
        {
            get { return _order.BFid; }
            set { _order.BFid = value; }
        }
        public string? BMemberName
        {
            get { return _order.BMemberName; }
            set { _order.BMemberName = value; }
        }
        public string? BMemberPhone
        {
            get { return _order.BMemberPhone; }
            set { _order.BMemberPhone = value; }
        }
        public DateTime? PickUpDate
        {
            get { return _order.PickUpDate; }
            set { _order.PickUpDate = value; }
        }
        public TimeSpan? PickUpTime
        {
            get { return _order.PickUpTime; }
            set { _order.PickUpTime = value; }
        }
        public string? PickUpType
        {
            get { return _order.PickUpType; }
            set { _order.PickUpType = value; }
        }
        public string? PickUpPerson
        {
            get { return _order.PickUpPerson; }
            set { _order.PickUpPerson = value; }
        }
        public string? PickUpPersonPhone
        {
            get { return _order.PickUpPersonPhone; }
            set { _order.PickUpPersonPhone = value; }
        }
        public string? PayTermCatId
        {
            get ; 
            set ; 
        }
        public string? OrderState
        {
            get { return _order.OrderState; }
            set { _order.OrderState = value; }
        }
        public string? Memo
        {
            get { return _order.Memo; }
            set { _order.Memo = value; }
        }
        public DateTime? OrderTime
        {
            get { return _order.OrderTime; }
            set { _order.OrderTime = value; }
        }
        public decimal? TotalAmount
        {
            get { return _order.TotalAmount; }
            set { _order.TotalAmount = value; }
        }
        public string? OrderISerialId
        {
            get { return _order.OrderISerialId; }
            set { _order.OrderISerialId = value; }
        }
        public int? ItemFid
        {
            get { return _order.ItemFid; }
            set { _order.ItemFid = value; }
        }
        public int? ProductFId
        {
            get { return _order.ProductFId; }
            set { _order.ProductFId = value; }
        }
        public string ProductName
        {
            get { return _order.ProductName; }
            set { _order.ProductName = value; }
        }
        public decimal? UnitPrice
        {
            get { return _order.UnitPrice; }
            set { _order.UnitPrice = value; }
        }
        public int? Qty
        {
            get { return _order.Qty; }
            set { _order.Qty = value; }
        }
        public string? Options
        {
            get { return _order.Options; }
            set { _order.Options = value; }
        }
        public decimal? SubTotal
        {
            get { return _order.SubTotal; }
            set { _order.SubTotal = value; }
        }
        public int? totalQty { get; set; }

        public List<COrderItemViewModel> items { get; set; }
    }
}
