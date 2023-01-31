using prjMSIT145_Final.Models;
using System.ComponentModel;

namespace prjMSIT145_Final.ViewModel
{
    public class CProductOptionViewModel
    {
        ProductOption _options;
        ProductOptionGroup _optionsGroup;
        ViewOptionsToGroup _vmTOoptions;
        public CProductOptionViewModel()
        {
            _options = new ProductOption();
            _optionsGroup = new ProductOptionGroup();
            _vmTOoptions = new ViewOptionsToGroup();
        }
        public ViewOptionsToGroup vmTOoptions
        {
            get { return _vmTOoptions; }
            set { _vmTOoptions = value; }
        }
        public ProductOptionGroup optionsGroup
        {
            get { return _optionsGroup; }
            set { _optionsGroup = value; }
        }
        public ProductOption options
        {
            get { return _options; }
            set { _options = value; }
        }
        public int Fid
        {
            get { return _options.Fid; }
            set { _options.Fid = value; }
        }
        public int? OptionGroupFid
        {
            get { return _options.OptionGroupFid; }
            set { _options.OptionGroupFid = value; }
        }
        public int? BFid
        {
            get { return _options.BFid; }
            set { _options.BFid = value; }
        }

        [DisplayName("配料類別")]
        public string? OptionGroupName
        {
            get { return _optionsGroup.OptionGroupName; }
            set { _optionsGroup.OptionGroupName = value; }
        }
        [DisplayName("配料名稱")]

        public string? OptionName
        {
            get { return _options.OptionName; }
            set { _options.OptionName = value; }
        }
        [DisplayName("配料價格")]
        public double? UnitPrice
        {
            get { return Convert.ToDouble(_options.UnitPrice); }
            set { _options.UnitPrice = Convert.ToDecimal(value); }
        }
    }
}
