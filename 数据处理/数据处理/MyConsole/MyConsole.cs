using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class MyConsole
    {
        public static void WriteLine(string data, ConsoleColor color = ConsoleColor.Black)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ResetColor();
        }
    }
}
