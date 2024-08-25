﻿using Azure.Data.Tables;
using ST10150702_CLDV6212_POE.Models;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace ST10150702_CLDV6212_POE
{
    public class TableService
    {
        private readonly TableClient _tableClient;

        public TableService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient("CustomerProfiles");
            _tableClient.CreateIfNotExists();
        }

        public async Task AddEntityAsync(CustomerProfile profile)
        {
            await _tableClient.AddEntityAsync(profile);
        }
    }
}