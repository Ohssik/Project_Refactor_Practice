using Microsoft.Build.Framework;
using prjMSIT145_Final.Models;


namespace prjMSIT145_Final.ViewModels
{
    public class CNormalMemberViewModel
    {
        private NormalMember _member;
        public CNormalMemberViewModel()
        {
            _member= new NormalMember();
        }
        public NormalMember member
        {
            get { return _member; }
            set { _member= value; }
        }
        public int Fid { 
            get {return _member.Fid;} 
            set {_member.Fid=value; }  
        }
      
        public string? MemberName
        {
            get { return _member.MemberName; }
            set { _member.MemberName = value; }
        }
        public string? Phone
        {
            get { return _member.Phone; }
            set { _member.Phone = value; }
        }
      
        public string? Password
        {
            get { return _member.Password; }
            set { _member.Password = value; }
        }
        
        [Required]
        public string? Gender
        {
            get { return _member.Gender; }
            set { _member.Gender = value; }
        }
        [Required]
        public string? AddressCity
        {
            get { return _member.AddressCity; }
            set { _member.AddressCity = value; }
        }
        public string? AddressArea
        {
            get { return _member.AddressArea; }
            set { _member.AddressArea = value; }
        }
        public DateTime? Birthday
        {
            get { return _member.Birthday; }
            set { _member.Birthday = value; }
        }
        public string? Email
        {
            get { return _member.Email; }
            set { _member.Email = value; }
        }
        public DateTime? RegisterTime
        {
            get { return _member.RegisterTime; }
            set { _member.RegisterTime = value; }
        }
        public string? MemberPhotoFile
        {
            get { return _member.MemberPhotoFile; }
            set { _member.MemberPhotoFile = value; }
        }
        public int? IsSuspensed
        {
            get { return _member.IsSuspensed; }
            set { _member.IsSuspensed = value; }
        }
        public int? EmailCertified
        {
            get { return _member.EmailCertified; }
            set { _member.EmailCertified = value; }
        }

        public string? OldPassword { get; set; }

        public string? Passwordcheck { get; set; }

        public IFormFile photo { get; set; }













    }
}
