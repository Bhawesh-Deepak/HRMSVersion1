using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Helpers.BlobHelper
{
    //code to upload the Image on to specific folder and return image path
    public class BlobHelper
    {
        public async Task<string> UploadImageToFolder(IFormFile uploadImage, IHostingEnvironment _hostingEnvironment)
        {
            string imagePath = string.Empty;
            if (uploadImage != null && uploadImage.Length > 0)
            {
                var upload = Path.Combine(_hostingEnvironment.WebRootPath, "Images//");
                using (FileStream fs = new FileStream(Path.Combine(upload, uploadImage.FileName), FileMode.Create))
                {
                    await uploadImage.CopyToAsync(fs);
                }
                imagePath = "/Images/" + uploadImage.FileName;
            }

            return imagePath;
        }
    }
}
