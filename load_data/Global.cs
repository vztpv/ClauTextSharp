using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClauTextSharp.load_data
{
    public class Global
    {
        public static bool IsWhiteSpace(char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n';
        }
    }
}
