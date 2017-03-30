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
        private readonly static String EQ_STR = "=";


        /// core
        private static bool isState0(long state_reserve)
        {
            return 1 == state_reserve;
        }
        /// core
        public static bool _LoadData(Deck<Token> strVec, Reserver reserver, UserType global)  // first, strVec.empty() must be true!!
        {

            int state = 0;
            int braceNum = 0;
            long state_reserve = 0;
            Vector<UserType> nestedUT = new Vector<UserType>();
            String var1 = "", var2 = "", val = "";


            nestedUT.push_back(global);

		    {
			    reserver.Functor(strVec);

			    while (strVec.empty())
			    {
				    reserver.Functor(strVec);
				    if (
                        strVec.empty() &&
					    reserver.end()
					    ) {
					    return false; // throw "Err nextToken does not exist"; // cf) or empty file or empty String!
				    }
			    }
    }

		
		    while (false == strVec.empty()) {
			    switch (state)
			    {
			    case 0:
				    if (LEFT == (Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
					    state = 2;
				    }
				    else {
					    Pair<Boolean, Token> bsPair = Utility.LookUp(strVec, nestedUT.get(braceNum), reserver);

					    if (bsPair.first) {
						    if (EQ_STR ==(bsPair.second.str)) {
                                String temp = "";
							    Utility.Pop(strVec, ref var2, nestedUT.get(braceNum), reserver);
							    Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);
							    state = 2;
						    }
						    else {
							    if (Utility.Pop(strVec, ref var1, nestedUT.get(braceNum), reserver)) {
								    nestedUT.get(braceNum).AddItem("", var1);
								    state = 0;
							    }
						    }
					    }
					    else {
						    if (Utility.Pop(strVec, ref var1, nestedUT.get(braceNum), reserver)) {
							    nestedUT.get(braceNum).AddItem("", var1);
							    state = 0;
						    }
					    }
				    }
				    break;
			    case 1:
				    if (RIGHT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                        String temp = "";
					    Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);
					    state = 0;
				    }
				    else {
					    // syntax error.
					    throw new Exception("syntax error 1 ");
				    }
				    break;
			    case 2:
				    if (LEFT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                            String temp = "";
                            Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);

					    ///
					    UserType pTemp = new UserType(var2);
                        nestedUT.get(braceNum).AddUserTypeItem(pTemp);

                        braceNum++;

					    /// new nestedUT
					    if (nestedUT.size() == braceNum) /// changed 2014.01.23..
						    nestedUT.push_back(null);

					    /// initial new nestedUT.
					    nestedUT.set(braceNum, pTemp);
					    ///
					    state = 3;
				    }
				    else {
					    if (Utility.Pop(strVec, ref val, nestedUT.get(braceNum), reserver)) {
						    nestedUT.get(braceNum).AddItem(var2, val);
						    var2 = "";
						    val = "";

						    state = 0;
					    }
				    }
				    break;
			    case 3:
				    if (RIGHT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                        String temp = "";
                        Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);

					    nestedUT.set(braceNum, null);
					    braceNum--;

					    state = 0;
				    }
				    else {
					    {
						    /// uisng struct
						    state_reserve++;
						    state = 4;
					    }
					    //else
					    {
						    //	throw  "syntax error 2 ";
					    }
				    }
				    break;
			    case 4:
				    if (LEFT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                        {
                            String strTemp = "";
                            Utility.Pop(strVec, ref strTemp, nestedUT.get(braceNum), reserver);
                        }
					    UserType temp = new UserType("");

                        nestedUT.get(braceNum).AddUserTypeItem(temp);
                        UserType pTemp = temp;

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
				    else if (RIGHT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                        String temp = "";

                        Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);
					    state = isState0(state_reserve) ? 0 : 4;
					    state_reserve--;

					    {
						    nestedUT.set(braceNum, null);
						    braceNum--;
					    }
				    }
				    else {
					    Pair<Boolean, Token> bsPair = Utility.LookUp(strVec, nestedUT.get(braceNum), reserver);
					    if (bsPair.first) {
						    if (EQ_STR ==(bsPair.second.str)) {
                                    // var2strTemp
                                String temp = "";
							    Utility.Pop(strVec, ref var2, nestedUT.get(braceNum), reserver);
							    Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver); // pass EQ_STR
							    state = 6;
						    }
						    else {
							    // var1
							    if (Utility.Pop(strVec, ref var1, nestedUT.get(braceNum), reserver)) {
								    nestedUT.get(braceNum).AddItem("", var1);
								    var1 = "";

								    state = 4;
							    }
						    }
					    }
					    else
					    {
						    // var1
						    if (Utility.Pop(strVec, ref var1, nestedUT.get(braceNum), reserver))
						    {
							    nestedUT.get(braceNum).AddItem("", var1);
							    var1 = "";

							    state = 4;
						    }
					    }
				    }
				    break;
			    case 5:
				    if (RIGHT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                            String temp = "";
					    Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);

					    //if (flag1 == 0) {
					    nestedUT.set(braceNum, null);
					    braceNum--;
					    // }
					    //
					    state = 4;
				    }

				    else {
					    int idx = -1;
                        int num = -1;

					
					    {
						    /// uisng struct
						    state_reserve++;
						    state = 4;
					    }
					    //else
					    {
						    //	throw "syntax error 4  ";
					    }
				    }
				    break;
			    case 6:
				    if (LEFT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                            String temp = "";
					    Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);

					    ///
					    {
						    UserType pTemp = new UserType(var2);
                            nestedUT.get(braceNum).AddUserTypeItem(pTemp);
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
				    else {
					    if (Utility.Pop(strVec, ref val, nestedUT.get(braceNum), reserver)) {

						    nestedUT.get(braceNum).AddItem(var2, val);
						    var2 = ""; val = "";
						    if (strVec.empty())
						    {
							    //
						    }
						    else if (RIGHT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                                    String temp = "";
							    Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);

							    {
								    state = isState0(state_reserve) ? 0 : 4;
								    state_reserve--;

								    {
									    nestedUT.set(braceNum, null);
									    braceNum--;
								    }
							    }
							    {
								    //state = 4;
							    }
						    }
						    else {
							    state = 4;
						    }
					    }
				    }
				    break;
			    case 7:
				    if (RIGHT ==(Utility.Top(strVec, nestedUT.get(braceNum), reserver))) {
                            String temp = "";
					    Utility.Pop(strVec, ref temp, nestedUT.get(braceNum), reserver);
					    //

					    nestedUT.set(braceNum, null);
					    braceNum--;
					    //
					    state = 4;
				    }
				    else {
					    int idx = -1;
                        int num = -1;
					
					    {
						    /// uisng struct
						    state_reserve++;

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
			    }

			    if (strVec.size() < 10) {
				    reserver.Functor(strVec);

				    while (strVec.empty()) // (strVec.empty())
				    {
					    reserver.Functor(strVec);
					    if (
                            strVec.empty() &&
						    reserver.end()
						    ) {
						    // throw "Err nextToken does not exist2";
						    break;
					    }
				    }
			    }
		    }
		    if (state != 0) {
			    throw new Exception("error final state is not 0!  : " + state.ToString());
		    }
		    if (braceNum != 0) {
			    throw new Exception("chk braceNum is " + braceNum.ToString());
		    }
		
		    return true;
	    }

	    public static bool LoadDataFromFile(String fileName, ref UserType global) 
        {
            UserType globalTemp=null;
            TextReader tr = null;

            try
            {
                tr = File.OpenText(fileName);

                globalTemp = new UserType(global);
                Deck<Token> strVec = new Deck<Token>(); ;

                InFileReserver ifReserver = new InFileReserver(tr);

                ifReserver.SetNum(100000);

                // cf) empty file..
                if (false == _LoadData(strVec, ifReserver, globalTemp))
                {
                    return true;
                }

                tr.Close();
            }
		    catch(Exception e)
            {
                if( null != tr) { tr.Close(); }
                return false;
            }

            global = globalTemp;
		    return true;
        }


        public static bool LoadDataFromString(String str, ref UserType ut)
        {
            UserType utTemp = new UserType(ut); // chk new UserType() ?
            Deck<Token> strVec = new Deck<Token>();

            String statement = str; int token_first = 0, token_last = 0; // idx of token in
                                                                             // statement.
            int state = 0;

            for (int i = 0; i < statement.Length; ++i)
            {
                if (0 == state && '\"' == statement[i])
                {
                    // token_last = i - 1;
                    // if (token_last >= 0 && token_last - token_first + 1 > 0) {
                    // strVec.emplace_back(statement.substr(token_first, token_last -
                    // token_first + 1));
                    // }
                    state = 1;
                    // token_first = i;
                    token_last = i;
                }
                else if (1 == state && '\\' == statement[i - 1] && '\"' == statement[i])
                {
                    token_last = i;
                }
                else if (1 == state && '\"' == statement[i])
                {
                    state = 0;
                    token_last = i;

                    // strVec.emplace_back(statement.substr(token_first, token_last -
                    // token_first + 1));
                    // token_first = i + 1;
                }

                if (0 == state && '=' == statement[i])
                {
                    token_last = i - 1;
                    if (token_last >= 0 && token_last - token_first + 1 > 0)
                    {
                        strVec.push_back(new Token(statement.Substring(token_first, token_last + 1)));
                    }
                    strVec.push_back(new Token("="));
                    token_first = i + 1;
                }
                else if (0 == state && Global.IsWhiteSpace(statement[i]))
                { // isspace																																									// etc...// ?
                    token_last = i - 1;
                    if (token_last >= 0 && token_last - token_first + 1 > 0)
                    {
                        strVec.push_back(new Token(statement.Substring(token_first, token_last - token_first + 1)));
                    }
                    token_first = i + 1;
                }
                else if (0 == state && '{' == statement[i])
                {
                    token_last = i - 1;
                    if (token_last >= 0 && token_last - token_first + 1 > 0)
                    {
                        strVec.push_back(new Token(statement.Substring(token_first, token_last + 1)));
                    }
                    strVec.push_back(new Token("{"));
                    token_first = i + 1;
                }
                else if (0 == state && '}' == statement[i])
                {
                    token_last = i - 1;
                    if (token_last >= 0 && token_last - token_first + 1 > 0)
                    {
                        strVec.push_back(new Token(statement.Substring(token_first, token_last - token_first + 1)));
                    }
                    strVec.push_back(new Token("}"));
                    token_first = i + 1;
                }

                if (0 == state && '#' == statement[i])
                { // different from load_data_from_file
                    token_last = i - 1;
                    if (token_last >= 0 && token_last - token_first + 1 > 0)
                    {
                        strVec.push_back(new Token(statement.Substring(token_first, token_last + 1)));
                    }
                    int j = 0;
                    for (j = i; j < statement.Length; ++j)
                    {
                        if (statement[j] == '\n') // cf)																																								// ?
                        { break; }
                    }
                    --j; // "before enter key" or "before end"

                    if (j - i + 1 > 0)
                    {
                        strVec.push_back(new Token(statement.Substring(i, j + 1), true));
                    }
                    token_first = j + 2;
                    i = token_first - 1;
                }
            }

            if (token_first < statement.Length)
            {
                strVec.push_back(new Token(statement.Substring(token_first)));
            }

            try
            {
                // empty String!
                NoneReserver nonReserver = new NoneReserver();
                if (false == _LoadData(strVec, nonReserver, utTemp))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
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
           // try
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
                sw.Close();
                outFile.Close();
            }
          //  catch( Exception e ) { Console.WriteLine(e.ToString()); if (null != sw) { sw.Close(); }  if (null != outFile) { outFile.Close(); } return false; }
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
