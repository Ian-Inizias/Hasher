using System;

namespace HashGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // for (int i = 0; i < 10; i++)
            // {
            //     Console.WriteLine(GeneratePassword(true, true, true, true, false, 12));
            // }

            Console.WriteLine("Account mail:");
            var email = Console.ReadLine();

            Console.WriteLine("Password: (If empty will generate a random one)");
            var password = Console.ReadLine();

            if (string.IsNullOrEmpty(password))
            {
                password = GeneratePassword(true, true, true, true, false, 12);
            }

            var hash = Hasher.GenerateIdentityV3Hash(password);
            Console.WriteLine("Generated hash: " + hash);

            var verified = Hasher.VerifyIdentityV3Hash(password, hash);
            if (!verified)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("ERROR");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Hash verified: " + verified);

                const string logPath = "Passwd.txt";

                var logWriter = System.IO.File.AppendText(logPath);
                logWriter.WriteLine($"Email: {email}   Password: {password}   Hash: {hash}");
                logWriter.Dispose();
            }
            Console.ReadKey();
        }

        private static string GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword)
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = @"!#$%&*@\";
            const string SPACE_CHARACTER = " ";
            const int PASSWORD_LENGTH_MIN = 8;
            const int PASSWORD_LENGTH_MAX = 128;

            if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
            {
                return "Password length must be between 8 and 128.";
            }

            var characterSet = "";

            if (includeLowercase)
            {
                characterSet += LOWERCASE_CHARACTERS;
            }

            if (includeUppercase)
            {
                characterSet += UPPERCASE_CHARACTERS;
            }

            if (includeNumeric)
            {
                characterSet += NUMERIC_CHARACTERS;
            }

            if (includeSpecial)
            {
                characterSet += SPECIAL_CHARACTERS;
            }

            if (includeSpaces)
            {
                characterSet += SPACE_CHARACTER;
            }

            var password = new char[lengthOfPassword];
            var characterSetLength = characterSet.Length;

            var random = new Random();
            for (var characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

                var moreThanTwoIdenticalInARow =
                    characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                    && password[characterPosition] == password[characterPosition - 1]
                    && password[characterPosition - 1] == password[characterPosition - 2];

                if (moreThanTwoIdenticalInARow)
                {
                    characterPosition--;
                }
            }

            return string.Join(null, password);
        }
    }
}
