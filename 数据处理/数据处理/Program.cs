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
                //PeaksMenu p = new PeaksMenu();
                //p.Begin();

                //DealCsvOrExcel csv = new DealCsvOrExcel();

                //CsvMenu menu = new CsvMenu();
                //menu.Begin();

                ExcelOrTxtMenu menu = new ExcelOrTxtMenu();
                menu.Begin();


                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0}", ex.ToString());
            }
        }
    }
}
