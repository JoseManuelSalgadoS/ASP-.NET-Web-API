using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI9A.Models;

namespace TestAPI9A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listComments = await _context.Comments.ToListAsync();
            if (listComments == null || listComments.Count == 0)
            {
                return NoContent();
            }
            return Ok(listComments);
        }

        [HttpPost("Store")]
        public async Task<HttpStatusCode> Store([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return HttpStatusCode.BadRequest; //code 400
            }
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return HttpStatusCode.Created; // code 201
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, [FromBody] Comment comment)
        {
            if (comment == null || comment.Id != id)
            {
                return BadRequest();//400
            }
            var entity = await _context.Comments.FindAsync(id);
            if (entity == null)
            {
                return BadRequest();//404
            }
            entity.Title = comment.Title;
            entity.Description = comment.Description;
            entity.Author = comment.Author;
            entity.CreatedAt = comment.CreatedAt;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // [HttpDelete("Destroy/{id}")]
        // public async Task<IActionResult> Destroy(int id)
        // {
        //     var comment = await _context.Comments.FindAsync(id);
        //     if (comment == null)
        //     {
        //         return NotFound(); // Devolver código 404 Not Found si el comentario no existe
        //     }

        //     _context.Comments.Remove(comment);
        //     await _context.SaveChangesAsync();

        //     return NoContent(); // Devolver código 204 No Content si se eliminó correctamente
        // }
    }
}