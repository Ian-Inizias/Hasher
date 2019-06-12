using System;

namespace HashGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Account mail:");
            string email = Console.ReadLine();

            Console.WriteLine("Password:");
            string password = Console.ReadLine();

            string hash = Hasher.GenerateIdentityV3Hash(password);
            Console.WriteLine("Generated hash: " + hash);

            bool verificado = Hasher.VerifyIdentityV3Hash(password, hash);
            if (!verificado)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("ERROR");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Hash verified: " + verificado);

                var logPath = "Claves.txt";

                var logWriter = System.IO.File.AppendText(logPath);
                logWriter.WriteLine(string.Format("Email: {0}   Password: {1}   Hash: {2}", email, password, hash));
                logWriter.Dispose();
            }
        }
    }
}
