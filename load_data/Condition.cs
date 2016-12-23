using System;
using System.Collections.Generic;

using ClauTextSharp.wiz;

namespace ClauTextSharp.load_data
{
    public class Condition
    {
        private int braceNum;
        private Vector<String> tokenVec;
        private ArrayStack<String> tokenStack;
        private String condition;
        private int i;
        private UserType position; // need set, get ter!!
        private UserType global;
        private int option;

        public Condition(String condition, UserType position, UserType global, int option = 0)
        {
            this.braceNum = 0;
            this.condition = condition;
            this.i = 0;
            this.position = position;
            this.option = option;
            this.tokenVec = new Vector<String>();
            this.tokenStack = new ArrayStack<String>();

            Init(condition);
        }

        private String GetType(String str) {
            if (Utility.IsInteger(str)) { return "INTEGER"; }
            else if (Utility.IsDouble(str)) { return "DOUBLE"; }
            else if (Utility.IsDate(str)) { return "DATE"; }
            else return "String";
        }
        private String Compare(String str1, String str2, int type = 0)
        {
            String type1 = GetType(str1);
            String type2 = GetType(str2);

            if (type1 != type2)
            {
                return "ERROR";
            }

            if ("String" == type1 || type == 1)
            {
                //if (str1 < str2)
                if (String.Compare(str1, str2) < 0)
                {
                    return "< 0";
                }
                else if (str1 == str2)
                {
                    return "== 0";
                }
                return "> 0";
            }
            else if ("INTEGER" == type1)
            {
                if (Utility.IsMinus(str1) && !Utility.IsMinus(str2)) { return "< 0"; }
                else if (!Utility.IsMinus(str1) && Utility.IsMinus(str2)) { return "> 0"; }

                bool minusComp = Utility.IsMinus(str1) && Utility.IsMinus(str2);

                if (false == minusComp)
                {
                    String x = StringUtility.reverse(str1);
                    String y = StringUtility.reverse(str2);

                    if (x[0] == '+') { x = StringUtility.subString(x, 1); }
                    if (y[0] == '+') { y = StringUtility.subString(y, 1); }

                    if (x.Length < y.Length)
                    {
                        for (int i = 0; i < y.Length - x.Length; ++i)
                        {
                            x = x + "0";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < x.Length - y.Length; ++i)
                        {
                            y = y + "0";
                        }
                    }
                    return Compare(StringUtility.reverse(x), StringUtility.reverse(y), 1);
                }
                else
                {
                    return Compare(StringUtility.subString(str2, 1), StringUtility.subString(str1, 1));
                }
            }
            else if ("DOUBLE" == type1)
            {
                StringTokenizer tokenizer1 = new StringTokenizer(str1, ".");
                StringTokenizer tokenizer2 = new StringTokenizer(str2, ".");

                String x = tokenizer1.nextToken();
                String y = tokenizer2.nextToken();

                String z = Compare(x, y);
                if ("= 0" == z)
                {
                    x = tokenizer1.nextToken();
                    y = tokenizer2.nextToken();

                    if (x.Length < y.Length)
                    {
                        for (int i = 0; i < y.Length - x.Length; ++i)
                        {
                            x = x + "0";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < x.Length - y.Length; ++i)
                        {
                            y = y + "0";
                        }
                    }
                    return Compare(x, y, 1);
                }
                else
                {
                    return z;
                }
            }
            else if ("DATE" == type1)
            {
                StringTokenizer tokenizer1 = new StringTokenizer(str1, ".");
                StringTokenizer tokenizer2 = new StringTokenizer(str2, ".");

                for (int i = 0; i < 3; ++i)
                {
                    String x = tokenizer1.nextToken();
                    String y = tokenizer2.nextToken();

                    String comp = Compare(x, y);

                    if (comp == "< 0") { return comp; }
                    else if (comp == "> 0") { return comp; }
                }
                return "== 0";
            }

            return "ERROR";
        }

        private Pair<bool, Vector<UserType>> Get(String var, String position, UserType utPosition, UserType global)
        {
            String valTemp = position;
            StringTokenizer tokenizer = new StringTokenizer(position, "/");
            UserType utTemp = new UserType("");

            if (false == tokenizer.hasMoreTokens())
            {
                utTemp = utPosition;
            }
            else
            {
                if (tokenizer.nextToken() != "root")
                {
                    utTemp = utPosition;
                }
                else
                {
                    valTemp = StringUtility.subString(position, 5);
                    utTemp = global;
                }
            }
            return UserType.Find(utTemp, valTemp);
        }
        private String GetValue(String op, String var, String val, UserType utPosition, UserType global, String option = "0")
        {
            /// ExistItem, ExistUserType
            if (null == utPosition) { return "ERROR"; }
            if ("EXISTITEM" == op)
            { /// option == 1?	
                var x = Get(var, val, utPosition, global);
                if (x.first)
                {
                    //if (x.second.size() > 1) { return "ERROR"; } ///
                    for (int i = 0; i < x.second.size(); ++i)
                    {
                        if (x.second.get(i).GetItem(var).size() > 0)
                        {
                            return "TRUE";
                        }
                    }
                    return "FALSE";
                }
                return "FALSE";
            }
            else if ("EXISTUSERTYPE" == op)
            { /// option == 1?	
                var x = Get(var, val, utPosition, global);
                if (x.first)
                {
                    //if (x.second.size() > 1) { return "ERROR"; } ///
                    for (int i = 0; i < x.second.size(); ++i)
                    {
                        if (x.second.get(i).GetUserTypeItem(var).size() > 0)
                        {
                            return "TRUE";
                        }
                    }
                    return "FALSE";
                }
                return "FALSE";
            }
            else if ("NOTEXISTITEM" == op)
            { /// option == 2 ?
                var x = Get(var, val, utPosition, global);
                if (x.first)
                {
                    //if (x.second.size() > 1) { return "ERROR"; } ///

                    for (int i = 0; i < x.second.size(); ++i)
                    {
                        if (0 < x.second.get(i).GetItem(var).size())
                        {
                            return "FALSE";
                        }
                    }
                    return "TRUE";
                }
                return "FALSE";
            }
            else if ("NOTEXISTUSERTYPE" == op)
            { /// option == 2 ?
                var x = Get(var, val, utPosition, global);
                if (x.first)
                {
                    //if (x.second.size() > 1) { return "ERROR"; } ///

                    for (int i = 0; i < x.second.size(); ++i)
                    {
                        if (0 < x.second.get(i).GetUserTypeItem(var).size())
                        {
                            return "FALSE";
                        }
                    }
                    return "TRUE";
                }
                return "FALSE";
            }

            return "ERROR";
        }
        private String GetValue(String op, String var1, String position1, String var2, String position2,
                    UserType utPosition, UserType global, String option = "0")
        {
            // COMP<, COMP>, EQ, NOTEQ
            if (null == utPosition) { return "ERROR"; }

            var x = Get(var1, position1, utPosition, global);
            var y = Get(var2, position2, utPosition, global);

            if ("~" != position1 && false == x.first)
            {
                return "ERROR GV1";
            }
            if ("~" != position2 && false == y.first)
            {
                return "ERROR GV2";
            }
            //
            if (((x.first && x.second.size() > 1) || (y.first && y.second.size() > 1)))
            {
                return "ERROR GV3";
            }

            Vector<ItemType<String>> value1 = new Vector<ItemType<String>>();  // Item<String> <- 
            Vector<ItemType<String>> value2 = new Vector<ItemType<String>>();

            // added..
            if (position1 != "~")
            {
                value1 = x.second.get(0).GetItem(var1);
            }
            if (position2 != "~")
            {
                value2 = y.second.get(0).GetItem(var2);
            }
            //
            if (position1 == "~")
            {
                value1.push_back(new ItemType<String>()); // var1
                value1.get(0).Push(var1);
            }
            if (position2 == "~")
            {
                value2.push_back(new ItemType<String>()); // var2
                value2.get(0).Push(var2);
            }

            //
            if (value1.size() == 0)
            {
                return "ERROR GV4";
            }
            if (value2.size() == 0)
            {
                return "ERROR GV5";
            }

            if (option == "0" && (value1.size() > 1 || value2.size() > 1))
            {
                return "ERROR GV6";
            }

            if ("COMP<" == op)
            {
                if ("0" != option)
                { // ToDo.. // 0. just 1-1, // 1. for any case // 2. for all case
                    for (int i = 0; i < value1.size(); ++i)
                    {
                        for (int j = 0; j < value2.size(); ++j)
                        {
                            if (Compare(value1.get(i).GetVal(), value2.get(j).GetVal()) == "< 0")
                            {
                                if ("1" == option) { return "TRUE"; }
                            }
                            else
                            {
                                if ("2" == option) { return "FALSE"; }
                            }
                        }
                    }
                    if ("1" == option) { return "FALSE"; }
                    else if ("2" == option)
                    {
                        return "TRUE";
                    }
                }
                else
                {
                    if (Compare(value1.get(0).GetVal(), value2.get(0).GetVal()) == "< 0")
                    {
                        return "TRUE";
                    }
                }
                return "FALSE";
            }
            else if ("COMP<EQ" == op)
            {
                if ("0" != option)
                { /// ToDo.. // 0. just 1-1, // 1. for any case // 2. for all case
                    for (int i = 0; i < value1.size(); ++i)
                    {
                        for (int j = 0; j < value2.size(); ++j)
                        {
                            String temp = Compare(value1.get(i).GetVal(), value2.get(j).GetVal());
                            if (temp == "< 0" || temp == "== 0")
                            {
                                if ("1" == option) { return "TRUE"; }
                            }
                            else
                            {
                                if ("2" == option) { return "FALSE"; }
                            }
                        }
                    }
                    if ("1" == option) { return "FALSE"; }
                    else if ("2" == option)
                    {
                        return "TRUE";
                    }
                }
                else
                {
                    String temp = Compare(value1.get(0).GetVal(), value2.get(0).GetVal());
                    if (temp == "< 0" || temp == "== 0")
                    {
                        return "TRUE";
                    }
                }
                return "FALSE";
            }
            else if ("COMP>" == op)
            {
                if ("0" != option)
                { /// ToDo.. // 0. just 1-1, // 1. for any case // 2. for all case
                    for (int i = 0; i < value1.size(); ++i)
                    {
                        for (int j = 0; j < value2.size(); ++j)
                        {
                            if (Compare(value1.get(i).GetVal(), value2.get(j).GetVal()) == "> 0")
                            {
                                if ("1" == option) { return "TRUE"; }
                            }
                            else
                            {
                                if ("2" == option) { return "FALSE"; }
                            }
                        }
                    }
                    if ("1" == option) { return "FALSE"; }
                    else if ("2" == option)
                    {
                        return "TRUE";
                    }
                }
                else
                {
                    if (Compare(value1.get(0).GetVal(), value2.get(0).GetVal()) == "> 0")
                    {
                        return "TRUE";
                    }
                }
                return "FALSE";
            }
            else if ("COMP>EQ" == op)
            {
                if ("0" != option)
                { /// ToDo.. // 0. just 1-1, // 1. for any case // 2. for all case
                    for (int i = 0; i < value1.size(); ++i)
                    {
                        for (int j = 0; j < value2.size(); ++j)
                        {
                            String temp = Compare(value1.get(i).GetVal(), value2.get(j).GetVal());
                            if (temp == "> 0" || temp == "== 0")
                            {
                                if ("1" == option) { return "TRUE"; }
                            }
                            else
                            {
                                if ("2" == option) { return "FALSE"; }
                            }
                        }
                    }
                    if ("1" == option) { return "FALSE"; }
                    else if ("2" == option)
                    {
                        return "TRUE";
                    }
                }
                else
                {
                    String temp = Compare(value1.get(0).GetVal(), value2.get(0).GetVal());
                    if (temp == "> 0" || temp == "== 0")
                    {
                        return "TRUE";
                    }
                }
                return "FALSE";
            }
            else if ("EQ" == op)
            {
                if ("0" != option)
                { /// ToDo.. // 0. just 1-1, // 1. for any case // 2. for all case
                    for (int i = 0; i < value1.size(); ++i)
                    {
                        for (int j = 0; j < value2.size(); ++j)
                        {
                            if (Compare(value1.get(i).GetVal(), value2.get(j).GetVal()) == "== 0")
                            {
                                if ("1" == option) { return "TRUE"; }
                            }
                            else
                            {
                                if ("2" == option) { return "FALSE"; }
                            }
                        }
                    }
                    if ("1" == option) { return "FALSE"; }
                    else if ("2" == option)
                    {
                        return "TRUE";
                    }
                }
                else
                {
                    if (Compare(value1.get(0).GetVal(), value2.get(0).GetVal()) == "== 0")
                    {
                        return "TRUE";
                    }
                }
                return "FALSE";
            }
            else if ("NOTEQ" == op)
            {
                if ("0" != option)
                { /// ToDo.. // 0. just 1-1, // 1. for any case // 2. for all case
                    for (int i = 0; i < value1.size(); ++i)
                    {
                        for (int j = 0; j < value2.size(); ++j)
                        {
                            if (Compare(value1.get(i).GetVal(), value2.get(j).GetVal()) != "== 0")
                            {
                                if ("1" == option) { return "TRUE"; }
                            }
                            else
                            {
                                if ("2" == option) { return "FALSE"; }
                            }
                        }
                    }
                    if ("1" == option) { return "FALSE"; }
                    else if ("2" == option)
                    {
                        return "TRUE";
                    }
                }
                else
                {
                    if (Compare(value1.get(0).GetVal(), value2.get(0).GetVal()) != "== 0")
                    {
                        return "TRUE";
                    }
                }
                return "FALSE";
            }

            return "ERROR GV7";
        }
        private String BoolOperation(String op, String x, String y)
        {
            if (x == "ERROR" || y == "ERROR") { return "ERROR"; }
            if ("NOT" == op) { return x == "TRUE" ? "FALSE" : "TRUE"; }
            else if ("AND" == op)
            {
                if (x == "TRUE" && y == "TRUE") { return "TRUE"; }
                else
                {
                    return "FALSE";
                }
            }
            else if ("OR" == op)
            {
                if (x == "TRUE" || y == "TRUE") { return "TRUE"; }
                else
                {
                    return "FALSE";
                }
            }
            else
            {
                return "ERROR";
            }
        }

        private void Init(String condition)
        {
            String str = Utility.AddSpace(condition);

            StringTokenizer tokenizer = new StringTokenizer(str, Utility.beforeWhitespaceVec);

            while (tokenizer.hasMoreTokens())
            {
                String temp = tokenizer.nextToken();

                if (temp == "^")
                { // 
                    temp = "";
                }
                tokenVec.push_back(temp);
            }
        }
        public bool Next()
        {
            try
            {
                if (i >= tokenVec.size()) { return false; }
                if (condition.Length == 0)
                {
                    return false;
                }
                {
                    if ("=" != tokenVec.get(i)
                        && "{" != tokenVec.get(i)
                        && "}" != tokenVec.get(i))
                    {
                        tokenStack.push(tokenVec.get(i));
                    }

                    if ("{" == tokenVec.get(i))
                    {
                        braceNum++;
                    }
                    else if ("}" == tokenVec.get(i))
                    {
                        braceNum--;

                        // COMP<, COMP<EQ, COMP>, COMP>EQ, EQ, NOTEQ
                        if (tokenStack.size() >= 6 && ("COMP<" == tokenStack.get(tokenStack.size() - 6)
                            || "COMP<EQ" == tokenStack.get(tokenStack.size() - 6)
                            || "COMP>" == tokenStack.get(tokenStack.size() - 6)
                            || "COMP>EQ" == tokenStack.get(tokenStack.size() - 6)
                            || "EQ" == tokenStack.get(tokenStack.size() - 6)
                            || "NOTEQ" == tokenStack.get(tokenStack.size() - 6)))
                        {
                            String op = tokenStack.get(tokenStack.size() - 6);
                            String var1 = tokenStack.get(tokenStack.size() - 5);
                            String position1 = tokenStack.get(tokenStack.size() - 4);
                            String var2 = tokenStack.get(tokenStack.size() - 3);
                            String position2 = tokenStack.get(tokenStack.size() - 2);
                            String option = tokenStack.get(tokenStack.size() - 1);
                            tokenStack.pop();
                            tokenStack.pop();
                            tokenStack.pop();
                            tokenStack.pop();
                            tokenStack.pop();
                            tokenStack.pop();

                            tokenStack.push(GetValue(op, var1, position1, var2, position2, position, global, option));
                        }
                        else if (tokenStack.size() >= 3 && "NOT" != tokenStack.get(tokenStack.size() - 2))
                        {
                            String op = tokenStack.get(tokenStack.size() - 3);
                            String operand1 = tokenStack.get(tokenStack.size() - 2);
                            String operand2 = tokenStack.get(tokenStack.size() - 1);

                            if (op == "AND" || op == "OR")
                            {
                                tokenStack.pop();
                                tokenStack.pop();
                                tokenStack.pop();

                                tokenStack.push(BoolOperation(op, operand1, operand2));
                            }
                            else
                            { // EXIST, NOTEXIST for ( ITEM or USERTYPE ) 
                                tokenStack.pop();
                                tokenStack.pop();
                                tokenStack.pop();

                                tokenStack.push(GetValue(op, operand1, operand2, position, global));
                            }
                        }
                        else if (tokenStack.size() >= 2 && "NOT" == tokenStack.get(tokenStack.size() - 2))
                        {
                            String op = tokenStack.get(tokenStack.size() - 2);
                            String operand1 = tokenStack.get(tokenStack.size() - 1);

                            tokenStack.pop();
                            tokenStack.pop();
                            tokenStack.push(BoolOperation(op, operand1, operand1));
                        }
                    }
                    ++i;
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); return false; }
            return true;
        }
        public int NowIndex() { return i; }
        public ArrayStack<String> Now() { return tokenStack; }

        public bool IsNowImportant() { // ??
            return "}" == tokenVec.get(i) && "NOT" != tokenVec.get(tokenStack.size() - 2);
        }
    }
}
