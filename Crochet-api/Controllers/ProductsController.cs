using Crochet_api.Data;
using Crochet_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace Crochet_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        public ProductsController(DataContext context) {
            _context = context;
        }

        [HttpGet]
        public List<Products> GetAll()
        {
            var allProducts = _context.Products.ToList();
            return allProducts;
        }


        [HttpGet("getTypes")]
        public List<ProductsTypeCount> GetTypes()
        {
            List<ProductsTypeCount> productTypes = _context.Products.GroupBy(x => x.Type).Select(i => new ProductsTypeCount {Type = i.Key, Count = i.Select(t => t.Type).Count() }).ToList();
            return productTypes;
        }

        [HttpGet("getProductDetailsByID/{productID}")]
        public Products GetProductByID(int productID)
        {
            Products productDetails = _context.Products.Single(x => x.ProductID == productID);
            return productDetails;
        }


        [HttpGet("getAllDetailsByID/{productID}")]
        public List<Products> getAllDetailsByID(int productID) {
            var allDetails = _context.Products.Join(_context.Images, products => products.ProductID, images => images.ProductID, (products, images) => new { Products = products, Images = images}).Where(x => x.Products.ProductID == productID);
            return new List<Products>();
        }

        [HttpGet("getProductDetailsByType/{productType}")]
        public List<DetailedProducts> getProductDetailsByType(string productType)
        {
            List<ProductsWithImages> allDetails = _context.Products.Join(_context.Images, products => products.ProductID, images => images.ProductID, (products, images) => new ProductsWithImages { Products = products, Images = images }).Where(x => x.Products.Type.ToLower() == productType.ToLower()).ToList();
            List<DetailedProducts> mappedProducts = new List<DetailedProducts>();
            foreach (ProductsWithImages item in allDetails)
            {
                DetailedProducts? mappedItem = mappedProducts.Find(x => x.ProductID == item.Products.ProductID);
                if (mappedItem == null)
                {
                    mappedItem = new DetailedProducts {
                        ProductID = item.Products.ProductID,
                        Type = item.Products.Type,
                        Name = item.Products.Name,
                        Description_Long = item.Products.Description_Long,
                        Description_Small = item.Products.Description_Small,
                        Price = item.Products.Price,
                        Images = new List<Images>()
                    };
                    mappedProducts.Add(mappedItem);
                }
                mappedItem.Images.Add(item.Images);
            }
            return mappedProducts;
        }


        [HttpGet("getProductsByType/{productType}")]
        public List<Products> getProductsByType(string productType) { 
            List<Products> productDetails = _context.Products.Where(x => x.Type.ToLower() == productType.ToLower()).ToList();
            return productDetails;    
        }

        [HttpGet("getImagesByID/{productID}")]
        public List<Images> getProductsByType(int productID)
        {
            List<Images> imageDetails = _context.Images.Where(x => x.ProductID == productID).ToList();
            return imageDetails;
        }

        [HttpGet("search")]
        public List<Products> search(string searchItem)
        {
            List<Products> productFound = _context.Products.Where(x => x.Type.ToLower() == searchItem.ToLower() || x.Name.ToLower() == searchItem.ToLower()).ToList();
            return productFound;

        }
    }
}
