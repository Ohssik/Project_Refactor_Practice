﻿using prjMSIT145_Final.Models;
using System.ComponentModel;

namespace prjMSIT145_Final.ViewModels
{
    public class CACouponViewModel
    {
        private Coupon _coupon;
        public CACouponViewModel()
        {
            _coupon = new Coupon();
        }
        public Coupon coupon
        {
            get { return _coupon; }
            set { _coupon = value; }
        }
        [DisplayName("ID")]
        public int Fid 
        {
            get { return _coupon.Fid; }
            set { _coupon.Fid = value; } 
        }
        [DisplayName("優惠券代碼")]
        public string? CouponCode
        {
            get { return _coupon.CouponCode; }
            set { _coupon.CouponCode = value; }
        }
        [DisplayName("扣除金額")]
        public decimal? Price
        {
            get { return _coupon.Price; }
            set { _coupon.Price = value; }
        }
        [DisplayName("是否已使用")]
        public int? IsUsed
        {
            get { return _coupon.IsUsed; }
            set { _coupon.IsUsed = value; }
        }
        [DisplayName("備註")]
        public string? Memo
        {
            get { return _coupon.Memo; }
            set { _coupon.Memo = value; }
        }
    }
}
