using System.ComponentModel.DataAnnotations;

namespace ST10150702_CLDV6212_POE.Models
{
    public class Customer
    {
        [Key]  // Marking the primary key
        public int cID { get; set; }          // Primary Key
        public string cName { get; set; }     // Customer Name
        public string cSurname { get; set; }  // Customer Surname
        public string cEmail { get; set; }    // Customer Email
        public string cContactNumber { get; set; }  // Contact Number
        public int cAge { get; set; }         // Customer Age
    }
}
