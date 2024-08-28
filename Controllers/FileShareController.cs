using Microsoft.AspNetCore.Mvc;

namespace ST10150702_CLDV6212_POE.Controllers
{
    public class FileShareController : Controller
    {
        private readonly FileService _fileService;

        public FileShareController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _fileService.ListFilesAsync("contracts");
            return Json(files);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile fileShare)
        {
            if (fileShare != null)
            {
                using (var stream = fileShare.OpenReadStream())
                {
                    await _fileService.UploadFileAsync("contracts", fileShare.FileName, stream);
                }
            }
            return Ok();
        }
    }

}
