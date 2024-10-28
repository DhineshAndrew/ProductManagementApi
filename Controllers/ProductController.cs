using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductManagementApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var result = await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = result }, product);
        }


        
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var result = await _productService.UpdateProductAsync(product);
            return result > 0 ? NoContent() : NotFound();
        }


        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return result > 0 ? NoContent() : NotFound();
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] int chunkIndex, [FromForm] int totalChunks)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty or missing.");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var tempFilePath = Path.Combine(uploadsPath, $"{fileName}.part{chunkIndex}");

            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

           
            if (chunkIndex == totalChunks - 1)
            {
                var finalFilePath = Path.Combine(uploadsPath, fileName);
                using (var finalStream = new FileStream(finalFilePath, FileMode.Create))
                {
                    for (int i = 0; i < totalChunks; i++)
                    {
                        var partFilePath = Path.Combine(uploadsPath, $"{fileName}.part{i}");
                        using (var partStream = new FileStream(partFilePath, FileMode.Open))
                        {
                            await partStream.CopyToAsync(finalStream);
                        }
                       
                        System.IO.File.Delete(partFilePath);
                    }
                }

                return Ok(new { FilePath = $"uploads/{fileName}" });
            }

            return Ok(new { message = $"Chunk {chunkIndex + 1} uploaded successfully." });
        }


        [HttpGet]
        [Route("get-file-path/{fileName}")]
        public IActionResult GetFilePath(string fileName)
        {
           
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
              
                return Ok(new { FilePath = $"uploads/{fileName}" });
            }

            return NotFound("File not found.");
        }



    }
}
