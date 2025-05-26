using System;

namespace CybersecurityAwarenessBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a console output renderer
            var renderer = new ConsoleRenderer();

            // Create an audio service to play greeting audio
            var audioService = new AudioService();

            // Display ASCII art welcome header
            renderer.DisplayAsciiArt();

            // Play welcome greeting audio
            audioService.PlayWelcomeAudio();

            // Ask user for their name with validation
            string userName = GetUserName(renderer);

            // Initialize the knowledge base with the user's name
            var knowledge = new KnowledgeBase(userName);

            // Initialize the input processor, which handles user queries
            var inputProcessor = new InputProcessor(knowledge, renderer, audioService, userName);

            // Start the main chatbot loop to process user input
            RunConversationLoop(renderer, inputProcessor);
        }

        /// <summary>
        /// Prompts the user to enter their name and validates that it is not empty.
        /// Displays a personalized greeting and South African cyber threat examples.
        /// </summary>
        static string GetUserName(ConsoleRenderer renderer)
        {
            string userName;
            do
            {
                renderer.DisplayColoredMessage("Please enter your name: ", ConsoleColor.Magenta);
                userName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userName))
                {
                    renderer.DisplayColoredMessage("Name cannot be empty. Please try again.", ConsoleColor.Red);
                }
            } while (string.IsNullOrWhiteSpace(userName));

            // Greet user and show local threat examples
            renderer.DisplayColoredMessage($"\nHello, {userName}! I'm your Cybersecurity Awareness Assistant.", ConsoleColor.Green);
            Console.WriteLine("Current South African cyber threats you should know about:");
            Console.WriteLine("- Fake SARS eFiling notifications");
            Console.WriteLine("- Banking app SIM swap scams");
            Console.WriteLine("- 'Hi Mum' WhatsApp scams\n");

            return userName;
        }

        /// <summary>
        /// Runs the chatbot's main interaction loop, allowing the user to input questions or commands.
        /// Breaks the loop if the user types 'exit' or 'quit'.
        /// </summary>
        static void RunConversationLoop(ConsoleRenderer renderer, InputProcessor processor)
        {
            // Show initial help options
            renderer.DisplayHelpMenu();

            while (true)
            {
                renderer.DisplayColoredMessage("\nWhat would you like to talk about? (type 'help' for options): ", ConsoleColor.Yellow);
                string input = Console.ReadLine();

                // If ProcessInput returns true, exit the loop (user chose to quit)
                if (processor.ProcessInput(input))
                {
                    break;
                }
            }
        }
    }
}
