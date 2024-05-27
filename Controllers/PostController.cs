using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SaveLogger.Data;

namespace SaveLogger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly ILogger<PostsController> _logger;

    public PostsController(AppDbContext context, ILogger<PostsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] JObject jsonContent)
    {
        var post = new RawPosts
        {
            Content = jsonContent.ToString()
        };

        _context.RawPosts.Add(post);
        await _context.SaveChangesAsync();

        return Ok(post);
    }

    [HttpGet(Name = "GetPosts")]
    public async Task<ActionResult<IEnumerable<RawPosts>>> GetPosts()
    {
        return await _context.RawPosts.ToListAsync();
    }
}
