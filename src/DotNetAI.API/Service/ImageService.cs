﻿using OpenAI;
using OpenAI.Images;

namespace DotNetAI.Service
{
    public class ImageService
    {
        private readonly OpenAIClient _openAIClient;
        private readonly string _model;

        public ImageService(OpenAIClient openAIClient, IConfiguration configuration)
        {
            _openAIClient = openAIClient;
            _model = configuration["OpenAI:ImageModel"] ?? "dall-e-3";
        }

        public async Task<IEnumerable<string>> GenerateImage(string prompt, string quality = "hd", int n = 1, int height = 1024, int width = 1024)
        {
            try
            {
                var imageClient = _openAIClient.GetImageClient(_model);

                var options = new ImageGenerationOptions
                {
                    Quality = quality.Equals("hd", StringComparison.OrdinalIgnoreCase) ? GeneratedImageQuality.High : GeneratedImageQuality.Standard,
                    Size = GetSize(height, width)
                };

                var response = await imageClient.GenerateImagesAsync(prompt, n, options);

                return response.Value.Select(result => result.ImageUri.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private GeneratedImageSize GetSize(int height, int width)
        {
            return (height, width) switch
            {
                (256, 256) => GeneratedImageSize.W256xH256,
                (512, 512) => GeneratedImageSize.W512xH512,
                (1024, 1024) => GeneratedImageSize.W1024xH1024,
                _ => GeneratedImageSize.W1024xH1024 // Fallback to default size
            };
        }
    }
}
