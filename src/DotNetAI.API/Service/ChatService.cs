using OpenAI;
using OpenAI.Chat;

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
            try
            {
                var chatClient = _openAIClient.GetChatClient(_model);
                var response = await chatClient.CompleteChatAsync(prompt);

                return response.Value.Content[^1].Text ?? "No response for OpenAI.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> GetResponseWithOptionsAsync(string prompt)
        {
            try
            {
                var chatClient = _openAIClient.GetChatClient(_model);

                var messages = new List<ChatMessage>
                {
                    new UserChatMessage(prompt)
                };

                var options = new ChatCompletionOptions
                {
                    Temperature = 0.4f,
                    MaxOutputTokenCount = 200
                };

                var response = await chatClient.CompleteChatAsync(messages, options);

                return response.Value.Content[^1].Text ?? "No response for OpenAI.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
