using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

const string deploymentName = "gpt-4.1";
const string endpoint = "<endpoint>";
const string apiKey = "<api-key>";

ChatClient client = new(
    credential: new ApiKeyCredential(apiKey),
    model: deploymentName,
    options: new OpenAIClientOptions()
    {
        Endpoint = new($"{endpoint}"),
    });
Console.WriteLine("Dime tu pregunta:");
var question = Console.ReadLine();

ChatCompletion completion = client.CompleteChat(
     [
         new SystemChatMessage("Eres un ayudante para un examen de programación"),
         new UserChatMessage(question),
     ]);

Console.WriteLine($"Model={completion.Model}");
foreach (ChatMessageContentPart contentPart in completion.Content)
{
    string message = contentPart.Text;
    Console.WriteLine($"Chat Role: {completion.Role}");
    Console.WriteLine("Message:");
    Console.WriteLine(message);
}
