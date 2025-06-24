﻿using OpenAI;
using OpenAI.Chat;

namespace DotNetAI.Service
{
    public class RecipeService
    {
        private readonly OpenAIClient _openAIClient;
        private readonly string _model;

        public RecipeService(OpenAIClient openAIClient, IConfiguration configuration)
        {
            _openAIClient = openAIClient;
            _model = configuration["OpenAI:ChatModel"] ?? "gpt-4o";
        }

        public async Task<string> GenerateRecipe(string ingredients, string cuisine, string dietaryRestrictions)
        {
            try
            {
                var systemMessage = new SystemChatMessage("You are a professional chef that provides creative and easy-to-follow recipes.");

                var userMessage = new UserChatMessage($"""
                I want to create a recipe using the following ingredients: {ingredients}.
                The cuisine type I prefer is {cuisine}.
                Please consider the following dietary restrictions: {dietaryRestrictions}.
                Please provide me with a detailed recipe including title, list of ingredients, and cooking instructions.
                """);

                var messages = new List<ChatMessage>
                {
                    systemMessage,
                    userMessage
                };

                var options = new ChatCompletionOptions
                {
                    Temperature = 0.4f,
                    MaxOutputTokenCount = 500
                };

                var chatClient = _openAIClient.GetChatClient(_model);

                var response = await chatClient.CompleteChatAsync(messages, options);

                return response.Value.Content[^1].Text ?? "No recipe generated.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
