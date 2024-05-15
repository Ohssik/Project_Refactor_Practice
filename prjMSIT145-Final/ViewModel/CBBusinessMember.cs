using prjMSIT145Final.Infrastructure.Models;

namespace prjMSIT145Final.Web.ViewModel
{
    public class CBBusinessMemberViewModel
    {
        private BusinessMember member;
        private BusinessImg Img;
        public CBBusinessMemberViewModel()
        {
            Img = new BusinessImg();
            member = new BusinessMember();
        }
        public BusinessMember businessMember
        {
            get { return member; }
            set { member = value; }
        }
        public BusinessMember _businessMember { get { return member; } set { member = value; } }
        public BusinessImg _businessImg { get { return Img; } set { Img = value; } }
        public int Fid { get { return member.Fid; } set { member.Fid = value; } }
        public string? MemberName { get { return member.MemberName; } set { member.MemberName = value; } }
        public string? Brand { get { return member.Brand; } set { member.Brand = value; } }
        public string? Phone { get { return member.Phone; } set { member.Phone = value; } }
        public string? Password { get { return member.Password; } set { member.Password = value; } }
        public TimeSpan? OpenTime { get { return member.OpenTime; } set { member.OpenTime = value; } }
        public TimeSpan? CloseTime { get { return member.CloseTime; } set { member.CloseTime = value; } }
        public string? Address { get { return member.Address; } set { member.Address = value; } }
        public string? ShopType { get { return member.ShopType; } set { member.ShopType = value; } }
        public string? Email { get { return member.Email; } set { member.Email = value; } }
        public string? ContactPerson { get { return member.ContactPerson; } set { member.ContactPerson = value; } }
        public string? Gps { get { return member.Gps; } set { member.Gps = value; } }
        public int ImgFid { get { return Img.Fid; } set { Img.Fid = value; } }
        public int? BFid { get { return Img.BFid; } set { Img.BFid = value; } }
        public string? LogoImgFileName { get { return Img.LogoImgFileName; } set { Img.LogoImgFileName = value; } }
        public string? SighImgFileName { get { return Img.SighImgFileName; } set { Img.SighImgFileName = value; } }
        public string? BannerImgFileName1 { get { return Img.BannerImgFileName1; } set { Img.BannerImgFileName1 = value; } }
        public string? BannerImgFileName2 { get { return Img.BannerImgFileName2; } set { Img.BannerImgFileName2 = value; } }
        public string? BannerImgFileName3 { get { return Img.BannerImgFileName3; } set { Img.BannerImgFileName3 = value; } }

        public IFormFile? LogoImgFile { get; set; }
        public IFormFile? SighImgFile { get; set; }
        public IFormFile BannerImg1File { get; set; }
        public IFormFile BannerImg2File { get; set; }
        public IFormFile BannerImg3File { get; set; }


    }
}
