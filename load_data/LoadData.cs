using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using ClauTextSharp.wiz;

namespace ClauTextSharp.load_data
{
    class LoadData
    {

        // move to where?
        private static long min(long a, long b)
        {
            if (a < b) { return a; }
            return b;
        }
        private static long max(long a, long b)
        {
            if( a > b ) { return a; }
            return b;
        }

        private readonly static String LEFT = "{";
        private readonly static String RIGHT = "}";
        private readonly static String EQ = "=";

        private readonly static int RIGHT_DO = 1;



        /// core
        public static bool _LoadData(ArrayQueue<String> strVec, Reserver vecReserver, ref UserType global) // first, strVec.empty() must be true!!
        {
            int state = 0;
            int braceNum = 0;
            ArrayStack<int> state_reserve = new ArrayStack<int>();
            ArrayStack<int> do_reserve = new ArrayStack<int>();
            Vector<UserType> nestedUT = new Vector<UserType>(1);
            String var1 = "", var2 = "", val = "";

            nestedUT.set(0, global);
            {
                vecReserver.functor(strVec);

                while (strVec.empty())
                {
                    vecReserver.functor(strVec);
                    if (
                        strVec.empty() &&
                        vecReserver.end()
                        )
                    {
                        return false; // throw "Err nextToken does not exist"; // cf) or empty file or empty String!
                    }
                }
            }

            while (false == strVec.empty())
            {
                switch (state)
                {
                    case 0:
                        if (LEFT == Utility.Top(strVec))
                        {
                            //Utility.Pop(strVec);
                            state = 2;
                        }
                        else
                        {
                            Pair<bool, String> bsPair = Utility.LookUp(strVec, 1);
                            if (bsPair.first)
                            {
                                if (EQ == bsPair.second)
                                {
                                    var2 = Utility.Pop(strVec);
                                    Utility.Pop(strVec);
                                    state = 2;
                                }
                                else
                                {
                                    var1 = Utility.Pop(strVec);
                                    nestedUT.get(braceNum).AddItem("", (String)var1);
                                    state = 0;
                                }
                            }
                            else
                            {
                                var1 = Utility.Pop(strVec);
                                nestedUT.get(braceNum).AddItem("", (String)var1);
                                state = 0;
                            }
                        }
                        break;
                    case 1:
                        if (RIGHT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);
                            state = 0;
                        }
                        else
                        {
                            // syntax error.
                            throw new Exception("syntax error 1 ");
                        }
                        break;
                    case 2:
                        if (LEFT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);

                            ///
                            nestedUT.get(braceNum).AddUserTypeItem(new UserType((String)var2));
                            UserType pTemp = new UserType();
                            nestedUT.get(braceNum).GetLastUserTypeItemRef((String)var2, ref pTemp);

                            braceNum++;

                            /// new nestedUT
                            if (nestedUT.size() == braceNum) /// changed 2014.01.23..
                                nestedUT.push_back(null);

                            /// initial new nestedUT.
                            nestedUT.set(braceNum, pTemp);
                            ///
                            state = 3;
                        }
                        else
                        {
                            val = Utility.Pop(strVec);

                            nestedUT.get(braceNum).AddItem((String)var2, (String)val);
                            var2 = "";
                            val = "";

                            state = 0;
                        }
                        break;
                    case 3:
                        if (RIGHT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);

                            nestedUT.set(braceNum, null);
                            braceNum--;

                            state = 0;
                        }
                        else
                        {
                            {
                                /// uisng struct
                                state_reserve.push(0);
                                do_reserve.push(RIGHT_DO);
                                state = 4;
                            }
                            //else
                            {
                                //	throw  "syntax error 2 ";
                            }
                        }
                        break;
                    case 4:
                        if (LEFT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);

                            UserType temp = new UserType();

                            nestedUT.get(braceNum).AddUserTypeItem(temp);
                            UserType pTemp = new UserType();
                            nestedUT.get(braceNum).GetLastUserTypeItemRef("", ref pTemp);

                            braceNum++;

                            /// new nestedUT
                            if (nestedUT.size() == braceNum) /// changed 2014.01.23..
                                nestedUT.push_back(null);

                            /// initial new nestedUT.
                            nestedUT.set(braceNum, pTemp);
                            ///
                            //}

                            state = 5;
                        }
                        else if (RIGHT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);
                            state = state_reserve.top();
                            state_reserve.pop();
                            int do_id = do_reserve.top();
                            do_reserve.pop();
                            //if (do_id == RIGHT_DO)
                            {
                                nestedUT.set(braceNum, null);
                                braceNum--;
                            }
                        }
                        else
                        {
                            Pair<bool, String> bsPair = Utility.LookUp(strVec, 1);
                            if (bsPair.first)
                            {
                                if (EQ == bsPair.second)
                                {
                                    // var2
                                    var2 = Utility.Pop(strVec);
                                    Utility.Pop(strVec); // pass EQ
                                    
                                    state = 6;
                                }
                                else
                                {
                                    // var1
                                    var1 = Utility.Pop(strVec);
                                    nestedUT.get(braceNum).AddItem("", (String)var1);
                                    var1 = "";

                                    state = 4;
                                }
                            }
                            else
                            {
                                // var1
                                var1 = Utility.Pop(strVec);
                                nestedUT.get(braceNum).AddItem("", (String)var1);
                                var1 = "";

                                state = 4;
                            }
                        }
                        break;
                    case 5:
                        if (RIGHT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);

                            //if (flag1 == 0) {
                            nestedUT.set(braceNum, null);
                            braceNum--;
                            // }
                            //
                            state = 4;
                        }

                        else
                        {
                            {
                                /// uisng struct
                                state_reserve.push(4);

                                {
                                    do_reserve.push(RIGHT_DO);
                                }
                                state = 4;
                            }
                            //else
                            {
                                //	throw "syntax error 4  ";
                            }
                        }
                        break;
                    case 6:
                        if (LEFT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);

                            ///
                            {
                                nestedUT.get(braceNum).AddUserTypeItem(new UserType((String)var2));
                                UserType pTemp = new UserType();
                                nestedUT.get(braceNum).GetLastUserTypeItemRef((String)var2, ref pTemp);
                                var2 = "";
                                braceNum++;

                                /// new nestedUT
                                if (nestedUT.size() == braceNum) /// changed 2014.01.23..
                                    nestedUT.push_back(null);

                                /// initial new nestedUT.
                                nestedUT.set(braceNum, pTemp);
                            }
                            ///
                            state = 7;
                        }
                        else
                        {
                            val = Utility.Pop(strVec);

                            nestedUT.get(braceNum).AddItem((String)var2, (String)val);
                            var2 = ""; val = "";
                            if (strVec.empty())
                            {
                                //
                            }
                            else if (RIGHT == Utility.Top(strVec))
                            {
                                Utility.Pop(strVec);

                                {
                                    state = state_reserve.top();
                                    state_reserve.pop();

                                    int do_id = do_reserve.top();
                                    do_reserve.pop();
                                    //if (do_id == RIGHT_DO)
                                    {
                                        nestedUT.set(braceNum, null);
                                        braceNum--;
                                    }
                                }
                                {
                                    //state = 4;
                                }
                            }
                            else
                            {
                                state = 4;
                            }
                        }
                        break;
                    case 7:
                        if (RIGHT == Utility.Top(strVec))
                        {
                            Utility.Pop(strVec);
                            //

                            nestedUT.set(braceNum, null);
                            braceNum--;
                            //
                            state = 4;
                        }
                        else
                        {
                            {
                                /// uisng struct
                                state_reserve.push(4);

                                {
                                    do_reserve.push(RIGHT_DO);
                                }
                                state = 4;
                            }
                            //else
                            {
                                //throw "syntax error 5 ";
                            }
                        }
                        break;
                    default:
                        // syntax err!!

                        throw new Exception("syntax error 6 ");
                        break;
                }

                if (strVec.size() < 10)
                {
                    vecReserver.functor(strVec);

                    while (strVec.empty()) // (strVec.empty())
                    {
                        vecReserver.functor(strVec);
                        if (
                            strVec.empty() &&
                            vecReserver.end()
                            )
                        {
                            // throw "Err nextToken does not exist2";
                            break;
                        }
                    }
                }
            }
            if (state != 0)
            {
                Console.WriteLine("strVec.size() " + strVec.size());
                throw new Exception("error final state is not 0! : state - " + state);
            }
            if (braceNum != 0)
            {
                throw new Exception(("chk braceNum is ") + braceNum.ToString());
            }

            return true;
        }

        public static bool LoadDataFromFile(String fileName, ref UserType global) /// global should be empty
		{
            UserType globalTemp = new UserType(global);
            FileStream inFile = null;
            try
            {
                inFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(inFile);
            
                ArrayQueue<String> strVec = new ArrayQueue<String>();

         
                InFileReserver ifReserver = new InFileReserver(sr);

                ifReserver.SetNum(100000);
                // cf) empty file..
                if (false == _LoadData(strVec, (Reserver)ifReserver, ref globalTemp))
                {
                    return true;
                }

                UserType.ReplaceAll(globalTemp, Utility.reverse_specialStr2, Utility.reverse_specialStr);
                sr.Close();
                inFile.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); if (null != inFile) { inFile.Close(); } return false; }

            global = globalTemp;
            return true;
        }

        public static bool LoadDataFromString(String str, ref UserType ut)
        {
            UserType utTemp = new UserType(ut);

            bool chk = Utility.ChkExist(str);
            if (chk)
            {
                str = Utility.ChangeStr(str, Utility.specialStr[0], Utility.specialStr2[0]);//{ "^" }, { "^0" });
                str = Utility.ChangeStr(str, Utility.specialStr[5], Utility.specialStr2[5]);//{ "#" }, { "^5" });
            }
            str = Utility.PassSharp(str);
            str = Utility.AddSpace(str);
            if (chk)
            {
                str = Utility.ChangeStr(str, Utility.beforeWhitespaceVec, Utility.afterWhitespaceVec);
            }
            StringTokenizer tokenizer = new StringTokenizer(str, Utility.afterWhitespaceVec);
            ArrayQueue<String> strVec = new ArrayQueue<String>();

            while (tokenizer.hasMoreTokens())
            {
                strVec.push(tokenizer.nextToken());
            }
            try
            {
                // empty String!
                NoneReserver nonReserver = new NoneReserver();
                if (false == _LoadData(strVec, (Reserver)nonReserver, ref utTemp))
                {
                    return true;
                }

                if (chk)
                {
                    UserType.ReplaceAll(utTemp, Utility.reverse_specialStr2, Utility.reverse_specialStr);
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); return false; }

            ut = utTemp;
            return true;
        }

        private UserType global; // ToDo - remove!

        public LoadData() { InitWizDB(); }

        /// need testing!
        public LoadData(LoadData ld)
        {
            global = ld.global;
        }

        public bool InitWizDB()
        {
            return InitWizDB(global);
        }
        // allRemove Query 
        public bool AllRemoveWizDB()
        {
            return AllRemoveWizDB(global);
        }
        // AddQuery AddData, AddUserTypeData
        public bool AddData(String position, String data, String condition = "")
        {
            return AddData(global, position, data, condition);
        }
        // 
        public bool AddNoNameUserType(String position, String data, String condition = "")
        {
            return AddNoNameUserType(global, position, data, condition);
        }
        // SetQuery
        public bool SetData(String position, String varName, String data, String condition = "")
        {
            return SetData(global, position, varName, data, condition);
        }
        /// 
        public String GetData(String position, String condition)
        {
            return GetData(global, position, condition);
        }
        public String GetItemListData(String position, String condition)
        {
            return GetItemListData(global, position, condition);
        }
        public String GetItemListNamesData(String position, String condition)
        {
            return GetItemListNamesData(global, position, condition);
        }
        public String GetUserTypeListNamesData(String position, String condition)
        {
            return GetUserTypeListNamesData(global, position, condition);
        }
        /// varName = val - do
        /// varName = { val val val } - GetData(position+"/varName", ""); 
        /// varName = { var = val } - GetData(position+"/varname", var);
        public String GetData(String position, String varName, String condition) // 
        {
            return GetData(global, position, varName, condition);
        }
        public bool Remove(String position, String var, String condition)
        {
            return Remove(global, position, var, condition);
        }

        public bool LoadWizDB(String fileName)
        {
            return LoadWizDB(global, fileName);
        }
        // SaveQuery
        public bool SaveWizDB(String fileName, String option = "0")
        { /// , int option
            return SaveWizDB(global, fileName, option);
        }

        /// To Do - ExistItem, ExistUserType, SetUserType GetUserType
        public bool ExistData(String position, String varName, String condition) // 
        {
            return ExistData(global, position, varName, condition);
        }
        /// ToDo - recursive function
        public String SearchItem(String var, String condition)
        {
            return SearchItem(global, var, condition);
        }
        public String SearchUserType(String var, String condition)
        {
            return SearchUserType(global, var, condition);
        }
        private void SearchItem(Vector<String> positionVec, String var, String nowPosition,
                UserType ut, String condition)
        {
            SearchItem(global, positionVec, var, nowPosition, ut, condition);
        }
        private void SearchUserType(Vector<String> positionVec, String var, String nowPosition,
            UserType ut, String condition)
        {
            SearchUserType(global, positionVec, var, nowPosition, ut, condition);
        }
        private static void SearchItem(UserType global, Vector<String> positionVec, String var, String nowPosition,
                        UserType ut, String condition)
        {
            String _var = var;
            if (_var == " ") { _var = ""; }
            if (ut.GetItem(_var).size() > 0)
            {
                String _condition = condition;

                if (_var == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                else
                    _condition = StringUtility.replace(_condition, "~~", _var); //
                Condition cond = new Condition(_condition, ut, global);

                while (cond.Next()) ;

                if (cond.Now().size() == 1 && "TRUE" == cond.Now().get(0))
                {
                    positionVec.push_back(nowPosition);
                }
            }

            for (int i = 0; i < ut.GetUserTypeListSize(); ++i)
            {
                String temp = ut.GetUserTypeList(i).GetName();
                if (temp == "") { temp = " "; }
                SearchItem(
                    global,
                    positionVec,
                    _var,
                    nowPosition + "/" + temp,
                    ut.GetUserTypeList(i),
                    condition
                );
            }
        }

        private static void SearchUserType(UserType global, Vector<String> positionVec, String var, String nowPosition,
        UserType ut, String condition)
        {
            String _var = var;
            if (_var == " ")
            {
                _var = "";
            }
            if (ut.GetUserTypeItem(_var).size() > 0)
            {
                String _condition = condition;

                if (_var == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                else _condition = StringUtility.replace(_condition, "~~", _var); //

                Condition cond = new Condition(_condition, ut, global);

                while (cond.Next()) ;

                if (cond.Now().size() == 1 && "TRUE" == cond.Now().get(0))
                {
                    positionVec.push_back(nowPosition);
                }
            }

            for (int i = 0; i < ut.GetUserTypeListSize(); ++i)
            {
                String temp = ut.GetUserTypeList(i).GetName();

                if (temp == "") { temp = " "; }
                SearchUserType(
                    global,
                    positionVec,
                    _var,
                    nowPosition + "/" + temp,
                    ut.GetUserTypeList(i),
                    condition
                );
            }
        }

        public static bool InitWizDB(UserType global)
        {
            global = new UserType("global");
            return true;
        }
        // allRemove Query 
        public static bool AllRemoveWizDB(UserType global)
        {
            global = new UserType("");
            return true;
        }
        // AddQuery AddData, AddUserTypeData
        public static bool AddData(UserType global, String position, String data, String condition = "")
        {
            UserType utTemp = new UserType("global");
            bool isTrue = false;

            if (false == LoadDataFromString(data, ref utTemp))
            {
                return false;
            }
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    int item_n = 0;
                    int user_n = 0;

                    /// chk temp test codes - > using flag 1.Exist 2.Comparision
                    //if (finded.second.get(i).GetItem("base_tax").GetCount() > 0) { continue; }
                    ///~end
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0)) // || cond.Now().size()  != 1
                        {
                            //std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }

                    for (int k = 0; k < utTemp.GetIListSize(); ++k)
                    {
                        if (utTemp.IsItemType(k))
                        {
                            finded.second.get(i).AddItemType(utTemp.GetItemList(item_n));
                            item_n++;
                        }
                        else
                        {
                            finded.second.get(i).AddUserTypeItem(utTemp.GetUserTypeList(user_n));
                            user_n++;
                        }
                    }
                    isTrue = true;
                }
                return isTrue;
            }
            else {
                return false;
            }
        }
        public static bool AddDataAtFront(UserType global, String position, String data, String condition = "")
        {
            UserType utTemp = new UserType("global");
            bool isTrue = false;

            if (false == LoadDataFromString(data, ref utTemp))
            {
                return false;
            }
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    int item_n = 0;
                    int user_n = 0;

                    /// chk temp test codes - > using flag 1.Exist 2.Comparision
                    //if (finded.second.get(i).GetItem("base_tax").GetCount() > 0) { continue; }
                    ///~end
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0)) // || cond.Now().size()  != 1
                        {
                            //std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }

                    for (int k = 0; k < utTemp.GetIListSize(); ++k)
                    {
                        if (utTemp.IsItemType(k))
                        {
                            finded.second.get(i).AddItemAtFront(utTemp.GetItemList(item_n).GetName(), utTemp.GetItemList(item_n).GetVal());
                            item_n++;
                        }
                        else
                        {
                            finded.second.get(i).AddUserTypeItemAtFront(utTemp.GetUserTypeList(user_n));
                            user_n++;
                        }
                    }
                    isTrue = true;
                }
                return isTrue;
            }
            else {
                return false;
            }
        }
        public static bool AddNoNameUserType(UserType global, String position, String data, String condition = "")
        {
            UserType utTemp = new UserType("");
            bool isTrue = false;

            if (false == LoadDataFromString(data, ref utTemp))
            {
                return false;
            }
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    int item_n = 0;
                    int user_n = 0;

                    /// chk temp test codes - > using flag 1.Exist 2.Comparision
                    //if (finded.second.get(i).GetItem("base_tax").GetCount() > 0) { continue; }
                    ///~end
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    finded.second.get(i).AddUserTypeItem(utTemp);

                    isTrue = true;
                }
                return isTrue;
            }
            else {
                return false;
            }
        }

        public static bool AddUserType(UserType global, String position, String var, String data, String condition = "")
        {
            bool isTrue = false;
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                String temp = var;
                if (temp == "") { temp = " "; }
                StringTokenizer tokenizer = new StringTokenizer(temp, "/");
                UserType utTemp = new UserType("");
                if (false == LoadDataFromString(data, ref utTemp))
                {
                    return false;
                }

                while (tokenizer.hasMoreTokens())
                {
                    String utName = tokenizer.nextToken();
                    Vector<String> strVec = new Vector<String>();
                    if (utName == " ") { utName = ""; }

                    if (utName.Length >= 3 && utName[0] == '[' && utName[utName.Length - 1] == ']')
                    {
                        StringTokenizer tokenizer2 = new StringTokenizer(utName, new Vector<String>(new List<String>{ "[", "~", "]" }));
                        while (tokenizer2.hasMoreTokens())
                        {
                            strVec.push_back(tokenizer2.nextToken());
                        }
                    }

                    long a = 0, b = 0, Min = 0, Max = 0;
                    bool chkInt = false;

                    if (strVec.size() == 2 && Utility.IsInteger(strVec.get(0)) && Utility.IsInteger(strVec.get(1)))
                    {
                        chkInt = true;
                        if (false == long.TryParse(strVec.get(0), out a))
                        {
                            throw new Exception("AddUserType, TryParse fail..");
                        }
                        if (false == long.TryParse(strVec.get(1), out b))
                        {
                            throw new Exception("AddUserType, TryParse fail..");
                        }
                        Min = min(a, b);
                        Max = max(a, b);
                    }

                    for (var x = Min; x <= Max; ++x)
                    {
                        if (strVec.size() == 2 && chkInt)
                        {
                            utName = x.ToString();
                        }
                        else { }
                        utTemp.SetName(utName);

                        for (int i = 0; i < finded.second.size(); ++i)
                        {
                            int item_n = 0;
                            int user_n = 0;


                            if (condition.Length != 0)
                            {
                                String _condition = condition;

                                if (utName == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                                else
                                    _condition = StringUtility.replace(_condition, "~~", utName); //

                                //	cout << "condition is " << _condition << endl;

                                Condition cond = new Condition(_condition, finded.second.get(i), global);

                                while (cond.Next()) ;

                                if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                                {
                                    //std.cout << cond.Now().get(0) << endl;
                                    continue;
                                }
                            }

                            finded.second.get(i).AddUserTypeItem(utTemp);

                            isTrue = true; // chk!!
                        }

                        // prevent from infinity loop.
                        if (x == Max) { break; }
                    }

                }
                return isTrue;
            }
            else {
                return false;
            }
        }
        /// SetData - Re Do!
        public static bool SetData(UserType global, String position, String varName, String data, String condition = "")
        {
            var finded = UserType.Find(global, position);
            bool isTrue = false;

            if (finded.first)
            {
                String temp = varName;
                if (temp == "") { temp = " "; }
                StringTokenizer tokenizer = new StringTokenizer(temp, "/");
                UserType utTemp = new UserType("");
                if (false == LoadDataFromString(data, ref utTemp))
                {
                    return false;
                }
                while (tokenizer.hasMoreTokens())
                {
                    String _varName = tokenizer.nextToken();
                    /// todo - if varName is "" then data : val val val ... 
                    if (_varName == "" || _varName == " ")
                    { // re?
                        int n = utTemp.GetItem("").size();
                        for (int i = 0; i < finded.second.size(); ++i)
                        {
                            if (condition.Length != 0)
                            {
                                String _condition = condition;
                                if (_varName == "" || _varName == " ") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                                else
                                    _condition = StringUtility.replace(_condition, "~~", _varName); //

                                Condition cond = new Condition(_condition, finded.second.get(i), global);

                                while (cond.Next()) ;

                                if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                                {
                                    //	std.cout << cond.Now().get(0) << endl;
                                    continue;
                                }
                            }
                            finded.second.get(i).RemoveItemList("");

                            for (int j = 0; j < n; ++j)
                            {
                                finded.second.get(i).AddItem("", utTemp.GetItem("").get(j).GetVal());
                            }
                            isTrue = true;
                        }
                    }
                    else
                    {
                        Vector<String> strVec = new Vector<String>();

                        if (_varName.Length >= 3 && _varName[0] == '[' && _varName[_varName.Length - 1] == ']')
                        {
                            StringTokenizer tokenizer2 = new StringTokenizer(_varName, new Vector<String>(new List<String> { "[", "~", "]" }));
                            while (tokenizer2.hasMoreTokens())
                            {
                                strVec.push_back(tokenizer2.nextToken());
                            }
                        }

                        long a = 0, b = 0, Min = 0, Max = 0;
                        bool chkInt = false;

                        if (strVec.size() == 2 && Utility.IsInteger(strVec.get(0)) && Utility.IsInteger(strVec.get(1)))
                        {
                            chkInt = true;
                            if (false == long.TryParse(strVec.get(0), out a))
                            {
                                throw new Exception("AddUserType, TryParse fail..");
                            }
                            if (false == long.TryParse(strVec.get(1), out b))
                            {
                                throw new Exception("AddUserType, TryParse fail..");
                            }
                            Min = min(a, b);
                            Max = max(a, b);
                        }
                        for (long x = Min; x <= Max; ++x)
                        {
                            if (strVec.size() == 2 && chkInt)
                            {
                                _varName = x.ToString();
                            }
                            else { }

                            for (int i = 0; i < finded.second.size(); ++i)
                            {
                                if (condition.Length != 0)
                                {
                                    String _condition = condition;
                                    if (_varName == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                                    else
                                        _condition = StringUtility.replace(_condition, "~~", _varName); //

                                    Condition cond = new Condition(_condition, finded.second.get(i), global);

                                    while (cond.Next()) ;

                                    if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                                    {
                                        //	std.cout << cond.Now().get(0) << endl;
                                        continue;
                                    }
                                }
                                finded.second.get(i).SetItem(_varName, data); /// chk
                                isTrue = true;
                            }

                            // prevent from infinity loop.
                            if (x == Max) { break; }
                        }
                    }
                }
                return isTrue;
            }
            else {
                return false;
            }
        }

        public static bool SetData(UserType global, String position, int var_idx, String data, String condition = "")
        {
            var finded = UserType.Find(global, position);
            bool isTrue = false;

            if (finded.first)
            {
                UserType utTemp = new UserType("");
                if (false == LoadDataFromString(data, ref utTemp))
                {
                    return false;
                }
                long a = 0, b = 0, Min = 0, Max = 0;

                Min = min(a, b);
                Max = max(a, b);

                for (long x = Min; x <= Max; ++x)
                {
                    for (int i = 0; i < finded.second.size(); ++i)
                    {
                        if (condition.Length != 0)
                        {
                            String _condition = condition;

                            Condition cond = new Condition(_condition, finded.second.get(i), global);

                            while (cond.Next()) ;

                            if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                            {
                                //	std.cout << cond.Now().get(0) << endl;
                                continue;
                            }
                        }
                        finded.second.get(i).SetItem(var_idx, data); /// chk
                        isTrue = true;
                    }

                    // prevent from infinity loop.
                    if (x == Max) { break; }
                }
                return isTrue;
            }
            else {
                return false;
            }
        }
        /// 
        public static String GetData(UserType global, String position, String condition)
        {
            String str = "";
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    str = str + finded.second.get(i).ToString() + "\n";
                }
                return str;
            }
            else {
                return "";
            }
        }
        public static String GetItemListData(UserType global, String position, String condition)
        {
            String str="";
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    str = str + finded.second.get(i).ItemListToString() + "\n";
                }
                return str;
            }
            else {
                return "";
            }
        }
        public static String GetItemListNamesData(UserType global, String position, String condition)
        {
            String str = "";
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    str = str + finded.second.get(i).ItemListNamesToString() + "\n";
                }
                return str;
            }
            else {
                return "";
            }
        }
        public static String GetUserTypeListNamesData(UserType global, String position, String condition)
        {
            String str = "";
            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    str = str + finded.second.get(i).UserTypeListNamesToString() + "\n";
                }
                return str;
            }
            else {
                return "";
            }
        }
        /// varName = val - do
        /// varName = { val val val } - GetData(position+"/varName", ""); 
        /// varName = { var = val } - GetData(position+"/varname", var);
        public static String GetData(UserType global, String position, String varName, String condition) // 
        {
            String str = "";
            String _var = varName;
            if (_var == " ") { _var = ""; }

            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        String _condition = condition;

                        // ~~ and ^ . do not used other mean?
                        if (_var == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                        else
                            _condition = StringUtility.replace(_condition, "~~", _var); /// varName . _var.

                        Condition cond = new Condition(_condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }

                    int num = finded.second.get(i).GetItem(_var).size();
                    for (int k = 0; k < num; ++k)
                    {
                        str = str + finded.second.get(i).GetItem(_var).get(k).GetVal() + "\n";
                    }
                }
            }
            return str;
        }

        public static bool Remove(UserType global, String position, String var, String condition)
        {
            var finded = UserType.Find(global, position);
            bool isTrue = false;

            if (finded.first)
            {
                String temp = var;
                if (temp == "") { temp = " "; }
                StringTokenizer tokenizer = new StringTokenizer(temp, "/");
                while (tokenizer.hasMoreTokens())
                {
                    String _var = tokenizer.nextToken();
                    if (_var == " ") { _var = ""; }
                    Vector<String> strVec = new Vector<string>();

                    if (_var.Length >= 3 && _var[0] == '[' && _var[_var.Length - 1] == ']')
                    {
                        StringTokenizer tokenizer2 = new StringTokenizer(_var, new Vector<String>(new List<String>{ "[", "~", "]" }));
                        while (tokenizer2.hasMoreTokens())
                        {
                            strVec.push_back(tokenizer2.nextToken());
                        }
                    }

                    long a = 0, b = 0, Min = 0, Max = 0;
                    bool chkInt = false;

                    if (strVec.size() == 2 && Utility.IsInteger(strVec.get(0)) && Utility.IsInteger(strVec.get(1)))
                    {
                        chkInt = true;
                        if (false == long.TryParse(strVec.get(0), out a))
                        {
                            throw new Exception("AddUserType, TryParse fail..");
                        }
                        if (false == long.TryParse(strVec.get(1), out b))
                        {
                            throw new Exception("AddUserType, TryParse fail..");
                        }
                        Min = min(a, b);
                        Max = max(a, b);
                    }
                    for (long x = Min; x <= Max; ++x)
                    {
                        if (strVec.size() == 2 && chkInt)
                        {
                            _var = x.ToString();
                        }
                        else { }

                        for (int i = 0; i < finded.second.size(); ++i)
                        {
                            UserType utTemp = finded.second.get(i);

                            if (condition.Length != 0)
                            {
                                String _condition = condition;
                                if (_var == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                                else
                                    _condition = StringUtility.replace(_condition, "~~", _var); //

                                Condition cond = new Condition(_condition, finded.second.get(i), global);
                                while (cond.Next()) ;

                                if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                                {
                                    // std.cout << cond.Now().get(0) << endl;
                                    continue;
                                }
                            }

                            utTemp.RemoveItemList(_var);
                            utTemp.RemoveUserTypeList(_var);
                            isTrue = true;
                        }

                        // prevent from infinity loop.
                        if (x == Max) { break; }
                    }
                }
                return isTrue;
            }
            else {
                return false;
            }
        }
        public static bool Remove(UserType global, String position, String condition)
        {
            var finded = UserType.Find(global, position);
            bool isTrue = false;

            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    UserType temp = finded.second.get(i);

                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            // std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }

                    temp.Remove();
                    isTrue = true;
                }
                return isTrue;
            }
            else {
                return false;
            }
        }
        // todo - static bool Remove(UserType global,  String positiion, oonst int idx,  String condition)
        public static bool Remove(UserType global, String position, int idx, String condition)
        {
            var finded = UserType.Find(global, position);
            bool isTrue = false;

            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    UserType temp = finded.second.get(i);

                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            // std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }

                    temp.RemoveList(idx);
                    isTrue = true;
                }
                return isTrue;
            }
            else {
                return false;
            }
        }
        // cf) idx == -1 . size()-1 ?? or RemoveBack() ??
        public static bool RemoveItem(UserType global, String position, String value)
        {
            var finded = UserType.Find(global, position);
            bool isTrue = false;


            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    UserType temp = finded.second.get(i);
                    Vector<int> idx = new Vector<int>();

                    for (int j = 0; j < temp.GetItemListSize(); ++j)
                    {
                        if (value == temp.GetItemList(j).GetVal())
                        {
                            idx.push_back(j);
                        }
                    }
                    //
                    idx.sort(); /// result ex) 5 4 3 2 1 
                    // 
                    for (int j = 0; j < idx.size(); ++j) { 
                        temp.RemoveItemList(idx.get(j));
                    }

                    isTrue = true;
                }
                return isTrue;
            }
            else {
                return false;
            }
        }

        public static bool LoadWizDB(UserType global, String fileName)
        {
            UserType globalTemp = new UserType("global");

            // Scan + Parse 
            if (false == LoadDataFromFile(fileName, ref globalTemp)) { return false; }
            Console.WriteLine("load end : " + fileName);
            global = globalTemp;
            return true;
        }
        // SaveQuery
        public static bool SaveWizDB(UserType global, String fileName, String option = "1", String option2 = "")
        { /// , int option
            FileStream outFile=null;
            StreamWriter sw=null;
            try
            {
                if (option2 == "")
                {
                    outFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(outFile);
                }
                else
                {
                    outFile = new FileStream(fileName, FileMode.Create | FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(outFile);

                    sw.WriteLine("");
                }

                /// saveFile
                if (option == "1") // for eu4.
                    global.Save1(sw); // cf) friend
                else if (option == "2")
                    global.Save2(sw);

                sw.Flush();
                outFile.Close();
            }
            catch( Exception e ) { Console.WriteLine(e.ToString()); if (null != outFile) { outFile.Close(); } return false; }
            return true;
        }

        /// To Do - ExistItem, ExistUserType, SetUserType GetUserType
        public static bool ExistData(UserType global, String position, String varName, String condition) // 
        {
            int count = 0;
            String _var = varName;
            if (_var == " ") { _var = ""; }

            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    count = count + (finded.second.get(i).GetItem(_var).size());
                }
            }
            return 0 != count;
        }
        public static bool ExistUserType(UserType global, String position, String condition) // 
        {
            int count = 0;

            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    count = count + (finded.second.get(i).GetUserTypeListSize());
                }
            }
            return 0 != count;
        }
        public static bool ExistOneUserType(UserType global, String position, String condition) // 
        {
            int count = 0;

            var finded = UserType.Find(global, position);
            if (finded.second.get(0) == global)
            {
                return true;
            }
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    count = count + (finded.second.get(i).GetUserTypeListSize());
                }
            }
            return 1 == count;
        }
        public static bool ExistItem(UserType global, String position, String varName, String condition) // 
        {
            int count = 0;
            String _var = varName;
            if (_var == " ") { _var = ""; }

            var finded = UserType.Find(global, position);
            if (finded.first)
            {
                for (int i = 0; i < finded.second.size(); ++i)
                {
                    if (condition.Length != 0)
                    {
                        Condition cond = new Condition(condition, finded.second.get(i), global);

                        while (cond.Next()) ;

                        if (cond.Now().size() != 1 || "TRUE" != cond.Now().get(0))
                        {
                            //	std.cout << cond.Now().get(0) << endl;
                            continue;
                        }
                    }
                    count = count + (finded.second.get(i).GetItem(_var).size());
                }
            }
            return 0 != count;
        }


        /// ToDo - global, position, var, condition + var is " "!
        // "root" . position.
        public static String SearchItem(UserType global, String var, String condition, String start_dir = "root")
        {
            Vector<String> positionVec = new Vector<string>();
            String temp="";

            SearchItem(global, positionVec, var, start_dir, global, condition);

            for (int i = 0; i < positionVec.size(); ++i)
            {
                temp = temp + positionVec.get(i) + "\n";
            }

            return temp;
        }
        public static String SearchUserType(UserType global, String var, String condition)
        {
            Vector<String> positionVec = new Vector<string>();
            String temp="";

            SearchUserType(global, positionVec, var, "root", global, condition);

            for (int i = 0; i < positionVec.size(); ++i)
            {
                temp = temp + positionVec.get(i) + "\n";
            }

            return temp;
        }
        public static void ReplaceItem(UserType global, String var, String val, String condition, String start_dir)
        {
            UserType ut = UserType.Find(global, start_dir).second.get(0); // chk!!
            ReplaceItem(global, var, start_dir, ut, val, condition);
        }
        public static void RemoveUserTypeTotal(UserType global, String ut_name, String condition, String start_dir)
        {
            UserType ut = UserType.Find(global, start_dir).second.get(0); // chk!!
            RemoveUserTypeTotal(global, ut_name, start_dir, ut, condition);
        }
        public static void ReplaceDateType(UserType global, String val, String condition, String start_dir)
        {
            UserType ut = UserType.Find(global, start_dir).second.get(0); // chk!!
            ReplaceDateType(global, start_dir, ut, val, condition);
        }
        public static void ReplaceDateType2(UserType global, String val, String condition, String start_dir)
        {
            UserType ut = UserType.Find(global, start_dir).second.get(0); // chk!!
            ReplaceDateType2(global, start_dir, ut, val, condition);
        }
        private static void ReplaceItem(UserType global, String var, String nowPosition,
                UserType ut, String val, String condition)
        {
            String _var = var;
            if (_var == " ") { _var = ""; }

            for (int i = 0; i < ut.GetItemListSize(); ++i)
            {
                if (ut.GetItemList(i).GetName() == _var)
                {
                    String _condition = condition;

                    if (_var == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                    else
                        _condition = StringUtility.replace(_condition, "~~", _var); //
                    Condition cond = new Condition(_condition, ut, global);

                    while (cond.Next()) ;

                    if (cond.Now().size() == 1 && "TRUE" == cond.Now().get(0))
                    {
                        ut.GetItemList(i).SetVal(val);
                    }
                }
            }

            for (int i = 0; i < ut.GetUserTypeListSize(); ++i) {
                String temp = ut.GetUserTypeList(i).GetName();
                if (temp == "") { temp = " "; }

                ReplaceItem(
                    global,
                    _var,
                    nowPosition + "/" + temp,
                    ut.GetUserTypeList(i),
                    val,
                    condition

                );
            }
        }
        private static void RemoveUserTypeTotal(UserType global, String ut_name, String nowPosition,
            UserType ut, String condition)
        {
            String _var = ut_name;
            if (_var == " ") { _var = ""; }

            for (int i = 0; i < ut.GetUserTypeListSize(); ++i)
            {
                if (ut.GetUserTypeList(i).GetName() == _var)
                {
                    String _condition = condition;

                    if (_var == "") { _condition = StringUtility.replace(_condition, "~~", "^"); }
                    else
                        _condition = StringUtility.replace(_condition, "~~", _var); //
                    Condition cond = new Condition(_condition, ut, global);

                    while (cond.Next()) ;

                    if (cond.Now().size() == 1 && "TRUE" == cond.Now().get(0))
                    {
                        ut.RemoveUserTypeList(i);
                        --i;
                    }
                }
            }

            for (int i = 0; i < ut.GetUserTypeListSize(); ++i) {
                String temp = ut.GetUserTypeList(i).GetName();
                if (temp == "") { temp = " "; }

                RemoveUserTypeTotal(
                    global,
                    _var,
                    nowPosition + "/" + temp,
                    ut.GetUserTypeList(i),
                    condition

                );
            }
        }
        private static void ReplaceDateType(UserType global, String nowPosition,
            UserType ut, String val, String condition)
        {
            for (int i = 0; i < ut.GetItemListSize(); ++i)
            {
                if (Utility.IsDate(ut.GetItemList(i).GetName()) || Utility.IsDate(ut.GetItemList(i).GetVal()))
                {
                    String _condition = condition;

                    String _var = ut.GetItemList(i).GetName();
                    _condition = StringUtility.replace(_condition, "~~", _var); //

                    Condition cond = new Condition(_condition, ut, global);

                    while (cond.Next()) ;

                    if (cond.Now().size() == 1 && "TRUE" == cond.Now().get(0))
                    {
                        if (Utility.IsDate(ut.GetItemList(i).GetName()))
                        {
                            ut.GetItemList(i).SetName(val);
                        }
                        if (Utility.IsDate(ut.GetItemList(i).GetVal()))
                        {
                            ut.GetItemList(i).SetVal(val);
                        }
                    }
                }
            }

            for (int i = 0; i < ut.GetUserTypeListSize(); ++i) {
                String temp = ut.GetUserTypeList(i).GetName();
                if (temp == "") { temp = " "; }


                ReplaceDateType(
                    global,
                    nowPosition + "/" + temp,
                    ut.GetUserTypeList(i),
                    val,
                    condition

                );
            }
        }
        private static void ReplaceDateType2(UserType global, String nowPosition,
            UserType ut, String val, String condition)
        {
            for (int i = 0; i < ut.GetUserTypeListSize(); ++i)
            {
                String temp = ut.GetUserTypeList(i).GetName();
                if (temp == "") { temp = " "; }

                if (Utility.IsDate(temp))
                {
                    String _condition = condition;

                    String _var = ut.GetUserTypeList(i).GetName();
                    _condition = StringUtility.replace(_condition, "~~", _var); //

                    Condition cond = new Condition(_condition, ut, global);

                    while (cond.Next()) ;

                    if (cond.Now().size() == 1 && "TRUE" == cond.Now().get(0))
                    {
                        ut.GetUserTypeList(i).SetName(val);
                    }
                }


                ReplaceDateType2(
                    global,
                    nowPosition + "/" + temp,
                    ut.GetUserTypeList(i),
                    val,
                    condition

                );
            }
        }
    }
}
