using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


// This app demonstrates the "Strategy" design pattern by using three encryption algos
// CeasarCipher, RSAEncryption and AESEncryption
// After starting this app input some text and select your encryption algo
// to leave type 'exit'

namespace Strategy
{
    public interface IEncryptionStrategy // Strategy interface to be implemented by each conrete strategy
    {
        string Encrypt(string text);
    }

    public class EncryptionContext // Context class 
    {
        private IEncryptionStrategy _algorithm;

        public EncryptionContext(IEncryptionStrategy algorithm)
        {
            _algorithm = algorithm;
        }

        public void SetStrategy(IEncryptionStrategy algorithm)
        {
            _algorithm = algorithm;
        }

        public string ExecuteStrategy(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text cannot be null or empty!!");
            }
            return _algorithm.Encrypt(text);
        }
    }

    public class UserInputHandler
    {
        public static string GetUserInput()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input cannot be null or empty!!");
            }
            return input;
        }
    }

    class Program // Client side app
    {
        static void Main(string[] args)
        {
            EncryptionContext context = new (new NoEncryption());
            Dictionary<string, IEncryptionStrategy> strategies = new()
            {
                {"1", new CaesarCipher()},
                {"2", new RSACipher()},
                {"3", new AESCipher()},
                {"4", new NoEncryption()}
            };

            try
            {
                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter text to encrypt: ");
                    string text = UserInputHandler.GetUserInput();

                    if (text.ToLower() == "exit")
                    {
                        break;
                    }

                    Console.WriteLine();
                    Console.WriteLine("Choose an encryption algorithm (or type 'exit' to exit):");
                    Console.WriteLine();
                    foreach (var pair in strategies)
                    {
                        Console.WriteLine($"{pair.Key} - {pair.Value.GetType().Name}");
                    }
                    string choice = UserInputHandler.GetUserInput();

                    if (choice.ToLower() == "exit")
                    {
                        break;
                    }

                    if (strategies.ContainsKey(choice))
                    {
                        context.SetStrategy(strategies[choice]);
                        string encryptedText = context.ExecuteStrategy(text);
                        Console.WriteLine();
                        Console.WriteLine($"Before: {text}");
                        Console.WriteLine();
                        Console.WriteLine($"After: {encryptedText}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice, try again.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}
