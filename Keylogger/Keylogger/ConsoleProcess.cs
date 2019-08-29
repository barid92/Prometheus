using System;
using System.Collections.Generic;
using System.Text;

namespace Keylogger
{
    static class ConsoleProcess
    {
        public static void SayHello()
        {
            Console.WriteLine("Hello World !!!1");
            Console.WriteLine(Const.trollFace);
        }
        public static void Say(string word)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(" _prometheus: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(word);
        }
    }
}
