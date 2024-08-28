using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QueueService
{
    private readonly QueueServiceClient _queueServiceClient;

    public QueueService(IConfiguration configuration)
    {
        // Initialize the QueueServiceClient with the Azure Storage connection string
        _queueServiceClient = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
    }

    // Method to receive messages from the queue (dequeues messages)
    public async Task<List<string>> ReceiveMessagesAsync(string queueName)
    {
        var messages = new List<string>();
        var queueClient = _queueServiceClient.GetQueueClient(queueName);

        // Fetch messages from the queue
        var response = await queueClient.ReceiveMessagesAsync(maxMessages: 10);

        // Iterate through the QueueMessage[] in the Response
        foreach (QueueMessage message in response.Value)
        {
            messages.Add(message.MessageText);
        }

        return messages;
    }

    // Method to add (send) a message to the queue
    public async Task SendMessageAsync(string queueName, string messageText)
    {
        var queueClient = _queueServiceClient.GetQueueClient(queueName);

        // Create the queue if it doesn't already exist
        await queueClient.CreateIfNotExistsAsync();

        // Send the message to the queue
        await queueClient.SendMessageAsync(messageText);
    }

    // Method to peek (view) messages from the queue without dequeuing
    public async Task<List<string>> ViewMessagesAsync(string queueName)
    {
        var messages = new List<string>();
        var queueClient = _queueServiceClient.GetQueueClient(queueName);

        // Peek the first 10 messages
        var peekedMessages = await queueClient.PeekMessagesAsync(maxMessages: 10);

        // Add each peeked message to the list
        foreach (PeekedMessage message in peekedMessages.Value)
        {
            messages.Add(message.MessageText);
        }

        return messages;
    }
}
