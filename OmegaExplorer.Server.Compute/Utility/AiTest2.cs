using LLama;
using LLama.Common;
using LLama.Sampling;
using System.Diagnostics;

namespace OmegaExplorer.Server.AI.Utility
{
    public class AiTest2
    {
        public async Task Test()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Define the story generation prompt
            string prompt = @"
[INST] 
<<SYS>>
You are a history generator about a fictive space game 4x futuristic where earth or other solar system planet not exist, the story happen in a spaceship named Appolo51 with hero pilot name John Weak with left arm broken, a jealous caracter and only one eye  working on the planet Ralama78. answering in french language. Returns a JSON strictly respecting the following structure:
{
    ""story"": ""string"",
    ""choices"": [
        {
            ""text"": ""string"",
            ""gold_change"": number
        }
    ]
}
<</SYS>>

Generate a fantasy story with 4 choices. Start response with { 
[/INST]";

            string modelPath = @"C:\Users\le_ma\Downloads\phi-4-q4.gguf";
            ModelParams parameters = new ModelParams(modelPath)
            {
                ContextSize = 2048, // The maximum memory length for the conversation.
                GpuLayerCount = 40   // Number of layers offloaded to the GPU.
            };
            using LLamaWeights model = LLamaWeights.LoadFromFile(parameters);
            using LLamaContext context = model.CreateContext(parameters);
            InteractiveExecutor executor = new InteractiveExecutor(context);

            // Initialize chat history with a system message.
            ChatHistory chatHistory = new ChatHistory();

            ChatSession session = new(executor, chatHistory);

            InferenceParams inferenceParams = new InferenceParams()
            {
                MaxTokens = 2048, // Limit the number of tokens to control output length.
                AntiPrompts = new List<string> { "User:" }, // Stop generation when an antiprompt is encountered.
                SamplingPipeline = new DefaultSamplingPipeline(),
            };

            Console.WriteLine("Starting story generation...");
            Console.ForegroundColor = ConsoleColor.Green;

            // Directly generate the story using the prompt without reading user input.
            await foreach (string text in session.ChatAsync(new ChatHistory.Message(AuthorRole.User, prompt), inferenceParams))
            {
                Console.Write(text);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done generating the story in {sw.ElapsedMilliseconds} ms.");
        }
    }
}
