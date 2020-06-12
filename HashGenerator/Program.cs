using System;

namespace HashGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Account mail:");
            string email = Console.ReadLine();

            Console.WriteLine("Password: (If empty will generate a random one)");
            string password = Console.ReadLine();

            if (string.IsNullOrEmpty(password))
            {
                password = GeneratePassword(true, true, true, true, false, 12);
            }

            string hash = Hasher.GenerateIdentityV3Hash(password);
            Console.WriteLine("Generated hash: " + hash);

            bool verified = Hasher.VerifyIdentityV3Hash(password, hash);
            if (!verified)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("ERROR");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Hash verified: " + verified);

                var logPath = "Passwd.txt";

                var logWriter = System.IO.File.AppendText(logPath);
                logWriter.WriteLine(string.Format("Email: {0}   Password: {1}   Hash: {2}", email, password, hash));
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

            string characterSet = "";

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

            char[] password = new char[lengthOfPassword];
            int characterSetLength = characterSet.Length;

            Random random = new Random();
            for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

                bool moreThanTwoIdenticalInARow =
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
