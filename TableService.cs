using Azure.Data.Tables;
using Azure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST10150702_CLDV6212_POE
{
    public class TableService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly string _tableName;

        public TableService(IConfiguration configuration)
        {
            // Initialize the TableServiceClient using the Azure Storage connection string from configuration
            _tableServiceClient = new TableServiceClient(configuration["AzureStorage:ConnectionString"]);
            _tableName = configuration["AzureStorage:TableName"];
        }

        // Method to add an entity to the table
        public async Task AddEntityAsync(TableEntity entity)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.CreateIfNotExistsAsync(); // Create the table if it doesn't exist
            await tableClient.AddEntityAsync(entity);   // Add the entity to the table
        }

        // Method to get an entity by partition key and row key
        public async Task<TableEntity> GetEntityAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            try
            {
                var entity = await tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey);
                return entity.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;  // Entity not found
            }
        }

        // Method to update an entity
        public async Task UpdateEntityAsync(TableEntity entity)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
        }

        // Method to get all entities in the table
        public async Task<List<TableEntity>> GetAllEntitiesAsync()
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            var entities = new List<TableEntity>();

            await foreach (var entity in tableClient.QueryAsync<TableEntity>())
            {
                entities.Add(entity);
            }

            return entities;
        }

        // Method to delete an entity
        public async Task DeleteEntityAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
