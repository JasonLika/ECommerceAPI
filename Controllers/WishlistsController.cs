using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace ProductAPI.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public WishlistsController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlists()
        {
            var wishlists = await _context.Wishlists.Include(c => c.Product).ToListAsync();
            return Ok(wishlists);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(Wishlist wishlistitem)
        {
            _context.Wishlists.Add(wishlistitem);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null) {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        // to add exiting product to cart increments quantity
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<Wishlist>> GetWishlistItemByProductId(int productId)
        {
            var wishlistItem = await _context.Wishlists.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (wishlistItem == null)
            {
                return NotFound();
            }
            return Ok(wishlistItem);
        }
    }
}