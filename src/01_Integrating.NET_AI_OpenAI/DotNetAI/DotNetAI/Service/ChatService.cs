using OpenAI;

namespace DotNetAI.Service
{
    public class ChatService
    {
        private readonly OpenAIClient _openAIClient;
        private readonly string _model;

        public ChatService(OpenAIClient openAIClient, IConfiguration configuration)
        {
            _openAIClient = openAIClient;
            _model = configuration["OpenAI:ChatModel"] ?? "gpt-4o";
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var chatClient = _openAIClient.GetChatClient(_model);
            var response = await chatClient.CompleteChatAsync(prompt);

            return response.Value.Content[^1].Text ?? "No response for OpenAI.";
        }
    }
}
