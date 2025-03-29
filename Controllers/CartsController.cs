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
    public class CartsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CartsController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            var carts = await _context.Carts.Include(c => c.Product).ToListAsync();
            return Ok(carts);
        }
       

        /* delete this
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) {

            var product = await _context.Products
                                    .Include(c => c.Reviews)
                                    .FirstOrDefaultAsync(c => c.Id == id);
            return product == null ? NotFound() : product;
        }
        */

        [HttpPost]
        public async Task<IActionResult> AddToCart(Cart cartItem)
        {
            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    [HttpDelete("clear")]
    public async Task<ActionResult> ClearCart()
    {
        // Fetch all cart items
        var cartItems = await _context.Carts.FindAsync();

        // Remove all cart items
        _context.Carts.RemoveRange(cartItems);

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Ok("Cart cleared successfully.");
    }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, Cart updatedCartItem)
        {
            if (id != updatedCartItem.Id)
            {
                return BadRequest("ID in the URL does not match ID in the body.");
            }

            var existingCartItem = await _context.Carts.FindAsync(id);
            if (existingCartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            existingCartItem.Quantity = updatedCartItem.Quantity;

            await _context.SaveChangesAsync();

            return Ok(existingCartItem);
        }
        
        // to add exiting product to cart increments quantity
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<Cart>> GetCartItemByProductId(int productId)
        {
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (cartItem == null)
            {
                return NotFound();
            }
            return Ok(cartItem);
        }

        
///delete this stuff for user id and cart



/*
[HttpPost]
[Authorize]
public async Task<IActionResult> AddToCartt(Cart cartItem)
{
    // Retrieve the UserId from the authenticated user's token
    var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userIdString))
        return Unauthorized();

    // Convert the UserId from string to int
    if (!int.TryParse(userIdString, out var userId))
    {
        return BadRequest("Invalid UserId in token.");
    }

    cartItem.UserId = userId;

    // Check if the product already exists in the cart for this user
    var existingCartItem = await _context.Carts
        .FirstOrDefaultAsync(c => c.ProductId == cartItem.ProductId && c.UserId == userId);

    if (existingCartItem != null)
    {
        // Increment quantity if the product already exists in the user's cart
        existingCartItem.Quantity += cartItem.Quantity;
    }
    else
    {
        // Add the new cart item
        _context.Carts.Add(cartItem);
    }

    await _context.SaveChangesAsync();
    return Ok();
}



[HttpGet]
[Authorize]
public async Task<ActionResult<IEnumerable<Cart>>> GetCartss()
{
    var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userIdString))
        return Unauthorized();

    // Convert the UserId from string to int
    if (!int.TryParse(userIdString, out var userId))
    {
        return BadRequest("Invalid UserId in token.");
    }

    // Filter items by the logged-in user
    var carts = await _context.Carts
        .Where(c => c.UserId == userId)
        .Include(c => c.Product)
        .ToListAsync();

    return Ok(carts);
}
*/
///


    }
}