using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class ExcelOrTxtMenu : MenuModoule
    {
        DealExcelOrTxt _manager;

        public ExcelOrTxtMenu(string path = "")
        {
            _manager = new DealExcelOrTxt(path);
        }

        public void PrintFiles(Dictionary<string, string> datas)
        {
            foreach (KeyValuePair<string, string> file in datas)
            {
                MyConsole.WriteLine(string.Format("{0} {1}", file.Key, file.Value),
                    ConsoleColor.Red);
            }
        }
        public override void PrintMenu()
        {
            MyConsole.WriteLine("***********************************", ConsoleColor.Magenta);
            MyConsole.WriteLine("****** 1 . Foot Dimensions.xls -- length width   ****", ConsoleColor.Magenta);
            MyConsole.WriteLine("****** 2 . Pressures and Forces -- Force graphs zones toe1 toe2-5  ****", ConsoleColor.Magenta);
            MyConsole.WriteLine("****** 0 . Exit    ****************", ConsoleColor.Magenta);
            MyConsole.WriteLine("***********************************", ConsoleColor.Magenta);
        }
        public override bool ReadChoice()
        {
            int ch = 0;

            string str = Console.ReadLine();
            if (str.Length == 1)
            {
                ch = Convert.ToInt32(str[0]);
            }
            else
            {
                ch = (int)ExcelOrTxt.FindLineByPreLine;
            }

            switch (ch)
            {
                case (int)ExcelOrTxt.FindLineByPreLine:
                    FindLineByPreLine();
                    break;
                case (int)ExcelOrTxt.FindMultiLineByPreLine:
                    FindMultiLineByPreLine();
                    break;
                case (int)ExcelOrTxt.Exit:
                    Exit();
                    break;
                default:
                    ReReadChoice();
                    break;
            }
            return true;
        }

        void FindLineByPreLine()
        {
            //黎明_翟 - 翟黎明-2cm-3mm_2 - 2021-5-10 - Foot Dimensions
            //Foot Dimensions文件的length、width
            MyConsole.WriteLine("输入文件名后缀(忽略大小写）：", ConsoleColor.Green);
            string filePostName = Console.ReadLine();

            string[] lineArr = new string[2];
            MyConsole.WriteLine("输入前置行1(忽略大小写）：", ConsoleColor.Green);
            lineArr[0] = Console.ReadLine();
            MyConsole.WriteLine("输入前置行2(忽略大小写）：", ConsoleColor.Green);
            lineArr[1] = Console.ReadLine();

            //文件名，行数据
            Dictionary<string, string> outAllFilesLineData;
            _manager.FindAllFilesLineByPreLine(lineArr, out outAllFilesLineData, filePostName);

            PrintFiles(outAllFilesLineData);
        }

        void FindMultiLineByPreLine()
        {
            //1cm-3mm-2 - 2021-4-30 - Pressures and Forces.xls
            //Pressures and Forces
            //MyConsole.WriteLine("输入文件名后缀(忽略大小写）：", ConsoleColor.Green);
            //string filePostName = Console.ReadLine();

            string[] lineArr = new string[2];
            MyConsole.WriteLine("输入前置行1(忽略大小写）：", ConsoleColor.Green);
            lineArr[0] = Console.ReadLine();
            MyConsole.WriteLine("输入前置行2(忽略大小写）：", ConsoleColor.Green);
            lineArr[1] = Console.ReadLine();

            //文件名，行数据
            Dictionary<string, Dictionary<uint,string>> outAllFilesLineData;
            _manager.FindMultiLine(lineArr, out outAllFilesLineData, "Pressures and Forces");

            //PrintFiles(outAllFilesLineData);
        }

    }

    public enum ExcelOrTxt
    {
        Error = 0,
        Exit = 48,//0
        FindLineByPreLine = 49,//1
        FindMultiLineByPreLine,//2
    }
}
