using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMulbauerReport
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadXML read = new ReadXML(@"E:\Госзнак\Muehlbauer PL3000\Muehlbauer PL30000\Файлы с машины\Report_KIZ_TEST_10_1419729_2022_09_22_13_17_20.xml");
            read.ReadXml();
            read.displayXML();
        }
    }
}
