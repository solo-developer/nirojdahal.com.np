using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [Route("attachment")]
    public class AttachmentController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public AttachmentController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [Route("resume")]
        [ResponseCache(Duration =600)]
        public IActionResult Index()
        {
            var baseDirectory = _environment.WebRootPath;
            return new PhysicalFileResult($@"{baseDirectory}/attachments/Niroj_Dahal_Resume.pdf", "application/pdf");
           // return View();
        }
    }
}
