using System;

namespace CybersecurityAwarenessBot
{
    // This class implements the IResponseRenderer interface
    // It is responsible for displaying messages to the console with specified colors
    public class ConsoleResponseRenderer : IResponseRenderer
    {
        // Displays a message on the console with the given text color
        public void DisplayColoredMessage(string message, ConsoleColor color)
        {
            // Set the console text color to the specified color
            Console.ForegroundColor = color;

            // Write the message to the console, followed by a new line
            Console.WriteLine(message);

            // Reset the console text color to the default color
            Console.ResetColor();
        }
    }
}
