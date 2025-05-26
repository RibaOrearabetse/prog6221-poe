namespace CybersecurityAwarenessBot
{
    /// <summary>
    /// Handles all console output formatting and display logic
    /// </summary>
    public class ConsoleRenderer : IResponseRenderer
    {
        /// <summary>
        /// Displays the Cybersecurity Bot ASCII art header
        /// </summary>
        public void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
  ____      _                                        _ _         
 / ___|   _| |__   ___ _ __ ___  ___  ___ _   _ _ __(_) |_ _   _ 
| |  | | | | '_ \ / _ \ '__/ __|/ _ \/ __| | | | '__| | __| | | |
| |__| |_| | |_) |  __/ |  \__ \  __/ (__| |_| | |  | | |_| |_| |
 \____\__, |_.__/ \___|_|  |___/\___|\___|\__,_|_|  |_|\__|\__, |
   / \|___/   ____ _ _ __ ___ _ __   ___  ___ ___          |___/ 
  / _ \ \ /\ / / _` | '__/ _ \ '_ \ / _ \/ __/ __|               
 / ___ \ V  V / (_| | | |  __/ | | |  __/\__ \__ \               
/_/___\_\_/\_/ \__,_|_|_ \___|_| |_|\___||___/___/               
 / ___| |__   __ _| |_| |__   ___ | |_                           
| |   | '_ \ / _` | __| '_ \ / _ \| __|                          
| |___| | | | (_| | |_| |_) | (_) | |_                           
 \____|_| |_|\__,_|\__|_.__/ \___/ \__|                          
            ");
            Console.ResetColor();
            Console.WriteLine("\nWelcome to the Cybersecurity Awareness Assistant for South Africa");
            Console.WriteLine("-------------------------------------------------------------\n");
        }

        /// <summary>
        /// Displays a message in the specified color
        /// </summary>
        /// <param name="message">Text to display</param>
        /// <param name="color">ConsoleColor to use</param>
        public void DisplayColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Displays the help menu with available commands
        /// </summary>
        public void DisplayHelpMenu()
        {
            DisplayColoredMessage("\nAvailable commands:", ConsoleColor.Magenta);
            Console.WriteLine("-------------------");
            Console.WriteLine("- 'cybersecurity': List all cybersecurity topics");
            Console.WriteLine("- 'help': Show this help message");
            Console.WriteLine("- 'exit' or 'quit': End the conversation");
            Console.WriteLine("- Ask about specific topics like 'phishing' or 'password safety'");
            Console.WriteLine("- Or just chat with me!");
        }

        /// <summary>
        /// Displays all available cybersecurity topics
        /// </summary>
        /// <param name="topics">Dictionary of available topics</param>
        public void DisplayCybersecurityTopics(Dictionary<string, string> topics)
        {
            DisplayColoredMessage("\nKey cybersecurity topics for South Africans:", ConsoleColor.Blue);
            Console.WriteLine("------------------------------------------");

            foreach (var topic in topics.Keys)
            {
                Console.WriteLine($"- {topic}");
            }

            Console.WriteLine("\nType any topic name to learn more about it.");
        }

        /// <summary>
        /// Displays the exit message
        /// </summary>
        /// <param name="userName">Current user's name for personalization</param>
        public void DisplayExitMessage(string userName)
        {
            DisplayColoredMessage($"\nGoodbye, {userName}! Remember to stay safe online in South Africa.", ConsoleColor.Magenta);
            Console.WriteLine("Report suspicious activity to your bank or the South African Fraud Prevention Service.");
        }
    }
}