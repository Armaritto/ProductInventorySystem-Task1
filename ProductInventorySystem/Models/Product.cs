using System.ComponentModel.DataAnnotations.Schema;

namespace ProductInventorySystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public required string Category { get; set; }
    }
}
