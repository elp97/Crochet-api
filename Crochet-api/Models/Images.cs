using System.ComponentModel.DataAnnotations;

namespace Crochet_api.Models
{
    public class Images
    {
        [Key]
        public required int ImageID {  get; set; }
        public required int ProductID { get; set; }
        public required string ImageURL { get; set; }
    }
}
