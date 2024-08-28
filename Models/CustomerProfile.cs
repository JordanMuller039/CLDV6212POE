using Azure.Data.Tables;
using Azure;
using System;

namespace ST10150702_CLDV6212_POE.Models
{
    public class CustomerProfile
    {
        public string PartitionKey { get; set; } // Required for Table Storage
        public string RowKey { get; set; } // Required for Table Storage
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

