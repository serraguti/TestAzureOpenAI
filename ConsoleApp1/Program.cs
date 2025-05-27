using Newtonsoft.Json;
using System.Text;

const string API_KEY = "<Key>";
const string ENDPOINT = "<URL>";
Console.WriteLine("Dime tu pregunta:");
var question = Console.ReadLine();
await AskQuestion(question);

static async Task AskQuestion(string question)
{
    var payload = new
    {
        messages = new object[]
        {
                new {
                    role = "system",
                    content = new object[] {
                        new {
                            type = "text",
                            text = "You are an AI assistant that helps people find information."
                        }
                    }
                },
                new {
                    role = "user",
                    content = new object[] {
                        new {
                            type = "text",
                            text = question
                        }
                    }
                }
        },
        temperature = 0.7,
        top_p = 0.95,
        max_tokens = 800,
        stream = false
    };
    await SendRequest(payload);
}

static async Task SendRequest(object payload)
{
    using (var httpClient = new HttpClient())
    {
        httpClient.DefaultRequestHeaders.Add("api-key", API_KEY);

        var response = await httpClient.PostAsync(ENDPOINT, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            var responseData = jsonObject?.choices?[0]?.message?.content;
            var completionTokens = jsonObject?.usage.completion_tokens;
            var prompt_tokens = jsonObject?.usage.prompt_tokens;
            var total_tokens = jsonObject?.usage.total_tokens;

            if (responseData != null)
            {
                Console.WriteLine(responseData);
                Console.WriteLine("--------------------");
                Console.WriteLine($"Completion tokens: {completionTokens}");
                Console.WriteLine($"Prompt tokens: {prompt_tokens}");
                Console.WriteLine($"Total tokens: {total_tokens}");
                Console.WriteLine("--------------------");
            }
            else
            {
                Console.WriteLine("Response data is null.");
            }
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
        }
    }
}