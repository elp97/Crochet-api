using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Crochet_api.Models
{
    public class Products
    {
        [Key]
        public required int ProductID { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public string? Description_Small { get; set; }
        public string? Description_Long { get; set;}
        public double Price { get; set; }    
    }

    public record ProductsTypeCount
    {
        public required string Type { get; set; }
        public required int Count { get; set; }
    }

    public class ProductsWithImages
    {
        public required Products Products { get; set; }
        public required Images Images { get; set; }
    }

    public class DetailedProducts : Products
    {
        public required List<Images> Images { get; set; }
    }
}
