using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.ReviewsController{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ReviewsController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(
            [FromQuery] string? search, 
            [FromQuery] string? description, 
            [FromQuery] int? maxRating,
            [FromQuery] int? minRating
            )
        {
            var query = _context.Reviews.AsQueryable();
 
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Title.Contains(search));
            if (!string.IsNullOrEmpty(description))
                query = query.Where(p => p.Description.Contains(description));
            if (maxRating.HasValue)
                query = query.Where(p => p.Rating <= maxRating.Value);
            if (minRating.HasValue)
                query = query.Where(p => p.Rating >= minRating.Value);
 
            return Ok(await query.Include(c => c.Product).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id) {

            var review = await _context.Reviews.FindAsync(id);
            if (review == null) 
            {
                return NotFound();
            }
            
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReview), new { id = review.Id,}, review);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Review review)
        {
            if (id != review.Id) {
                return BadRequest("ID mismatch");
            }

            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}