using Microsoft.AspNetCore.Mvc;

namespace ST10150702_CLDV6212_POE.Controllers
{
    public class QueueController : Controller
    {
        private readonly QueueService _queueService;

        public QueueController(QueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _queueService.ViewMessagesAsync("inventory");
            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] string message)
        {
            await _queueService.SendMessageAsync("inventory", message);
            return Ok();
        }
    }
}
