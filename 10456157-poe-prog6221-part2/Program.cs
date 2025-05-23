using System;
using NAudio.Wave;

namespace CyberAwarenessBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Play voice greeting
            try
            {
                using (var audioFile = new AudioFileReader("greeting.wav")) // Plays voicew greeting recorded using NAudio
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100); // Wait for audio to finish
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error playing greeting audio: " + ex.Message);
                Console.ResetColor();
            }

            //  Generates ASCII Art Greeting to show when apllication starts
            Console.WriteLine(@"
  ___  _  _  ____  ____  ____  ____  ____  ___  _  _  ____  __  ____  _  _     __   _  _   __   ____  ____  __ _  ____  ____  ____    ____   __  ____ 
 / __)( \/ )(  _ \(  __)(  _ \/ ___)(  __)/ __)/ )( \(  _ \(  )(_  _)( \/ )   / _\ / )( \ / _\ (  _ \(  __)(  ( \(  __)/ ___)/ ___)  (  _ \ /  \(_  _)
( (__  )  /  ) _ ( ) _)  )   /\___ \ ) _)( (__ ) \/ ( )   / )(   )(   )  /   /    \\ /\ //    \ )   / ) _) /    / ) _) \___ \\___ \   ) _ ((  O ) )(  
 \___)(__/  (____/(____)(__\_)(____/(____)\___)\____/(__\_)(__) (__) (__/    \_/\_/(_/\_)\_/\_/(__\_)(____)\_)__)(____)(____/(____/  (____/ \__/ (__)
");

            Console.WriteLine("Welcome to the Cybersecurity Awareness Bot! ");

            // Ask for user's name
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("What’s your name? ");
            string name = Console.ReadLine();
            Console.ResetColor();
            if (string.IsNullOrWhiteSpace(name)) name = "Friend";


            Console.WriteLine($"Hello, {name}! I'm here to help you stay safe online.💻Ask me anything about cybersecurity. Type 'exit' to leave. "); // responds to the user by stating their name and what the user must ask questions about or type to exit.
            //saves the most recent last topic and favorite topic here so that it can recall the topic and respond accordingly.
            string favoriteTopic = "";
            string lastTopic = "";
            // keyword detection
            Dictionary<string, string> keywordResponses = new Dictionary<string, string>
{
    { "password", "🔐 Use strong, unique passwords and avoid using names or birthdays." },
    { "privacy", "🛡️ Adjust your social media and app settings to enhance privacy." },
    { "scam", "🚨 Watch out for offers that seem too good to be true — they're usually scams." }
};



            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nYou: ");
                Console.ResetColor();
                string input = Console.ReadLine()?.ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Please enter a valid question.");
                    Console.ResetColor();
                    continue;
                }

                if (input == "exit")
                {
                    Console.WriteLine("👋 Goodbye! Stay safe online!");
                    break;
                }

                string command = "default";

                // Keyword-based detection
                if (input.Contains("password")) command = "password";
                else if (input.Contains("privacy")) command = "privacy";
                else if (input.Contains("scam")) command = "scam";
                else if (input.Contains("phishing")) command = "phishing";
                else if (input.Contains("how are you")) command = "how";
                else if (input.Contains("purpose")) command = "purpose";
                else if (input.Contains("ask you about")) command = "topics";
                else if (input.Contains("safe browsing")) command = "browsing";
                else if (input.Contains("interested in")) command = "memory";
                else if (input.Contains("more") && lastTopic != "") command = "followup";
                else if (input.Contains("worried") || input.Contains("frustrated") || input.Contains("curious")) command = "sentiment";

                // Switch-style response
                switch (command)
                {
                    case "password":
                    case "privacy":
                    case "scam":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(keywordResponses[command]);
                        Console.ResetColor();
                        lastTopic = command;
                        break;

                    case "phishing":
                        Random rand = new Random();
                        Console.WriteLine(phishingTips[rand.Next(phishingTips.Count)]);
                        lastTopic = "phishing";
                        break;

                    case "how":
                        Console.WriteLine("🤖 I'm running securely and ready to help!");
                        break;

                    case "purpose":
                        Console.WriteLine("🛡️ I help South African citizens learn how to stay safe online.");
                        break;

                    case "topics":
                        Console.WriteLine("💡 You can ask about phishing, password safety, privacy, scams, or safe browsing.");
                        break;

                    case "browsing":
                        Console.WriteLine("🌐 Avoid clicking unknown links and use secure, trusted websites.");
                        lastTopic = "browsing";
                        break;
                    // memory and recall
                    case "memory":
                        int index = input.IndexOf("interested in");
                        if (index != -1)
                        {
                            favoriteTopic = input.Substring(index + 13).Trim();
                            Console.WriteLine($"👍 Got it! I'll remember you're interested in {favoriteTopic}.");
                        }
                        break;
                    // followup to what was said based on last topic. 
                    case "followup":
                        Console.WriteLine($"🔎 Here's more on {lastTopic}:");
                        if (lastTopic == "password")
                            Console.WriteLine("💡 Consider using a password manager and enabling two-factor authentication.");
                        else if (lastTopic == "phishing")
                            Console.WriteLine("📨 Always verify the sender’s identity before clicking links.");
                        else if (lastTopic == "privacy")
                            Console.WriteLine("🔐 Use privacy-focused search engines like DuckDuckGo.");
                        else
                            Console.WriteLine("📘 I'm still learning more about that. Ask another question!");
                        break;
                    //sentimental responses
                    case "sentiment":
                        if (input.Contains("worried"))
                            Console.WriteLine("🤝 It's okay to be worried. Let's take this one step at a time.");
                        else if (input.Contains("frustrated"))
                            Console.WriteLine("😓 I get it. Cybersecurity can be confusing. I'm here to make it easier.");
                        else if (input.Contains("curious"))
                            Console.WriteLine("🧠 Curiosity is great! Ask me anything and let's learn together.");
                        break;

                    default:
                        if (!string.IsNullOrEmpty(favoriteTopic))
                        {
                            Console.WriteLine($"🔁 Not sure how to help with that, {name}, but since you're interested in {favoriteTopic}, maybe ask me more about it.");
                        }
                        else
                        {
                            Console.WriteLine("❓ I didn’t quite understand that. Could you try rephrasing?");
                        }
                        break;
                }
            }
        }
    }
}

