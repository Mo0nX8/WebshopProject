using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework;
using Webshop.Services.Services.GPT;

namespace WebshopWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : Controller
    {
        private readonly GlobalDbContext _context;
        private readonly IConfiguration _configuration;

        public ChatBotController(GlobalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            string userQuestion = request.Question;
            string productContext = "";
            var product= await _context.StorageData.FirstOrDefaultAsync(x => userQuestion.Contains(x.ProductName));
            if(product is not null)
            {
                productContext = $"Product Info:\nName: {product.ProductName}\nPrice: {product.Price}\nStock: {product.Quantity}\nDescription: {product.Description}\n";
            }
            string prompt = $"You are an AI assistant for DXMarket webshop. Answer the user question based on the context below.\n{productContext}\nUser: {userQuestion}\nAI:";

            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("OpenAI API kulcs nincs beállítva az appsettings.json-ban.");
            }

            var response = await GPTService.AskAsync(prompt, apiKey);
            ;
            return Ok(new { answer = response ?? "Sajnálom, hiba történt a feldolgozás során." });


        }
        public class ChatRequest
        {
            public string Question { get; set; }
        }
    }
}
