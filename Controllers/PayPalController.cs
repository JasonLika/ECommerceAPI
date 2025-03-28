using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayPalController : ControllerBase
    {
        private readonly AppDBContext _context;

        public PayPalController(AppDBContext context)
        {
            _context = context;
        }

        // POST /api/paypal/create-order
        // Creates an order from the user's cart, sets status = "Awaiting Payment"
        // DOES NOT remove cart items yet
        /*
        [HttpPost("create-order")]
        [Authorize]
        public async Task<IActionResult> CreatePayPalOrder()
        {
            //private UserProfile user;
            //user = await CartService.GetCurrentUserProfileAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            // Get cart items
            var cartItems = await _context.Carts
                .Where(ci => ci.UserId.ToString() == userId)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                return BadRequest("Cart is empty. Cannot create an order.");
            }

            // Create a new order with status "Awaiting Payment"
            var order = new Order
            {
                UserId = userId,
                Status = "Awaiting Payment",
                CreatedAt = DateTime.UtcNow,
                // Build order items from the cart
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Return the new order ID to the client
            return Ok(order.Id);
        }
        */
        [HttpPost("create-order")]
        [Authorize]
        public async Task<IActionResult> CreatePayPalOrder()
        {
            return Ok(1);
        }
    }
}