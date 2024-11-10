using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10150702_CLDV6212_POE.Models
{
    public class Order
    {
        [Key]  // Marking the primary key
        public int oID { get; set; }          // Primary Key
        public decimal oValue { get; set; }   // Total Order Value

        [ForeignKey("Customer")]  // Foreign key to Customer
        public int cID { get; set; }          // Foreign Key to Customer
        public string cName { get; set; }     // Customer Name (stored during order)

        [ForeignKey("Product")]  // Foreign key to Product
        public int pID { get; set; }          // Foreign Key to Product
        public string pName { get; set; }     // Product Name (stored during order)

        // Navigation properties
        public Customer Customer { get; set; }  // Related Customer
        public Product Product { get; set; }    // Related Product
    }
}
