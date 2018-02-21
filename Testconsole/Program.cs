using PwnedPasswords.net;
using System;

namespace Testconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var pwd = new PwnedPasswordsClient();
            Console.WriteLine("Hello World!");

            if(pwd.CheckPassword("password"))
            {
                Console.WriteLine("pwned!");
            }
            else
            {
                Console.WriteLine("Not pwned!");
            }

            Console.ReadKey();
        }
    }
}
