using prjMSIT145Final.Parameters;
using prjMSIT145Final.Service.ParameterDtos;

namespace prjMSIT145Final.Helpers
{
    public interface IUploadImgHelper
    {
        Task<string> UploadAdImg(UploadImgParameter parameter);
    }
}
