using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ST10150702_CLDV6212_POE.Models;

namespace ST10150702_CLDV6212_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        // Function URLs
        private readonly string _blobFunctionUrl = "https://cldvst10150702.azurewebsites.net/api/UploadBlob?code=Ke-2Ie96H825ZFq5Y1INxO8JYPlbf4pP8O55fIbASgryAzFuQkJlnw%3D%3D";
        private readonly string _fileFunctionUrl = "https://cldvst10150702.azurewebsites.net/api/UploadFile?code=Ke-2Ie96H825ZFq5Y1INxO8JYPlbf4pP8O55fIbASgryAzFuQkJlnw%3D%3D";
        private readonly string _tableFunctionUrl = "https://cldvst10150702.azurewebsites.net/api/StoreTableInfo?code=Ke-2Ie96H825ZFq5Y1INxO8JYPlbf4pP8O55fIbASgryAzFuQkJlnw%3D%3D";
        private readonly string _queueFunctionUrl = "https://cldvst10150702.azurewebsites.net/api/ProcessQueueMessage?code=Ke-2Ie96H825ZFq5Y1INxO8JYPlbf4pP8O55fIbASgryAzFuQkJlnw%3D%3D";

        public HomeController(HttpClient httpClient, ILogger<HomeController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Explain()
        {
            return View();
        }

        // Blob Storage Upload
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a valid image file.");
                return View("Index");
            }

            try
            {
                using var stream = file.OpenReadStream();
                var result = await _httpClient.PostAsync(_blobFunctionUrl, new StreamContent(stream));

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to upload the image. Please try again.");
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the image.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View("Index");
            }
        }

        // File Share Upload
        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a valid contract file.");
                return View("Index");
            }

            try
            {
                using var stream = file.OpenReadStream();
                var content = new StreamContent(stream);
                content.Headers.Add("Content-Type", file.ContentType);

                var result = await _httpClient.PostAsync($"{_fileFunctionUrl}&filename={file.FileName}", content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to upload the contract. Please try again.");
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the contract.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View("Index");
            }
        }

        // Table Storage: Add Customer Profile
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (profile == null)
            {
                ModelState.AddModelError("", "Invalid customer profile.");
                return View("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var jsonContent = JsonConvert.SerializeObject(profile);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var result = await _httpClient.PostAsync(_tableFunctionUrl, content);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add customer profile. Please try again.");
                        return View("Index");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding the customer profile.");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                    return View("Index");
                }
            }
            return View("Index");
        }

        // Queue Storage: Process Order
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                ModelState.AddModelError("OrderId", "Invalid order ID.");
                return View("Index");
            }

            try
            {
                var jsonContent = JsonConvert.SerializeObject(new { orderId });
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var result = await _httpClient.PostAsync(_queueFunctionUrl, content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to process the order. Please try again.");
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the order.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View("Index");
            }
        }
    }
}
