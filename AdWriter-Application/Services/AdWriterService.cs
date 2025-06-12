// AdWriter_Application/Services/AdWriterService.cs
using AdWriter_Application.Interfaces;
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


        public async Task<AdContentResponse> GenerateAdContentAsync2(string description, string language)
        {
            // 1. Alege prompt‐ul în limba selectată, cu instrucțiuni să nu folosească code fences
            string langName = language == "ro" ? "Romanian" : "English";
            string prompt = language == "ro"
                ? $@"
                Acționează ca un copywriter de top pentru social media. Folosește vocabular bogat, elemente emoționale și apeluri clare la acțiune.
                Răspunde **NUMAI** cu JSON raw, fără backticks sau markdown fences, în acest format:
                {{
                  ""titles"": [""title1"", ""title2"", ""title3""],
                  ""descriptions"": [""desc1"", ""desc2""],
                  ""hashtags"": [""#tag1"", ""#tag2"", ""#tag3""]
                }}
                Descriere: {description}"
                                : $@"
                Act as a world-class social media copywriter. Use rich vocabulary, emotional hooks and strong calls-to-action.
                Respond **ONLY** with raw JSON, no backticks or markdown fences, in this format:
                {{
                  ""titles"": [""title1"", ""title2"", ""title3""],
                  ""descriptions"": [""desc1"", ""desc2""],
                  ""hashtags"": [""#tag1"", ""#tag2"", ""#tag3""]
                }}
                Description: {description}";

            // 2. Construiește request-ul
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are a creative social media copywriter." },
            new { role = "user",   content = prompt }
        }
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = JsonContent.Create(requestBody)
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // 3. Trimite și primește răspunsul
            using var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            var raw = await response.Content.ReadAsStringAsync();

            // 4. Extrage conținutul JSON din choices[0].message.content
            using var doc = JsonDocument.Parse(raw);
            var contentText = doc.RootElement
                                 .GetProperty("choices")[0]
                                 .GetProperty("message")
                                 .GetProperty("content")
                                 .GetString()!
                                 .Trim();

            // 5. Înlătură eventuale backticks rămase
            if (contentText.StartsWith("```"))
            {
                // scoate prima linie (```...) și ultimele ``` dacă există
                var lines = contentText.Split('\n');
                // elimină prima și ultima linie
                contentText = string.Join('\n', lines.Skip(1).Take(lines.Length - 2)).Trim();
            }

            // 6. Deserializare JSON în model
            try
            {
                var result = JsonSerializer.Deserialize<AdContentResponse>(
                    contentText,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return result ?? new AdContentResponse();
            }
            catch (JsonException ex)
            {
                // fallback: întoarce gol dacă JSON-ul nu e valid
                Console.Error.WriteLine($"JSON parse error: {ex.Message}");
                return new AdContentResponse();
            }
        }


    }
}
