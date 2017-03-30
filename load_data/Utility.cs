using System;
using System.Collections.Generic;

using System.IO;
using System.Threading;
using System.Text;

using ClauTextSharp.wiz;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

namespace ClauTextSharp.load_data
{
    public class Utility
    {
        // need reanme?, // for speed up?
        public readonly static String[] specialStr =  { "^",   " ",    "\t",   "\r",   "\n",   "#"  };
        public readonly static String[] specialStr2 = { "^0",  "^1",   "^2",   "^3",   "^4",   "^5" };
        public readonly static Vector<String> beforeWhitespaceVec = new Vector<String>(new List<String> { " ", "\t", "\r", "\n" });


        public Utility() { }


		public static bool IsInteger( String str) {
			int state = 0;
			for (int i = 0; i < str.Length; ++i) {
				switch (state)
				{
				case 0:
					if ('+' == str[i] || '-' == str[i]) {
						state = 0;
					}
					else if (str[i] >= '0' && str[i] <= '9')
					{
						state = 1;
					}
					else return false;
					break;
				case 1:
					if (str[i] >= '0' && str[i] <= '9') {
						state = 1;
					}
					else return false;
                    break;
				}
			}
			return 1 == state; /// chk..
		}
        public static bool IsNumberInJson(String str)
        {
            int state = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                switch (state)
                {
                    case 0:
                        if ( // '+' == str[i] || // why can`t +
                            '-' == str[i]
                            )
                        {
                            state = 0;
                        }
                        else if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 1;
                        }
                        else { return false; }
                        break;
                    case 1:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 1;
                        }
                        else if (str[i] == '.')
                        {
                            state = 2;
                        }
                        else { return false; }
                        break;
                    case 2:
                        if (str[i] >= '0' && str[i] <= '9') { state = 3; }
                        else { return false; }
                        break;
                    case 3:
                        if (str[i] >= '0' && str[i] <= '9') { state = 3; }
                        else if ('e' == str[i] || 'E' == str[i])
                        {
                            state = 4;
                        }
                        else { return false; }
                        break;
                    case 4:
                        if (str[i] == '+' || str[i] == '-')
                        {
                            state = 5;
                        }
                        else
                        {
                            state = 5;
                        }
                        break;
                    case 5:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 6;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 6:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 6;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }
            }
            return 3 == state || 6 == state;
        }
        public static bool IsDouble(String str)
        {
            int state = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                switch (state)
                {
                    case 0:
                        if ('+' == str[i] || '-' == str[i])
                        {
                            state = 0;
                        }
                        else if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 1;
                        }
                        else { return false; }
                        break;
                    case 1:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 1;
                        }
                        else if (str[i] == '.')
                        {
                            state = 2;
                        }
                        else { return false; }
                        break;
                    case 2:
                        if (str[i] >= '0' && str[i] <= '9') { state = 3; }
                        else { return false; }
                        break;
                    case 3:
                        if (str[i] >= '0' && str[i] <= '9') { state = 3; }
                        else if ('e' == str[i] || 'E' == str[i])
                        {
                            state = 4;
                        }
                        else { return false; }
                        break;
                    case 4:
                        if (str[i] == '+' || str[i] == '-')
                        {
                            state = 5;
                        }
                        else
                        {
                            state = 5;
                        }
                        break;
                    case 5:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 6;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 6:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 6;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }
            }
            return 3 == state || 6 == state;
        }
        public static bool IsDate(String str)
        {
            int state = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                switch (state)
                {
                    case 0:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 1;
                        }
                        else return false;
                        break;
                    case 1:
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            state = 1;
                        }
                        else if (str[i] == '.')
                        {
                            state = 2;
                        }
                        else return false;
                        break;
                    case 2:
                        if (str[i] >= '0' && str[i] <= '9') { state = 2; }
                        else if (str[i] == '.')
                        {
                            state = 3;
                        }
                        else return false;
                        break;
                    case 3:
                        if (str[i] >= '0' && str[i] <= '9') { state = 4; }
                        else return false;
                        break;
                    case 4:
                        if (str[i] >= '0' && str[i] <= '9') { state = 4; }
                        else return false;
                        break;
                }
            }
            return 4 == state;
        }
        public static bool IsMinus(String str)
        {
            return str.Length != 0 && str[0] == '-';
        }
        public static bool IsWhitespace(Char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\t' || ch == '\n';
        }

        // AddSpace : return string
        public static void AddSpace(String str, ref String temp)
		{
			temp = "";

			for (int i = 0; i < str.Length; ++i)
			{
				/// To Do - chabnge to switch statement.
				if ('=' == str[i]) {
                    temp += " = ";
                }
				else if ('{' == str[i])
                {
                    temp += " { ";
				}
				else if ('}' == str[i]) {
                    temp += " } ";
				}
				else {
					temp += str[i];
				}
			}
			//return temp;
		}

        public static Pair<bool, int> Reserve2(TextReader tr, Deck<Token> aq, int num, ref bool _end) // _end?
        {
            int count = 0;
            String temp;
            Vector<String> strVecTemp = new Vector<string>();
			for (int i = 0; i < num; ++i) {
                temp = tr.ReadLine();
                if (null == temp) { _end = true; break; }
				strVecTemp.push_back(temp);
				count++;
			}

            {
                int left = 0; int right = count - 1;

                for (int x = left; x <= right; ++x)
                {
                    //StringTokenizer tokenizer(std::move( (*strVecTemp)[x] ) );
                    //while (tokenizer.hasMoreTokens()) {
                    //	aq.push(tokenizer.nextToken());
                    //}
                    String statement = strVecTemp.get(x);
                    int token_first = 0, token_last = 0; // idx of token in statement.
                    int state = 0;


                    for (int i = 0; i < statement.Length; ++i)
                    {
                        if (0 == state && '\"' == statement[i])
                        {
                            //token_last = i - 1;
                            //if (token_last >= 0 && token_last - token_first + 1 > 0) {
                            //	aq.push_back(statement.substring(token_first, token_last - token_first + 1));
                            //}
                            state = 1;
                            //token_first = i; 
                            token_last = i;
                        }
                        else if (1 == state && '\\' == statement[i - 1] && '\"' == statement[i])
                        {
                            token_last = i;
                        }
                        else if (1 == state && '\"' == statement[i])
                        {
                            state = 0; token_last = i;

                            //aq.push_back(statement.substring(token_first, token_last - token_first + 1));
                            //token_first = i + 1;
                        }

                        if (0 == state && '=' == statement[i])
                        {
                            token_last = i - 1;
                            if (token_last >= 0 && token_last - token_first + 1 > 0)
                            {
                                aq.push_back(new Token(statement.Substring(token_first, token_last - token_first + 1)));
                            }
                            aq.push_back(new Token("="));
                            token_first = i + 1;
                        }
                        else if (0 == state && IsWhitespace(statement[i]))
                        { // isspace ' ' \t \r \n , etc... ?
                            token_last = i - 1;
                            if (token_last >= 0 && token_last - token_first + 1 > 0)
                            {
                                aq.push_back(new Token(statement.Substring(token_first, token_last - token_first + 1)));
                            }
                            token_first = i + 1;
                        }
                        else if (0 == state && '{' == statement[i])
                        {
                            token_last = i - 1;
                            if (token_last >= 0 && token_last - token_first + 1 > 0)
                            {
                                aq.push_back(new Token(statement.Substring(token_first, token_last - token_first + 1)));
                            }
                            aq.push_back(new Token("{"));
                            token_first = i + 1;
                        }
                        else if (0 == state && '}' == statement[i])
                        {
                            token_last = i - 1;
                            if (token_last >= 0 && token_last - token_first + 1 > 0)
                            {
                                aq.push_back(new Token(statement.Substring(token_first, token_last - token_first + 1)));
                            }
                            aq.push_back(new Token("}"));
                            token_first = i + 1;
                        }

                        if (0 == state && '#' == statement[i])
                        { // different from load_data_from_file
                            token_last = i - 1;
                            if (token_last >= 0 && token_last - token_first + 1 > 0)
                            {
                                aq.push_back(new Token(statement.Substring(token_first, token_last - token_first + 1)));
                            }
                            int j = 0;
                            for (j = i; j < statement.Length; ++j)
                            {
                                if (statement[j] == '\n') // cf) '\r' ?
                                {
                                    break;
                                }
                            }
                            --j; // "before enter key" or "before end"

                            if (j - i + 1 > 0)
                            {
                                aq.push_back(new Token(statement.Substring(i, j - i + 1), true));
                            }
                            token_first = j + 2;
                            i = token_first - 1;
                        }
                    }


                    if (token_first < statement.Length)
                    {
                        aq.push_back(new Token(statement.Substring(token_first)));
                    }
                }
            }
            
            return new Pair<bool, int>(count > 0, count);
		}

        public static bool ChkComment(Deck<Token> strVec, load_data.UserType ut, Reserver reserver, int offset)
		{
			if (strVec.size() < offset) {

                reserver.Functor(strVec);
				while (strVec.size() < offset) // 
				{

                    reserver.Functor(strVec);
					if (
                        strVec.size() < offset &&
						reserver.end()
						) {
						return false;
					}
				}
            }

            LinkedListNode<Token> x = strVec.GetFirst();
            int count = 0;

			do {
				if (x.Value.isComment) {
					ut.PushComment(x.Value.str);
                    x = strVec.remove(x);
                }
				else if (count == offset - 1) {
					return true;
				}
				else {
					count++;
                    x = x.Next;
				}

				if (x == null) { // chk
                    reserver.Functor(strVec);
                    x = strVec.GetFirst();
                    for( int k=0; k < count; ++k)
                    {
                        x = x.Next;
                    }
					while (strVec.size() < offset) // 
					{
                        reserver.Functor(strVec);
                        
                        if (
                            strVec.size() < offset &&
							reserver.end()
							) {
							return false;
						}
					}
                    // chk here?
                    x = strVec.GetFirst();
                    for (int k = 0; k < count; ++k)
                    {
                        x = x.Next;
                    }
				}
			} while (true);
		}
	    public static string Top(Deck<Token> strVec, load_data.UserType ut, Reserver reserver)
        {
            if (strVec.empty() || strVec.GetFirst().Value.isComment)
            {
                if (false == ChkComment(strVec, ut, reserver, 1))
                {
                    return "";
                }
            }
            if (strVec.empty()) { return ""; }
            return strVec.GetFirst().Value.str;
         }

		public static bool Pop(Deck<Token> strVec, ref String str, load_data.UserType ut, Reserver reserver)
        {
            if (strVec.empty() || strVec.GetFirst().Value.isComment)
            {
                if (false == ChkComment(strVec, ut, reserver, 1))
                {
                    return false;
                }
            }

            if (strVec.empty())
            {
                return false;
            }

            if (str != null)
            {
                str = strVec.GetFirst().Value.str;
            }
            strVec.pop_front();

            return true;
        }

// lookup just one!
		public static Pair<bool, Token> LookUp(Deck<Token> strVec, load_data.UserType ut, Reserver reserver)
        {
            if (!(strVec.size() >= 2 && false == strVec.GetFirst().Value.isComment && false == strVec.get(1).isComment))
            {
                if (false == ChkComment(strVec, ut, reserver, 2))
                {
                    return new Pair<bool, Token>(false, null);
                }
            }

            if (strVec.size() >= 2)
            {
                return new Pair<bool, Token>(true, strVec.get(1));
            }
            return new Pair<bool, Token>(false, null);
        }
        public static bool ChkExist(String str) // for \"
        {
            int state = -1;

            for (int i = 0; i < str.Length; ++i)
            {
                if (0 >= state && i == 0 && '\"' == str[i])
                {
                    state = 1;
                }
                else if (0 >= state && i > 0 && '\"' == str[i] && '\\' != str[i - 1])
                {
                    state = 1;
                }
                else if (1 == state && i > 0 && '\\' != str[i - 1] && '\"' == str[i])
                {
                    state = 0;
                }
                else if (0 >= state && str[i] == '#')
                {
                    break;
                }
            }

            return 0 == state; // exist and valid !! chk - todo!
        } 
    }
}
