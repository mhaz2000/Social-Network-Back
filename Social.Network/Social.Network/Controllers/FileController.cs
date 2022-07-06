using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Social.Network.SeedWorks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Social.Network.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostFile()
        {
            var files = HttpContext.Request.Body;
            HttpContext.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;

            Guid id = Guid.NewGuid();
            var path = @$"{Directory.GetCurrentDirectory()}\FileStorage\{id}.png";
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                files.CopyTo(fileStream);
            }

            return OkResult("Image is uploaded successfully.", id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            var filename = $"{id}.png";

            if (!System.IO.File.Exists(@$"{Directory.GetCurrentDirectory()}\FileStorage\{filename}"))
                throw new Exception("File cannot be found!");

            var stream = new FileStream(@$"{Directory.GetCurrentDirectory()}\FileStorage\{filename}", FileMode.Open, FileAccess.Read, FileShare.Read);

            var mimeType = GetMimeType(filename);
            var cdStr = $"inline; filename=\"{filename}\"";


            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Disposition");
            Response.Headers.Add("Content-Disposition", cdStr);
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(stream, mimeType);
        }

        private static string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
