using System.ComponentModel.DataAnnotations;

namespace ST10150702_CLDV6212_POE.Models
{
    public class Product
    {
        [Key]  // Marking the primary key
        public int pID { get; set; }          // Primary Key
        public string pName { get; set; }     // Product Name
        public decimal pPrice { get; set; }   // Product Price
        public string pType { get; set; }     // Product Type/Category
        public int pStock { get; set; }       // Product Stock Quantity
    }
}
