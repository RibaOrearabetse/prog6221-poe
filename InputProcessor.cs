using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot
{
    public class InputProcessor
    {
        // Dependencies and services used by this class
        private readonly KnowledgeBase _knowledge;          // Holds knowledge topics and information
        private readonly IResponseRenderer _renderer;       // Responsible for displaying messages to the user
        private readonly AudioService _audioService;        // Handles audio output (not shown used here but injected)
        private readonly string _userName;                   // Stores the username of the current user

        // Variables to remember conversation context
        private string _lastTopic;                           // Last discussed topic to support follow-ups
        private string _favoriteTopic;                       // User's favorite or interested topic

        // Predefined cybersecurity jokes to lighten conversation
        private readonly List<string> jokes = new List<string>
        {
            "Why did the hacker break up with the internet? Too many insecure connections.",
            "Why don’t hackers take vacations? Because they prefer to work on phishing trips.",
            "I changed my password to 'incorrect' so whenever I forget it, the computer says: 'Your password is incorrect.'",
            "Why was the computer cold? It left its Windows open.",
            "What do you call a hacker who uses Google? A search-and-destroy specialist.",
            "Knock knock. Who’s there? A phishing email. A phishing email who? The one asking for your bank details urgently!",
            "Why did the smartphone go to therapy? It couldn’t deal with all its app issues."
        };

        // Follow-up information keyed by topics, used when user asks for more details
        private readonly Dictionary<string, string> topicFollowUps = new Dictionary<string, string>()
        {
            { "password", "Use two-factor authentication (2FA) wherever possible. Avoid '123456' or 'password' as passwords – seriously!" },
            { "scam", "Watch out for poor grammar, urgent messages, or requests for money. Always double-check with the source." },
            { "privacy", "Be cautious with app permissions and browser extensions. Less is more when it comes to sharing data." },
            { "phishing", "Phishing happens via email, text, or phone. If something feels off, don’t click anything!" },
            { "malware", "Avoid downloading from shady websites. Keep antivirus software updated and don’t click suspicious attachments." },
            { "social engineering", "They may act friendly or urgent. Never share passwords or private info, even with 'colleagues' you just met." },
            { "2fa", "App-based 2FA (like Authy or Google Authenticator) is better than SMS. Use it for critical accounts." },
            { "firewall", "A firewall helps block unauthorized traffic. Think of it like a bouncer for your data." },
            { "vpn", "A VPN hides your online activity from hackers and ISPs. Very useful on public Wi-Fi!" },
            { "email", "Don't trust email links. When in doubt, go directly to the company website instead." },
        };

        // Basic keyword responses providing simple explanations about cybersecurity topics
        private readonly Dictionary<string, string> keywordResponses = new Dictionary<string, string>()
        {
            { "malware", "Malware is malicious software like viruses, ransomware, or spyware. It can steal or destroy your data!" },
            { "password", "Strong passwords matter! Use at least 12 characters with symbols, numbers, and uppercase letters." },
            { "scam", "Online scams trick people into giving up info or money. Always double-check before clicking." },
            { "privacy", "Protect your personal data online. Avoid oversharing and use privacy settings on social media." },
            { "social engineering", "Social engineering tricks people into giving up confidential info by pretending to be someone trustworthy." },
            { "2fa", "Two-factor authentication adds a second step when logging in. It’s like a secret handshake for your account!" },
            { "firewall", "A firewall acts like a shield that filters out bad internet traffic trying to reach your system." },
            { "vpn", "A VPN encrypts your internet activity. Super helpful when using public Wi-Fi or accessing blocked content." },
            { "email", "Always check the sender’s address. Legit companies won’t ask for your password or personal info via email." },
        };

        // Predefined responses based on detected sentiment words in user input
        private readonly Dictionary<string, string> sentimentResponses = new Dictionary<string, string>()
        {
            { "worried", "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe." },
            { "frustrated", "I'm sorry you're feeling frustrated. Cybersecurity can feel overwhelming, but I’m here to help simplify it." },
            { "anxious", "Cyber threats can be scary, but you're taking the right step by learning more. Let's go at your pace." },
            { "curious", "Curiosity is the first step to awareness! Ask me anything about staying safe online." },
            { "confused", "No problem! I'm here to explain anything you need in simple terms." },
        };

        // List of phishing tips to be shared on request
        private readonly List<string> phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Hover over links to see the real URL before clicking. If it looks suspicious, don’t touch it!",
            "Never download attachments from unknown senders. They could contain malware.",
            "Phishing can occur via email, SMS (smishing), or even voice calls (vishing).",
            "When in doubt, contact the organization directly using a verified phone number or website."
        };

        private readonly Dictionary<string, string> casualResponses = new Dictionary<string, string>
{
    { "hello", "Hello there! 👋 How can I assist you with cybersecurity today?" },
    { "hi", "Hi! 😊 What would you like to learn about today?" },
    { "hey", "Hey! Ready to boost your cyber-awareness?" },
    { "how are you", "I'm just code, but I'm running great! Thanks for asking. How can I help you stay safe online?" },
    { "what's up", "Staying secure and helping folks like you! How can I assist you today?" },
};


        // Constructor to initialize dependencies and user context
        public InputProcessor(KnowledgeBase knowledge, IResponseRenderer renderer, AudioService audioService, string userName)
        {
            _knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _userName = userName ?? string.Empty;
            _lastTopic = string.Empty;
            _favoriteTopic = string.Empty;
        }

        // Processes user input and returns true if user wants to exit, otherwise false
        public bool ProcessInput(string input)
        {
            // Handle empty input gracefully
            if (string.IsNullOrWhiteSpace(input))
            {
                _renderer.DisplayColoredMessage("Please enter something so I can help!", ConsoleColor.Yellow);
                return false;
            }

            input = input.ToLower().Trim();  // Normalize input to lowercase for easier matching

            // Help command: show available commands and tips
            if (input == "help")
            {
                _renderer.DisplayColoredMessage("Available commands:", ConsoleColor.Cyan);
                _renderer.DisplayColoredMessage("- Type 'cybersecurity' to see a list of topics.", ConsoleColor.White);
                _renderer.DisplayColoredMessage("- Ask about topics like 'malware', 'privacy', 'phishing', etc.", ConsoleColor.White);
                _renderer.DisplayColoredMessage("- Say 'more' to get additional info on the last topic.", ConsoleColor.White);
                _renderer.DisplayColoredMessage("- Say 'joke' or 'funny' to hear a cybersecurity joke.", ConsoleColor.White);
                _renderer.DisplayColoredMessage("- Say 'exit' or 'quit' to leave the chatbot.", ConsoleColor.White);
                return false;
            }

            // Exit command: ends conversation
            if (input == "exit" || input == "quit")
            {
                _renderer.DisplayColoredMessage("Stay safe out there! Goodbye!", ConsoleColor.Cyan);
                return true;
            }

            // Jokes command: randomly selects and shows a joke
            if (input.Contains("joke") || input.Contains("funny"))
            {
                var random = new Random();
                int index = random.Next(jokes.Count);
                _renderer.DisplayColoredMessage("😂 Here's a cybersecurity joke:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(jokes[index], ConsoleColor.White);
                return false;
            }

            // Sentiment detection: reply with empathy and optionally provide topic info
            foreach (var sentiment in sentimentResponses)
            {
                if (input.Contains(sentiment.Key))
                {
                    _renderer.DisplayColoredMessage(sentiment.Value, ConsoleColor.Magenta);

                    // If input also contains a cybersecurity topic keyword, provide info on that topic
                    foreach (var keyword in keywordResponses.Keys)
                    {
                        if (input.Contains(keyword))
                        {
                            _lastTopic = keyword;
                            _renderer.DisplayColoredMessage(keywordResponses[keyword], ConsoleColor.White);
                            return false;
                        }
                    }
                    return false;
                }
            }

            // Remember user's favorite topic if they express interest
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains("interested in") && input.Contains(keyword))
                {
                    _favoriteTopic = keyword;
                    _lastTopic = keyword;
                    _renderer.DisplayColoredMessage($"Great! I'll remember that you're interested in {keyword}. It's a crucial part of staying safe online.", ConsoleColor.Green);
                    return false;
                }
            }

            // Remind user of their favorite topic if requested
            if (input.Contains("remind me") || input.Contains("favourite topic"))
            {
                if (!string.IsNullOrEmpty(_favoriteTopic))
                {
                    _renderer.DisplayColoredMessage($"As someone interested in {_favoriteTopic}, you might want to review your {_favoriteTopic} practices regularly.", ConsoleColor.Green);
                }
                else
                {
                    _renderer.DisplayColoredMessage("I don't know your favourite topic yet. Tell me by saying something like: 'I'm interested in phishing.'", ConsoleColor.Yellow);
                }
                return false;
            }

            // Show the list of cybersecurity topics available
            if (input == "cybersecurity")
            {
                _renderer.DisplayColoredMessage("Here are some cybersecurity topics you can ask me about:", ConsoleColor.Cyan);
                foreach (var topic in keywordResponses.Keys)
                {
                    _renderer.DisplayColoredMessage($"- {topic}", ConsoleColor.White);
                }
                return false;
            }

            // Provide follow-up details on last topic when user asks for more info
            if (input.Contains("more") || input.Contains("explain") || input.Contains("details") || input.Contains("don't understand"))
            {
                if (!string.IsNullOrEmpty(_lastTopic) && topicFollowUps.ContainsKey(_lastTopic))
                {
                    _renderer.DisplayColoredMessage($"Here's more on {_lastTopic}: {topicFollowUps[_lastTopic]}", ConsoleColor.White);
                }
                else
                {
                    _renderer.DisplayColoredMessage("Can you clarify what you'd like more information about?", ConsoleColor.Yellow);
                }
                return false;
            }

            // Direct request for more info about a specific topic
            if (input.StartsWith("more about") || input.StartsWith("tell me more about"))
            {
                foreach (var topic in topicFollowUps.Keys)
                {
                    if (input.Contains(topic))
                    {
                        _lastTopic = topic;
                        _renderer.DisplayColoredMessage($"Here's more on {topic}: {topicFollowUps[topic]}", ConsoleColor.White);
                        return false;
                    }
                }

                _renderer.DisplayColoredMessage("Hmm... I’m not sure what topic you meant. Try saying something like 'malware', 'VPN', or 'firewall'.", ConsoleColor.Yellow);
                return false;
            }

            // Provide random phishing tip on request
            if (input.Contains("phishing tip"))
            {
                var random = new Random();
                int index = random.Next(phishingTips.Count);
                _lastTopic = "phishing";
                _renderer.DisplayColoredMessage("🎣 Here's a phishing tip (and no, not the kind with worms):", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(phishingTips[index], ConsoleColor.White);
                return false;
            }

            // Provide basic info on any recognized keyword topic in user input
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    _lastTopic = keyword;
                    _renderer.DisplayColoredMessage(keywordResponses[keyword], ConsoleColor.White);
                    return false;
                }
            }

            // If input exactly matches a knowledge topic, provide detailed info from knowledge base
            if (_knowledge.Topics.ContainsKey(input))
            {
                _lastTopic = input;
                _renderer.DisplayColoredMessage(_knowledge.Topics[input], ConsoleColor.White);
                return false;
            }

            // Casual conversation
            foreach (var casual in casualResponses)
            {
                if (input.Contains(casual.Key))
                {
                    _renderer.DisplayColoredMessage(casual.Value, ConsoleColor.Cyan);
                    return false;
                }
            }

            // Default response if input is unrecognized
            _renderer.DisplayColoredMessage("Sorry, I didn’t quite get that. Try typing 'help' to see what you can ask me.", ConsoleColor.Yellow);
            return false;
        }
    }
}
