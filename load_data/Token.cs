using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClauTextSharp.load_data
{
    public class Token
    {
        public String str;
        public bool isComment;
        
        public Token(Token token) { str = token.str; isComment = token.isComment; }
        public Token() { str = ""; isComment = false; }
        public Token(String str, bool isComment = false) { this.str = str; this.isComment = isComment; }
    }
}
