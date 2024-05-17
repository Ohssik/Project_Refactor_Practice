using MapsterMapper;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.Interfaces;

namespace prjMSIT145Final.Service.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepo
            ,IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<dynamic>> Get(int id)
        {
            var orderDatas = await _orderRepo.Get(id);
            return orderDatas;
            //if (orderDatas != null)
            //{
            //    CANormalMemberOrderViewModel member = new CANormalMemberOrderViewModel();
            //    List<CANormalMemberOrderDetailViewModel> items = new List<CANormalMemberOrderDetailViewModel>();
            //    foreach (var vsf in orderDatas.Distinct())
            //    {
            //        CANormalMemberOrderDetailViewModel detail = new CANormalMemberOrderDetailViewModel();
            //        detail.productName = vsf.ProductName;
            //        detail.Options = vsf.Options + "/" + "$" + ((decimal)vsf.SubTotal).ToString("###,###") + "/" + vsf.Qty + "份";
            //        items.Add(detail);
            //    }
            //    member.details = items;


            //    member.BMemberName = orderDatas.Distinct().ToList()[0].BMemberName;
            //    member.BMemberPhone = orderDatas.Distinct().ToList()[0].BMemberPhone;
            //    member.OrderISerialId = orderDatas.Distinct().ToList()[0].OrderISerialId;
            //    member.PickUpDate = orderDatas.Distinct().ToList()[0].PickUpDate;
            //    member.PickUpTime = orderDatas.Distinct().ToList()[0].PickUpTime;
            //    member.TotalAmount = orderDatas.Distinct().ToList()[0].TotalAmount;
            //    member.PickUpType = orderDatas.Distinct().ToList()[0].PickUpType;
            //    member.PickUpPerson = orderDatas.Distinct().ToList()[0].PickUpPerson;
            //    member.PickUpPersonPhone = orderDatas.Distinct().ToList()[0].PickUpPersonPhone;
            //    member.Memo = orderDatas.Distinct().ToList()[0].Memo;
            //    member.businessImgFile = orderDatas.Distinct().ToList()[0].LogoImgFileName;
            //    member.businessAddress = orderDatas.Distinct().ToList()[0].Address;
            //    member.PayTermCatName = orderDatas.Distinct().ToList()[0].PaymentType;
            //    member.OrderState = orderDatas.Distinct().ToList()[0].OrderState;

            //    return member;
            //}
        }

        public async Task<IEnumerable<OrderDto>> GetByBusinessMemberId(int id)
        {
            var result = await _orderRepo.GetByBusinessMemberId(id);
            return _mapper.Map<IEnumerable<OrderDto>>(result);
        }

        public async Task<IEnumerable<OrderDto>> GetByNormalMemberId(int id)
        {
            var result = await _orderRepo.GetByNormalMemberId(id);
            return _mapper.Map<IEnumerable<OrderDto>>(result);
        }
    }
}
