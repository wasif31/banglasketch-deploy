using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repositories;

namespace WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : Controller
    {
        private readonly DataContext _context;
        public NewsController(DataContext context)
        {
            _context = context;
        }
        
        // GET
        [HttpGet]
        public IActionResult GetNews()
        {
            var newsList = _context.News;
            return Ok(newsList);
        }
        
        [Route("insert")]
        [HttpPost]
        public async Task<IActionResult> InsertAsync(News news)
        {
            /* var newsId = new Guid();
             var cmd = this.MySqlDatabase.Connection.CreateCommand();
             var date = new DateTime();
             cmd.CommandText =
                 $"INSERT INTO news(news_id, date, details) VALUES ('{newsId}', '{date}','{news.Details}' ";
             cmd.ExecuteNonQuery();*/

            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
            return Ok("successful insert"); 
        }
    }
}