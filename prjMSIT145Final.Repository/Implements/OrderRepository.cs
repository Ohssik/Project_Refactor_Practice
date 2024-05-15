using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ispanMsit145shibaContext _context;

        public OrderRepository(
            ispanMsit145shibaContext context
        )
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> Get(int id)
        {
            var result = await Task.Run(() =>
            {
                var orderDatas =
                from order in _context.Orders
                join f in _context.ViewShowFullOrders on order.Fid equals f.OrderFid
                into group7
                from g7 in group7.DefaultIfEmpty()
                join bm in _context.BusinessImgs on g7.BFid equals bm.Fid
                into group2
                from g2 in group2.DefaultIfEmpty()
                join b in _context.BusinessMembers on order.BFid equals b.Fid
                into group3
                from g3 in group3.DefaultIfEmpty()
                join pay in _context.PaymentTermCategories on order.PayTermCatId equals pay.Fid
                into group4
                from g4 in group4.DefaultIfEmpty()
                where order.Fid == (int)id
                select new
                {
                    g7.BMemberPhone,
                    g7.BMemberName,
                    order.OrderISerialId,
                    order.PickUpDate,
                    order.PickUpPerson,
                    order.PickUpPersonPhone,
                    order.PickUpTime,
                    order.PickUpType,
                    order.PayTermCatId,
                    order.Memo,
                    order.TotalAmount,
                    g7.ProductName,
                    order.OrderState,
                    g7.Options,
                    g7.Qty,
                    g7.SubTotal,
                    g2.LogoImgFileName,
                    g3.Address,
                    g4.PaymentType
                };

                return orderDatas;
            });

            return result;
        }

        public async Task<IEnumerable<Order>> GetByBusinessMemberId(int id)
        {
            var result = await Task.Run(() => {
                return _context.Orders.Where(o => o.BFid.Equals(id));
            });

            return result ?? Enumerable.Empty<Order>();
        }

        public async Task<IEnumerable<Order>> GetByNormalMemberId(int id)
        {
            var result = await Task.Run(() => _context.Orders.Where(order => order.NFid.Equals(id)));
            return result ?? Enumerable.Empty<Order>();
        }
    }
}
