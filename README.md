# prog6221-poe

Cybersecurity Awareness Chatbot
The Cybersecurity Awareness Chatbot is a C# console application developed to support everyday users in learning about cybersecurity. It was built as part of a Programming summative project to promote digital safety and awareness, with a focus on being approachable, informative, and locally relevant to users in South Africa.

Purpose
The chatbot is designed to:

Make cybersecurity topics easier to understand for the general public

Help users recognize common online threats such as phishing, scams, and malware

Provide practical tips on protecting personal information and staying safe online

Create an engaging and conversational learning experience

Key Features
Keyword Detection: Responds to user input with relevant information based on cybersecurity-related keywords like “malware,” “firewall,” “VPN,” “phishing,” and others.

Sentiment Responses: Offers thoughtful replies if a user seems confused, anxious, or curious, helping them feel understood and supported.

Follow-up Support: Remembers the last topic discussed and can offer more detailed information on request.

User Memory: Keeps track of the user's favorite cybersecurity topic for personalized future tips.

Jokes and Tips: Includes cybersecurity-related jokes and phishing safety tips to keep the interaction light yet informative.

Simple Help Menu: Users can type “help” to see available commands and interaction tips.

Exit Command: Users can leave the conversation at any time by typing “exit” or “quit.”

Technical Summary
Language: C#

Framework: .NET Console Application

Core Components:

InputProcessor: Handles user input and generates relevant responses.

KnowledgeBase: Stores detailed information on cybersecurity topics.

IResponseRenderer: Interface used to render chatbot messages with custom formatting.

ConsoleResponseRenderer: Concrete implementation for displaying messages in the console with color coding.

AudioService (optional): Placeholder for adding future audio functionality.

Local Relevance
This project was created with South African users in mind, where increasing internet use and smartphone access make digital security an important area of focus. The tips and examples are framed in a relatable way to help build better cybersecurity habits across different age groups and experience levels.

How to Use
Run the chatbot console application.

Type a greeting or a cybersecurity-related question (e.g., “What is phishing?”).

Ask for jokes, more details on a topic, or general help.

Type “exit” to end the conversation.

Future Improvements
Add voice support for accessibility

Integrate local examples of scams or news events
