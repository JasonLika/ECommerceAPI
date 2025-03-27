using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IO;
using Azure.Storage;
using Azure.Storage.Blobs;

namespace ProductAPI.Controllers{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDBContext _context;
        // Modify the controller’s constructor to inject IConfiguration for the connection string:
        private readonly IConfiguration _configuration;

        public ProductsController(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] string? search, 
            [FromQuery] decimal? minPrice, 
            [FromQuery] decimal? maxPrice, 
            [FromQuery] int? minStock, 
            [FromQuery] int? maxStock)
        {
            var query = _context.Products.AsQueryable();
 
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search));
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);
            if (minStock.HasValue)
                query = query.Where(p => p.Stock >= minStock.Value);
            if (maxStock.HasValue)
                query = query.Where(p => p.Stock <= maxStock.Value);
 
            return Ok(await query.Include(c => c.Reviews).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) {

            var product = await _context.Products
                                    .Include(c => c.Reviews)
                                    .FirstOrDefaultAsync(c => c.Id == id);
            return product == null ? NotFound() : product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id,}, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Product product)
        {
            if (id != product.Id) {
                return BadRequest("ID mismatch");
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/upload-image")]
        public async Task<ActionResult> UploadImage(int id, IFormFile file)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

            var containerClient = blobServiceClient.GetBlobContainerClient("product-images");
            await containerClient.CreateIfNotExistsAsync();

            
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }

            
            product.ImageUrl = blobClient.Uri.ToString();
            product.ImageFileName = fileName;
            await _context.SaveChangesAsync();

            return Ok(new { product.ImageUrl, product.ImageFileName });
        }
    }
}