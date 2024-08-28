using Microsoft.AspNetCore.Mvc;

namespace ST10150702_CLDV6212_POE.Controllers
{
    public class BlobController : Controller
    {
        private readonly BlobServices _blobService;

        public BlobController(BlobServices blobService)
        {
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlobs()
        {
            var blobs = await _blobService.ListBlobsAsync("media");
            return Json(blobs);
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(IFormFile blobFile)
        {
            if (blobFile != null)
            {
                using (var stream = blobFile.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync("media", blobFile.FileName, stream);
                }
            }
            return Ok();
        }
    }

}
