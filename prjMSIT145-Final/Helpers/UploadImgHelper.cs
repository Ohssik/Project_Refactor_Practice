using prjMSIT145Final.Parameters;

namespace prjMSIT145Final.Helpers
{
    public class UploadImgHelper : IUploadImgHelper
    {
        private readonly IWebHostEnvironment _host;

        public UploadImgHelper(IWebHostEnvironment host) 
        {
            _host = host;
        }
        public async Task<string> UploadAdImg(UploadImgParameter parameter)
        {
            byte[] imageBytes = parameter.Icon;
            string[] fileTypeArr = parameter.FileType.Split('/');
            string fileType = "." + fileTypeArr[1];

            string fName = Guid.NewGuid().ToString() + fileType;
            MemoryStream buf = new MemoryStream(imageBytes);
            string filePath = Path.Combine(_host.WebRootPath, "adminImg/adDisplay", fName);

            await Task.Run(() =>
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    buf.WriteTo(fileStream);
                }

                buf.Close();
                buf.Dispose();
            });

            return fName;
        }
    }
}
