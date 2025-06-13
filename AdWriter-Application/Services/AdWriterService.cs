// AdWriter_Application/Services/AdWriterService.cs
using AdWriter_Application.Interfaces.Services;
using AdWriter_Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AdWriter_Application.Services
{
    public class AdWriterService : IAdWriterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AdWriterService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenAI:Key"]
                ?? throw new ArgumentNullException("OpenAI:ApiKey missing");
        }

        public async Task<string> GenerateAdContentAsync(string description, string language)
        {
            var url = "https://api.openai.com/v1/chat/completions";
            Console.WriteLine("Calling OpenAI: " + url);
            string prompt = language.ToLower() switch
            {
                "en" => $"""
                You are a creative social media copywriter.

                Write social media content **in English** for the topic:

                "{description}"

                Return the result in this exact format, with **no commentary**:

                Viral Titles:
                - ...
                - ...
                - ...

                Persuasive Descriptions:
                - ...
                - ...
                - ...
                - ...
                - ...

                Popular Hashtags:
                #example1 #example2 #example3
                """,

                _ => $"""
                Ești un copywriter creativ pentru social media.

                Scrie un conținut pentru postare **în limba română** pe tema:

                "{description}"

                Întoarce rezultatul în următorul format, fără alte comentarii:

                Titluri virale:
                - ...
                - ...
                - ...

                Descrieri convingătoare:
                - ...
                - ...
                - ...
                - ...
                - ...

                Hashtaguri populare:
                #exemplu1 #exemplu2 #exemplu3
                """
            };

            string prompt2 = language.ToLower() switch
            {
                "en" => $"""
                Write a complete and engaging social media content for the topic:

                "{description}"

                Return the result in the following format, with **no** extra commentary:

                Viral Titles:
                - ...
                - ...
                - ...

                Persuasive Descriptions:
                - ...
                - ...
                - ...

                Popular Hashtags:
                #example1 #example2 #example3
                """,

                        _ => $"""
                Scrie un conținut complet și atractiv pentru o postare social media pe tema:

                "{description}"

                Întoarce rezultatul în următorul format, fără alte comentarii:

                Titluri virale:
                - ...
                - ...
                - ...

                Descrieri convingătoare:
                - ...
                - ...
                - ...

                Hashtaguri populare:
                #exemplu1 #exemplu2 #exemplu3
                """
            };

            var body = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are a creative social media copywriter." },
            new { role = "user", content = prompt }
        }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(body)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);

            Console.WriteLine($"Status: {response.StatusCode}");
            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw Response: " + raw);

            if (!response.IsSuccessStatusCode)
                return $"Error: {response.StatusCode} - {raw}";

            var result = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
            return result?.Choices.FirstOrDefault()?.Message.Content.Trim() ?? "No result.";
        }
    }
}
