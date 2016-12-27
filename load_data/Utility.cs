using System;
using System.Collections.Generic;

using System.IO;
using System.Threading;
using System.Text;

using ClauTextSharp.wiz;

namespace ClauTextSharp.load_data
{
    public class Utility
    {
        // need reanme?, // for speed up?
        public readonly static String[] specialStr =  { "^",   " ",    "\t",   "\r",   "\n",   "#"  };
        public readonly static String[] specialStr2 = { "^0",  "^1",   "^2",   "^3",   "^4",   "^5" };
        public readonly static Vector<String> reverse_specialStr = new Vector<String>(new List<String> { "#", "\n", "\r", "\t", " ", "^" });
        public readonly static Vector<String> reverse_specialStr2 = new Vector<String>(new List<String> { "^5", "^4", "^3", "^2", "^1", "^0" });
        public readonly static Vector<String> beforeWhitespaceVec = new Vector<String>(new List<String> { " ", "\t", "\r", "\n" });
        public readonly static Vector<String> afterWhitespaceVec = new Vector<String>(new List<String> { "^1", "^2", "^3", "^4" });

   
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
        

        private class DoThreadData // need to rename!
        {
            public Vector<String> strVec;
            public MovableDeck<String> aq;
            public int strVecStart;
            public int strVecEnd;
            public DoThreadData()
            {
                strVec = new Vector<String>();
                aq = new MovableDeck<String>();

                strVecStart = -1;
                strVecEnd = -1;
            }
            public DoThreadData(DoThreadData other)
            {
                strVec = other.strVec;
                aq = other.aq;
                strVecStart = other.strVecStart;
                strVecEnd = other.strVecEnd;
            }
        }
        
        private static bool DoThread(object param) // need to rename!
        {
            
            DoThreadData data = (DoThreadData)param;
            StringTokenizer tokenizer = new StringTokenizer();

            for (int i = data.strVecStart; i <= data.strVecEnd; ++i)
            {
                tokenizer.init(data.strVec.get(i));
           
                while (tokenizer.hasMoreTokens()) {
                    String temp = tokenizer.nextToken();
                    data.aq.push_back(temp);
                }  
            }
          
            return true;
		}

        private class DoThreadData2 // need to rename!
        {
            public Vector<String> strVec;
            public int strVecStart;
            public int strVecEnd;

            public DoThreadData2() { }

            public DoThreadData2(DoThreadData2 other)
            {
                strVec = other.strVec;
                strVecStart = other.strVecStart;
                strVecEnd = other.strVecEnd;
            }
        }

        private static bool DoThread2(object val) {
            DoThreadData2 data = new DoThreadData2((DoThreadData2)val);
            
			for (int i = data.strVecStart; i <= data.strVecEnd; ++i)
			{
				bool chkStr = ChkExist(data.strVec.get(i));
				if (chkStr) {
					data.strVec.set(i, ChangeStr(data.strVec.get(i), specialStr[0], specialStr2[0])); // ^ . ^0
				    data.strVec.set(i, ChangeStr(data.strVec.get(i), specialStr[5], specialStr2[5])); // # . ^5
				}

                data.strVec.set(i, PassSharp(data.strVec.get(i)));
                data.strVec.set(i, AddSpace(data.strVec.get(i)));

				if (chkStr) {
					data.strVec.set(i, ChangeStr(data.strVec.get(i), beforeWhitespaceVec, afterWhitespaceVec));
				}
			}
            return true;
        }

        public static bool ChkExist(String str) /// has bug?, unstatble?
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

            return 0 == state;
        }

        public static Pair<bool, int> Reserve2(TextReader tr, MovableDeck<String> aq, int num, int thread_num,
            Vector<String> strVecTemp, MovableDeck<String>[] MovableDeck, ref bool _end)
        {
            int count = 0;
            String temp = "";
            strVecTemp.clear();
            Vector<Func<Object, bool>> work = new Vector<Func<Object, bool>>(thread_num);
            Vector<Func<Object, bool>> work2 = new Vector<Func<object, bool>>(thread_num);
            Vector<Func<Object, bool>> work3 = new Vector<Func<object, bool>>(thread_num);
            Vector<IAsyncResult> asyncRes = new Vector<IAsyncResult>(thread_num);
            Vector<IAsyncResult> asyncRes2 = new Vector<IAsyncResult>(thread_num);
            Vector<IAsyncResult> asyncRes3 = new Vector<IAsyncResult>(thread_num);


            for (int i = 0; i < num; ++i)
            {
                temp = tr.ReadLine();
                if (null == temp) { _end = true;  break; }
                if (temp.Length == 0) { continue; }
                strVecTemp.push_back(temp);
                count++;
            }

            if (count > 100)
            {
                DoThreadData2 param = new DoThreadData2();
                //Thread[] thread = new Thread[thread_num];
                param.strVec = strVecTemp;

                for (int i = 0; i < thread_num - 1; ++i)
                {
                    param.strVecStart = (count / thread_num) * i;
                    param.strVecEnd = (count / thread_num) * (i + 1) - 1;

                    work2.set(i, DoThread2);
                    asyncRes2.set(i, work2.get(i).BeginInvoke((object)new DoThreadData2(param), null, null));
                    // thread[i] = new Thread(DoThread2);
                    // thread[i].Start(new DoThreadData2(param));
                }
                param.strVecStart = (count / thread_num) * (thread_num - 1);
                param.strVecEnd = count - 1;
                // thread[thread_num-1] = new Thread(DoThread2);
                // thread[thread_num-1].Start(new DoThreadData2(param));
                work2.set(thread_num - 1, DoThread2);
                asyncRes2.set(thread_num - 1, work2.get(thread_num - 1).BeginInvoke((object)new DoThreadData2(param), null, null));

                for (int i = 0; i < thread_num; ++i)
                {
                    work2.get(i).EndInvoke(asyncRes2.get(i));
                }
            }
            else if (count > 0)
            {
                DoThreadData2 param = new DoThreadData2();
                param.strVec = strVecTemp;
                param.strVecStart = 0;
                param.strVecEnd = count - 1;

                DoThread2(new DoThreadData2(param));
            }

            if (count > 100 && !aq.empty()) // chk!!
            {
                DoThreadData param = new DoThreadData();
                
                param.strVec = strVecTemp;

                for (int i = 0; i < thread_num-1; ++i)
                {
                    MovableDeck[i].clear();
                    param.aq = MovableDeck[i];
                    param.strVecStart = (count / thread_num) * i;
                    param.strVecEnd = (count / thread_num) * (i + 1) - 1;
                    //thread[i] = new Thread(DoThread);
                    // thread[i].Start(new DoThreadData(param));
                    work.set(i, DoThread);
                    asyncRes.set(i, work.get(i).BeginInvoke((object)new DoThreadData(param), null, null));
                }

                MovableDeck[thread_num-1].clear();
                param.aq = MovableDeck[thread_num-1];
                param.strVecStart = (count / thread_num) * (thread_num - 1);
                param.strVecEnd = count - 1;
                //thread[thread_num-1] = new Thread(DoThread);
                // thread[thread_num-1].Start(new DoThreadData(param));
                work.set(thread_num-1, DoThread);
                asyncRes.set(thread_num - 1, work.get(thread_num-1).BeginInvoke((object)new DoThreadData(param), null, null));

                for (int i = 0; i < thread_num; ++i)
                {
                    work.get(i).EndInvoke(asyncRes.get(i));
                }


                for (int i = 0; i < thread_num; ++i)
                {
                    aq.add_move(MovableDeck[i]); // has bug? - todo!!
                }
            }
            else if (count > 0)
            {
                DoThreadData param = new DoThreadData();
                param.strVec = strVecTemp;
                param.aq = aq;
                param.strVecStart = 0;
                param.strVecEnd = count - 1;

                DoThread(new DoThreadData(param));
            }

            return new Pair<bool, int>(count > 0, count);
        }

        /// must lineNum > 0
        public static Pair<bool, int> Reserve(TextReader tr, MovableDeck<String> strVec, int num = 1)
        {
            String temp = "";
            int count = 0;
            

            for (int i = 0; i < num; ++i)
            {
                temp = tr.ReadLine();
                if( temp == null ) { break; }
                strVec.push_back(temp);
                count++;
            }
            return new Pair<bool, int>( count > 0, count );
        }

        public static String Top(MovableDeck<String> strVec)
        {
            return strVec.front();
        }
        public static String Pop(MovableDeck<String> strVec)
        {
            return strVec.pop_front();
        }

        private static Stack<String> tempStack = new Stack<String>();
        public static Pair<bool, String> LookUp(MovableDeck<String> strVec, int idx = 1)
        {
            if (strVec.size() <= idx)
            {
                return new Pair<bool, String>(false, "" );
            }
            for (int i = 0; i < idx; ++i)
            {
                tempStack.Push(Pop(strVec));
            }

            String temp = Top(strVec);
            for( int i=0; i < idx; ++i)
            {
                strVec.push_front(tempStack.Pop());
            }

            return new Pair<bool, String>(true, temp);
        }

		public static String AddSpace(String str)
        {
            StringBuilder temp = new StringBuilder();

            for (int i = 0; i < str.Length; ++i)
            {
                /// To Do - chabnge to switch statement.
                if ('=' == str[i])
                {
                    temp.Append(" ");
                    temp.Append("=");
                    temp.Append(" ");
                }
                else if ('{' == str[i])
                {
                    temp.Append(" ");
                    temp.Append("{");
                    temp.Append(" ");
                }
                else if ('}' == str[i])
                {
                    temp.Append(" ");
                    temp.Append("}");
                    temp.Append(" ");
                }
                else
                {
                    temp.Append(str[i]);
                }
            }

            return temp.ToString();
        }

        /// need testing!
        public static String PassSharp(String str)
        {
            StringBuilder temp = new StringBuilder();
            int state = 0;

            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == '#') { state = 1; }
                else if (str[i] == '\n') { state = 0; }

                if (0 == state)
                {
                    temp.Append(str[i]);
                }
            }
            return temp.ToString();
        }


        private static bool _ChangeStr(String str, Vector<String> changed_str, Vector<String> result_str, ref int i, ref int state, ref StringBuilder temp)
        {
            for (int j = 0; j < changed_str.size(); ++j)
            {
                if (StringUtility.Comp(str, i, changed_str.get(j), changed_str.get(j).Length))
                {
                    state = 1;
                    temp.Append(result_str.get(j));
                    i = i + changed_str.get(j).Length - 1;
                    return true;
                }
            }
            return false;
        }
        private static bool _ChangeStr(String str, String changed_str, String result_str, ref int i, ref int state, ref StringBuilder temp)
        {
            if (StringUtility.Comp(str, i, changed_str, changed_str.Length))
            {
                state = 1;
                temp.Append(result_str);
                i = i + changed_str.Length - 1;
                return true;
            }
            return false;
        }

        // 길이가 긴 문자열이 먼저 나와야 한다?
        public static String ChangeStr(String str, String changed_str, String result_str)
        {
            StringBuilder temp = new StringBuilder();
            int state = 0;


            for (int i = 0; i < str.Length; ++i)
            {
                if (0 == state && i == 0 && '\"' == str[i])
                {
                    state = 1;

                    temp.Append(str[i]);
                }
                else if (0 == state && i > 0 && '\"' == str[i] && '\\' != str[i - 1])
                {
                    state = 1;

                    temp.Append(str[i]);
                }
                else if (1 == state && _ChangeStr(str, changed_str, result_str, ref i, ref state, ref temp))
                {
                    //
                }
                else if ((1 == state && i > 0 && '\\' != str[i - 1] && '\"' == str[i]))
                {
                    state = 0;

                    temp.Append('\"');
                }
                else
                {

                    temp.Append(str[i]);
                }
            }

            return temp.ToString();
        }
        public static String ChangeStr(String str, Vector<String> changed_str, Vector<String> result_str)
        {
            StringBuilder temp = new StringBuilder();
            int state = 0;


            for (int i = 0; i < str.Length; ++i)
            {
                if (0 == state && i == 0 && '\"' == str[i])
                {
                    state = 1;
                    temp.Append(str[i]);
                }
                else if (0 == state && i > 0 && '\"' == str[i] && '\\' != str[i - 1])
                {
                    state = 1;
                    temp.Append(str[i]);
                }
                else if (1 == state && _ChangeStr(str, changed_str, result_str, ref i, ref state, ref temp))
                {
                    //
                }
                else if ((1 == state && i > 0 && '\\' != str[i - 1] && '\"' == str[i]))
                {
                    state = 0;
                    temp.Append('\"');
                }
                else
                {
                    temp.Append(str[i]);
                }
            }

            return temp.ToString();
        }

    }
}
