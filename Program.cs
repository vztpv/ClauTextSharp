using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClauTextSharp.wiz;
using ClauTextSharp.load_data;

namespace ClauTextSharp
{
    class Program
    {
        static void Main(String[] args)
        {
            /// load and save test
            UserType global = new UserType("global");
            LoadData.LoadDataFromFile("input.eu4", ref global);
            Console.WriteLine("load end");
            LoadData.SaveWizDB(global, "output.eu4", "1");
            Console.WriteLine("save end");
        }
    }
}
