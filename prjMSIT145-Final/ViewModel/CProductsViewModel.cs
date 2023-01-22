using prjMSIT145_Final.Models;
using System.ComponentModel;

namespace prjMSIT145_Final.ViewModel
{
    public class CProductsViewModel
    {
        Product _product;
        ProductCategory _proCategory;
        public CProductsViewModel()
        {
            _product = new Product();
            _proCategory = new ProductCategory();
        }
        public ProductCategory proCategory
        {
            get { return _proCategory; }
            set { _proCategory = value; }
        }
        public string? CategoryName
        {
            get { return _proCategory.CategoryName; }
            set { _proCategory.CategoryName = value; }
        }

        public Product product
        {
            get { return _product; }
            set { _product = value; }
        }
        public int Fid
        {
            get { return _product.Fid; }
            set { _product.Fid = value; }
        }
        public int BFid
        {
            get { return _product.BFid; }
            set { _product.BFid = value; }
        }
        [DisplayName("產品名稱")]
        public string ProductName
        {
            get { return _product.ProductName; }
            set { _product.ProductName = value; }
        }
        [DisplayName("產品價格")]

        public decimal? UnitPrice
        {
            get { return _product.UnitPrice; }
            set { _product.UnitPrice = value; }
        }
        [DisplayName("產品備註")]
        public string? Memo
        {
            get { return _product.Memo; }
            set { _product.Memo = value; }
        }

        public int? IsForSale
        {
            get { return _product.IsForSale; }
            set { _product.IsForSale = value; }
        }
        [DisplayName("產品圖片")]

        public string? Photo
        {
            get { return _product.Photo; }
            set { _product.Photo = value; }
        }
        public IFormFile file { get; set; }

        public int? CategoryFid
        {
            get { return _product.CategoryFid; }
            set { _product.CategoryFid = value; }
        }
    }
}
