﻿using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
namespace ST10150702_CLDV6212_POE
{
    public class QueueService
    {
        private readonly QueueServiceClient _queueServiceClient;
        public QueueService(IConfiguration configuration)
        {
            _queueServiceClient = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);
        }
    }
}
