using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Data
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu m = new Menu();
            m.Init();

            Console.WriteLine("menu启动成功");
            Console.ReadKey();
        }
    }
}
