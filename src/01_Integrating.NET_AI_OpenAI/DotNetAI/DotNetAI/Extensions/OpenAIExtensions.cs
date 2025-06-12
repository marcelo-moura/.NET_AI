﻿using OpenAI;

namespace DotNetAI.Extensions
{
    public static class OpenAIExtensions
    {
        public static WebApplicationBuilder AddOpenAI(this WebApplicationBuilder builder)
        {
            // var apiKey = builder.Configuration["OpenAI:Key"];
            var apiKey = Environment.GetEnvironmentVariable("OPEN_AI_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API Key is not set.");
            }

            var openAIClient = new OpenAIClient(apiKey);

            builder.Services.AddSingleton(openAIClient);

            return builder;
        }
    }
}
