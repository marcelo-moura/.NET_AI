using DotNetAI.Service;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAI.Controllers
{
    [ApiController]
    public class GenerativeAIController : ControllerBase
    {
        private readonly ChatService _chatService;

        public GenerativeAIController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("ask-ai")]
        public async Task<IActionResult> AskAi([FromQuery] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return BadRequest("Prompt cannot be empty.");
            }

            var response = await _chatService.GetResponseAsync(prompt);
            return Ok(response);
        }
    }
}
