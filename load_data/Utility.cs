using System;
using System.Collections.Generic;

using System.IO;
using System.Threading;

using ClauTextSharp.wiz;

namespace ClauTextSharp.load_data
{
    class Utility
    {

        // need reanme?, // for speed up?
        private static String[] specialStr =  { "^",   " ",    "\t",   "\r",   "\n",   "#"  };
        private static String[] specialStr2 = { "^0",  "^1",   "^2",   "^3",   "^4",   "^5" };
        private static Vector<String> beforeStrVec = new Vector<String>(new List<String> { " ", "\t", "\r", "\n" });
        private static Vector<String> afterStrVec = new Vector<String>(new List<String> { "^1", "^2", "^3", "^4" });

   
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

        public static Pair<bool, bool> PassSharp(FileStream inFile, ArrayQueue<String> strVec)
        {
            String temp;
            StreamReader sr = new StreamReader(inFile);
            bool chk = sr.EndOfStream;
            temp = sr.ReadLine();
            sr = null;

            if (false == chk)
            {
                StringTokenizer tokenizer = new StringTokenizer(temp, "#");

                if (tokenizer.countTokens() == 1 && tokenizer.isFindExist())
                {
                    //
                }
                else if (tokenizer.countTokens() >= 1)
                {
                    temp = tokenizer.nextToken();
                    if (temp.Length != 0)
                    {
                        strVec.push(temp);
                        return new Pair<bool, bool>(true, true);
                    }
                }
            }
            return new Pair<bool, bool>(chk, false);
        }

        private class DoThreadData // need to rename!
        {
            public Vector<String> strVec;
            public ArrayQueue<String> aq;
            public int strVecStart;
            public int strVecEnd;
        }
        private static void DoThread(object param) // need to rename!
        {
            DoThreadData data = (DoThreadData)param;
            for (int i = data.strVecStart; i <= data.strVecEnd; ++i)
            {
                StringTokenizer tokenizer = new StringTokenizer(data.strVec.get(i));
                while (tokenizer.hasMoreTokens()) {
                    String temp = tokenizer.nextToken();
                    data.aq.push(temp);
                }
            }
		}

        private class DoThreadData2 // need to rename!
        {
            public Vector<String> strVec;
            public int strVecStart;
            public int strVecEnd;
        }

        private static void DoThread2(object val) {
            DoThreadData2 data = (DoThreadData2)val;

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
					data.strVec.set(i, ChangeStr(data.strVec.get(i), beforeStrVec, afterStrVec));
				}
			}
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
        public static Pair<bool, int> Reserve2(FileStream inFile, ArrayQueue<String> aq, int num = 1)
        {
            int count = 0;
            String temp = "";
            Vector<String> strVecTemp = new Vector<string>();
            ArrayQueue<String>[] arrayQueue = new ArrayQueue<String>[4];
            StreamReader sr = new StreamReader(inFile);

            for (int i = 0; i < num && false == sr.EndOfStream; ++i)
            {
                temp = sr.ReadLine();
                if (temp.Length == 0) { continue; }
                strVecTemp.push_back(temp);
                count++;
            }

            if (count >= 100)
            {
                DoThreadData2 param = new DoThreadData2();
                Thread threadA, threadB, threadC, threadD;
                param.strVec = strVecTemp;

                param.strVecStart = 0;
                param.strVecEnd = count / 4 - 1;

                threadA = new Thread(DoThread2);
                threadA.Start(param);

                param.strVecStart = count / 4;
                param.strVecEnd = (count / 4) * 2 - 1;
                threadB = new Thread(DoThread2);
                threadB.Start(param);

                param.strVecStart = (count / 4) * 2;
                param.strVecEnd = (count / 4) * 3 - 1;
                threadC = new Thread(DoThread2);
                threadC.Start(param);

                param.strVecStart = (count / 4) * 3;
                param.strVecEnd = count - 1;
                threadD = new Thread(DoThread2);
                threadD.Start(param);

                threadA.Join();
                threadB.Join();
                threadC.Join();
                threadD.Join();
            }
            else if (count > 0)
            {
                DoThreadData2 param = new DoThreadData2();
                param.strVec = strVecTemp;
                param.strVecStart = 0;
                param.strVecEnd = count - 1;

                DoThread2(param);
            }

            if (count >= 100)
            {
                DoThreadData param = new DoThreadData();
                Thread threadA, threadB, threadC, threadD;
                param.strVec = strVecTemp;
                param.aq = aq;

                param.strVecStart = 0;
                param.strVecEnd = count / 4 - 1;

                threadA = new Thread(DoThread);
                threadA.Start(param);

                param.strVecStart = count / 4;
                param.strVecEnd = (count / 4) * 2 - 1;
                threadB = new Thread(DoThread);
                threadB.Start(param);

                param.strVecStart = (count / 4) * 2;
                param.strVecEnd = (count / 4) * 3 - 1;
                threadC = new Thread(DoThread);
                threadC.Start(param);

                param.strVecStart = (count / 4) * 3;
                param.strVecEnd = count - 1;
                threadD = new Thread(DoThread);
                threadD.Start(param);

                threadA.Join();
                threadB.Join();
                threadC.Join();
                threadD.Join();

                for (int i = 0; i < 4; ++i)
                {
                    aq.push(arrayQueue[i]);
                }
            }
            else if (count > 0)
            {
                DoThreadData param = new DoThreadData();
                param.strVec = strVecTemp;
                param.aq = aq;
                param.strVecStart = 0;
                param.strVecEnd = count - 1;

                DoThread(param);
            }

            return new Pair<bool, int>(count > 0, count);
        }

        /// must lineNum > 0
        public static Pair<bool, int> Reserve(FileStream inFile, ArrayQueue<String> strVec, int num = 1)
        {
            string temp = "";
            int count = 0;

            StreamReader sr = new StreamReader(inFile);

            for (int i = 0; i < num && false == sr.EndOfStream; ++i)
            {
                temp = sr.ReadLine();
                strVec.push(temp);
                count++;
            }
            sr = null;
            return new Pair<bool, int>( count > 0, count );
        }

        public static String Top(ArrayQueue<String> strVec)
        {
            return strVec.get(0);
        }
        public static String Pop(ArrayQueue<String>  strVec)
        {
            return strVec.pop();
        }
        public static int GetIndex(ArrayQueue<String> strVec, String str)
        {
            int idx = -1;

            for (int i = 0; i < strVec.size(); ++i)
            {
                String x = strVec.get(i);
                idx++;
                if (x == str)
                {
                    return idx;
                }
            }
            return -1;
        }

        public static Pair<bool, String> LookUp(ArrayQueue<String> strVec, int idx = 1)
        {
            if (strVec.size() <= idx)
            {
                return new Pair<bool, String>(false, "" );
            }
            return new Pair<bool, String>(true, strVec.get(idx));
        }
        /// must strVec[start] == up or down
        /// now not use!!
    
		// To Do
		// AddSpace : return string
		public static string AddSpace(String str)
        {
            string temp = "";

            for (int i = 0; i < str.Length; ++i)
            {
                /// To Do - chabnge to switch statement.
                if ('=' == str[i])
                {
                    temp = temp + " ";
                    temp = temp + "=";
                    temp = temp + " ";
                }
                else if ('{' == str[i])
                {
                    temp = temp + " ";
                    temp = temp + "{";
                    temp = temp + " ";
                }
                else if ('}' == str[i])
                {
                    temp = temp + " ";
                    temp = temp + "}";
                    temp = temp + " ";
                }
                else
                {
                    temp = temp + str[i];
                }
            }

            return temp;
        }

        /// need testing!
        public static String PassSharp(String str)
        {
            string temp = "";
            int state = 0;

            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == '#') { state = 1; }
                else if (str[i] == '\n') { state = 0; }

                if (0 == state)
                {
                    temp = temp + str[i];
                }
            }
            return temp;
        }


        private static bool _ChangeStr(String str, Vector<String> changed_str, Vector<String> result_str, ref int i, ref int state, String temp)
        {
            for (int j = 0; j < changed_str.size(); ++j)
            {
                if (StringUtility.Comp(str, i, changed_str.get(j), changed_str.get(j).Length))
                {
                    state = 1;
                    temp = temp + result_str.get(j);
                    i = i + changed_str.get(j).Length - 1;
                    return true;
                }
            }
            return false;
        }
        private static bool _ChangeStr(String str, String changed_str, String result_str, ref int i, ref int state, String temp)
        {
            if (StringUtility.Comp(str, i, changed_str, changed_str.Length))
            {
                state = 1;
                temp = temp + result_str;
                i = i + changed_str.Length - 1;
                return true;
            }
            return false;
        }

        // 길이가 긴 문자열이 먼저 나와야 한다?
        public static String ChangeStr(String str, String changed_str, String result_str)
        {
            String temp = "";
            int state = 0;


            for (int i = 0; i < str.Length; ++i)
            {
                if (0 == state && i == 0 && '\"' == str[i])
                {
                    state = 1;
                    temp = temp + str[i];
                }
                else if (0 == state && i > 0 && '\"' == str[i] && '\\' != str[i - 1])
                {
                    state = 1;
                    temp = temp + str[i];
                }
                else if (1 == state && _ChangeStr(str, changed_str, result_str, ref i, ref state, temp))
                {
                    //
                }
                else if ((1 == state && i > 0 && '\\' != str[i - 1] && '\"' == str[i]))
                {
                    state = 0;
                    temp = temp + '\"';
                }
                else
                {
                    temp = temp + str[i];
                }
            }

            return temp;
        }
        public static String ChangeStr(String str, Vector<String> changed_str, Vector<String> result_str)
        {
            String temp = "";
            int state = 0;


            for (int i = 0; i < str.Length; ++i)
            {
                if (0 == state && i == 0 && '\"' == str[i])
                {
                    state = 1;
                    temp = temp + str[i];
                }
                else if (0 == state && i > 0 && '\"' == str[i] && '\\' != str[i - 1])
                {
                    state = 1;
                    temp = temp + str[i];
                 }
                else if (1 == state && _ChangeStr(str, changed_str, result_str, ref i, ref state, temp))
                {
                    //
                }
                else if ((1 == state && i > 0 && '\\' != str[i - 1] && '\"' == str[i]))
                {
                    state = 0;
                    temp = temp + '\"';
                }
                else
                {
                    temp = temp + str[i];
                }
            }

            return temp;
        }

    }
}
