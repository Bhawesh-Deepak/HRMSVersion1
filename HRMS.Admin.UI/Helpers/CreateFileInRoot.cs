using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Helpers
{
    public static class CreateFileInRoot
    {
        public static void CreateFileIfNotExistsOrDelete(string fileName, IHostingEnvironment _IHostingEnviroment, string url)
        {

            var sWebRootFolder = _IHostingEnviroment.WebRootPath;
            var sFileName = $"{fileName}.xlsx";
            var URL =url;
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(fileName: Path.Combine(sWebRootFolder, sFileName));
            }
        }
    }
}
