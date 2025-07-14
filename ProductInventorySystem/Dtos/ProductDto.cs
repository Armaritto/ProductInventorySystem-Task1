using System.ComponentModel.DataAnnotations.Schema;

namespace ProductInventorySystem.Dtos
{
    public class ProductDto
    {
        public required string ProductName { get; set; }
        public int ProductQuantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ProductPrice { get; set; }
        public required string ProductCategory { get; set; }
    }
}
