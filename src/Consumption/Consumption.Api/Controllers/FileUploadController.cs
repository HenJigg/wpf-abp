using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Consumption.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> logger;
        private readonly IWebHostEnvironment hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="hostingEnvironment"></param>
        public FileUploadController(ILogger<FileUploadController> logger,
            IWebHostEnvironment hostingEnvironment)
        {
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            try
            {
                List<IFormFile> formFiles = new List<IFormFile>();
                if (Request != null &&
                    Request.Form != null &&
                    Request.Form.Files != null &&
                    Request.Form.Files.Count > 0)
                    formFiles.AddRange(Request.Form.Files);

                long size = formFiles.Sum(f => f.Length);

                DateTime dateTime = DateTime.Now;
                string basePath = $"\\Files\\{dateTime.ToString("yyyyMMdd")}\\";
                string filePath = hostingEnvironment.WebRootPath + basePath;
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                List<string> filePathResultList = new List<string>();
                foreach (var formFile in formFiles)
                {
                    if (formFile.Length > 0)
                    {
                        string fileExt = System.IO.Path.GetExtension(formFile.FileName);
                        string fileName = System.Guid.NewGuid().ToString() + fileExt;
                        string fileFullName = filePath + fileName;
                        using (var stream = new FileStream(fileFullName, FileMode.Create))
                            await formFile.CopyToAsync(stream);

                        filePathResultList.Add($"/Files/{dateTime.ToString("yyyyMMdd")}/{fileName}");
                    }
                }
                string message = $"{formFiles.Count} 个文件上传成功!";
                return Ok(new
                {
                    Success = true,
                    Message = message,
                    FilePathList = filePathResultList
                });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
