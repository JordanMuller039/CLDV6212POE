using Microsoft.AspNetCore.Mvc;
using ST10150702_CLDV6212_POE.Models;
using System.Diagnostics;
using ST10150702_CLDV6212_POE.Controllers;
using Azure.Data.Tables;

namespace ST10150702_CLDV6212_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobServices _blobServices;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        public HomeController(BlobServices blobService, TableService tableService, QueueService queueService, FileService fileService)
        {
            _blobServices = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BlobStorage()
        {
            return View();
        }

        public IActionResult FileShare()
        {
            return View();
        }

        public IActionResult TableStorage()
        {
            return View();
        }

        public IActionResult QueueStorage()
        {
            return View();
        }

        private TableEntity ConvertToTableEntity(CustomerProfile profile)
        {
            var entity = new TableEntity(profile.PartitionKey, profile.RowKey)
            {
                { "FirstName", profile.FirstName },
                { "LastName", profile.LastName },
                { "Email", profile.Email },
                { "DateOfBirth", profile.DateOfBirth.ToString("o") } // Store DateTime as an ISO string
            };

            return entity;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _blobServices.UploadBlobAsync("media", file.FileName, stream);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                var entity = ConvertToTableEntity(profile);
                await _tableService.AddEntityAsync(entity);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderId)
        {
            await _queueService.SendMessageAsync("inventory", $"Processing order {orderId}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _fileService.UploadFileAsync("contracts", file.FileName, stream);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetMessages()
        {
            var messages = await _queueService.ViewMessagesAsync("inventory");
            return Json(messages);
        }

    }
}

