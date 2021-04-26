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
            try
            {
                Menu m = new Menu();
                m.Init();

                Console.ReadKey();
            }
            catch(Exception ex)
            {
                ;
            }
        }
    }
}
