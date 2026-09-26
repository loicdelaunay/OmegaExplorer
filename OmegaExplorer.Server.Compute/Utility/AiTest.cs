using LLama;
using LLama.Common;
using LLama.Sampling;

namespace OmegaExplorer.Server.AI.Utility;

public class AiTest
{
    public async Task Test()
    {
        string modelPath = @"C:\Users\le_ma\Downloads\phi-4-q4.gguf";

        ModelParams parameters = new ModelParams(modelPath)
        {
            ContextSize = 1024, // The longest length of chat as memory.
            GpuLayerCount = 5 // How many layers to offload to GPU. Please adjust it according to your GPU memory.
        };
        using LLamaWeights model = LLamaWeights.LoadFromFile(parameters);

        using LLamaContext context = model.CreateContext(parameters);
        InteractiveExecutor executor = new InteractiveExecutor(context);

        // Add chat histories as prompt to tell AI how to act.
        ChatHistory chatHistory = new ChatHistory();
        chatHistory.AddMessage(AuthorRole.System,
                               "Tu t'appelles Conscience, tu est une ia parodique qui jour le role de maitre du jeu dans OmegaExplorer qui est un jeu 4x tour par tour dans l'espace spatial dans un futur distopique.");

        ChatSession session = new(executor, chatHistory);

        InferenceParams inferenceParams = new InferenceParams()
        {
            MaxTokens = 512, // No more than 256 tokens should appear in answer. Remove it if antiprompt is enough for control.
            AntiPrompts = new List<string> { "User:" }, // Stop generation once antiprompts appear.

            SamplingPipeline = new DefaultSamplingPipeline(),
        };

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("The chat session has started.\nUser: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine() ?? "";

        while (userInput != "exit")
        {
            await foreach ( // Generate the response streamingly.
                           string text
                           in session.ChatAsync(
                                                new ChatHistory.Message(AuthorRole.User, userInput),
                                                inferenceParams))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(text);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            userInput = Console.ReadLine() ?? "";
        }
    }
}