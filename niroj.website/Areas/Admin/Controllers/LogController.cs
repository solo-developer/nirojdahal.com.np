using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using niroj.website.Areas.Admin.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace niroj.website.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("admin/log")]
    public class LogController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public LogController(IHostingEnvironment host)
        {
            _hostingEnvironment= host;
        }

        [Route("error")]
        public IActionResult Index()
        {
            string[] filePaths = Directory.GetFiles(Path.Combine(this._hostingEnvironment.ContentRootPath, "Logs/"));

            List<FileDetailModel> files = new List<FileDetailModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new FileDetailModel { Name = Path.GetFileName(filePath) ,CreatedDate= (new FileInfo(filePath).CreationTime)});
            }
            return View(files.OrderByDescending(a=>a.CreatedDate).ToList());
        }


        [Route("error/{fileName}/download")]
        public IActionResult GetFileDetail(string fileName)
        {
            string path = Path.Combine(this._hostingEnvironment.ContentRootPath, "Logs/") + fileName;
            return PhysicalFile(path, MimeTypes.GetMimeType(path), Path.GetFileName(path));
            //Read the File data into Byte Array.
            //byte[] bytes = System.IO.File.ReadAllBytes(path);

            ////Send the File to Download.
            //return File(bytes, "application/octet-stream", fileName);
        }
    }
}
