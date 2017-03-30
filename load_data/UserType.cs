using System;
using System.Collections.Generic;
using System.Threading;

using System.IO;

using ClauTextSharp.wiz;

namespace ClauTextSharp.load_data
{
    public class UserType : Type
    {
        public void PushComment(String comment)
        {
            commentList.push_back(comment);
        }
        // private memeber..
        private Vector<int> ilist;
        private Vector<String> commentList;
        private Vector<ItemType<String>> itemList;
        private Vector<UserType> userTypeList;
        private UserType parent;

        // constructor.
        public UserType(String name = "") : base(name)
        {
            ilist = new Vector<int>();
            itemList = new Vector<ItemType<String>>();
            userTypeList = new Vector<UserType>();
            commentList = new Vector<String>();
            parent = null;
        }
        public UserType(UserType other) : base(other.GetName())
        {
            Reset(other);
        }
        // chk!!
        private void Reset(UserType other)
        {
            ilist = (other.ilist);
            itemList = (other.itemList);
            commentList = other.commentList;
            parent = other.parent;

            userTypeList = new Vector<UserType>();
            for (int i = 0; i < other.userTypeList.size(); i++)
            {
                userTypeList.push_back((other.userTypeList.get(i)));
            }
        }
        //
        public int GetIListSize() { return ilist.size(); }
        public int GetItemListSize() { return itemList.size(); }
        public int GetUserTypeListSize() { return userTypeList.size(); }

        public ItemType<String> GetItemList(int idx) { return itemList.get(idx); }
        public UserType GetUserTypeList(int idx) { return userTypeList.get(idx); }
        public UserType GetParent() { return parent; }
        // idx <- ilist`s idx
        public bool IsItemType(int idx) { return ilist.get(idx) == 1; }
        public bool IsUserType(int idx) { return ilist.get(idx) == 2; }
        public Type GetList(int idx) {
            if(IsItemType(idx))
            {
                int item_idx = -1;

                for (int i = 0; i < ilist.size() && i <= idx; ++i)
                {
                    if (IsItemType(i))
                    {
                        item_idx++;
                    }
                }

                return (Type)itemList.get(item_idx);
            }
            else
            {
                int usertype_idx = -1;

                for (int i = 0; i < ilist.size() && i <= idx; ++i)
                {
                    if (IsUserType(i))
                    {
                        usertype_idx++;
                    }
                }

                return (Type)itemList.get(usertype_idx);
            }
        }

        private int _GetIndex(Vector<int> ilist, int val, int start = 0)
        {
            for (int i = start; i < ilist.size(); ++i)
            {
                if (ilist.get(i) == val) { return i; }
            }
            return -1;
        }
        private int _GetItemIndexFromIlistIndex(Vector<int> ilist, int ilist_idx)
        {
            if (ilist.size() == ilist_idx) { return ilist.size(); }
            int idx = _GetIndex(ilist, 1, 0);
            int item_idx = -1;

            while (idx != -1)
            {
                item_idx++;
                if (ilist_idx == idx) { return item_idx; }
                idx = _GetIndex(ilist, 1, idx + 1);
            }

            return -1;
        }
        private int _GetUserTypeIndexFromIlistIndex(Vector<int> ilist, int ilist_idx)
        {
            if( ilist.size() == ilist_idx ) { return ilist.size(); }
            int idx = _GetIndex(ilist, 2, 0);
            int usertype_idx = -1;

            while (idx != -1)
            {
                usertype_idx++;
                if (ilist_idx == idx) { return usertype_idx; }
                idx = _GetIndex(ilist, 2, idx + 1);
            }

            return -1;
        }
        private int _GetIlistIndex(Vector<int> ilist, int index, int type)
        {
            int count = -1;

            for (int i = 0; i < ilist.size(); ++i)
            {
                if (ilist.get(i) == type)
                {
                    count++;
                    if (index == count)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        // Remove
        public void RemoveItemList(int idx)
        {
            for (int i = idx + 1; i < GetItemListSize(); ++i)
            {
                itemList.set(i - 1, itemList.get(i));
            }
            itemList.pop_back(); // resize(itemList.size()-1);

            int count = 0;
            for (int i = 0; i < ilist.size(); ++i)
            {
                if (IsItemType(i)) { count++; }
                if (count == idx + 1)
                {
                    for (int k = i + 1; k < ilist.size(); ++k)
                    {
                        ilist.set(k - 1, ilist.get(k));
                    }
                    ilist.pop_back(); // ilist.resize( ilist.size() - 1 )
                    break;
                }
            }
        }
        public void RemoveUserTypeList(int idx, bool chk = true)
        {
            if (chk)
            {
               userTypeList.set(idx, null);
            }
            // left shift start idx, to end, at itemList. and resize!
            for (int i = idx + 1; i < GetUserTypeListSize(); ++i)
            {
                userTypeList.set(i - 1, userTypeList.get(i));
            }
            userTypeList.pop_back();

            //  ilist left shift and resize - count itemType!
            int count = 0;
            for (int i = 0; i < ilist.size(); ++i)
            {
                if (IsUserType(i)) { count++; }
                if (count == idx + 1)
                {
                    // i���� left shift!and resize!
                    for (int k = i + 1; k < ilist.size(); ++k)
                    {
                        ilist.set(k - 1, ilist.get(k));
                    }
                    ilist.pop_back();
                    break;
                }
            }
        }
        public void RemoveItemList(String varName)
		{
			int k = _GetIndex(ilist, 1, 0);
            Vector<ItemType<String>> tempDic = new Vector<ItemType<String>>();
			for (int i = 0; i<itemList.size(); ++i) {
				if (varName != itemList.get(i).GetName()) {
					tempDic.push_back(itemList.get(i));
					k = _GetIndex(ilist, 1, k + 1);
                }
				else {
					// remove item, ilist left shift 1.
					for (int j = k + 1; j<ilist.size(); ++j) {
						ilist.set(j-1, ilist.get(j));
					}
                    ilist.pop_back();
					k = _GetIndex(ilist, 1, k);
				}
			}
			itemList = (tempDic);
		}
        public void RemoveItemList() /// ALL
        {
            itemList.clear(); // itemList = new Vector<ItemType<String>>();
            //
            Vector<int> temp = new Vector<int>();

            int before_size = ilist.size();
            
            for (int i = 0; i < before_size; ++i)
            {
                if (IsUserType(i))
                {
                    temp.push_back(2);
                }
            }
            ilist = temp;
        }
        public void RemoveEmptyItem() // fixed..
        {
            int k = _GetIndex(ilist, 1, 0);
            Vector<ItemType<String>> tempDic = new Vector<ItemType<String>>();
            for (int i = 0; i < itemList.size(); ++i)
            {
                if (itemList.get(i).size() > 0)
                {
                    tempDic.push_back(itemList.get(i));
                }
                else
                {
                    // remove item, ilist left shift 1.
                    for (int j = k + 1; j < ilist.size(); ++j)
                    {
                        ilist.set(j - 1, ilist.get(j));
                    }
                    ilist.pop_back();
                }
                k = _GetIndex(ilist, 1, k + 1);
            }
            itemList = tempDic;
        }
        public void Remove()
        {
            /// parent.removeUserType(name); - ToDo - X
            ilist.clear(); // = Vector<int>();
            itemList.clear(); // = Vector<ItemType<String>>();
            commentList.clear();
            RemoveUserTypeList();
        }
        public void RemoveUserTypeList()
        { /// chk memory leak test!!
            for (int i = 0; i < userTypeList.size(); i++)
            {
                if (null != userTypeList.get(i))
                {
                    userTypeList.set(i, null);
                }
            }
            // DO Empty..
            userTypeList.clear();

            Vector<int> temp = new Vector<int>();
            for (int i = 0; i < ilist.size(); ++i)
            {
                if (IsItemType(i))
                {
                    temp.push_back(1);
                }
            }
            ilist = temp;
        }
        public void RemoveUserTypeList(String varName, bool chk = true)
        {
            int k = _GetIndex(ilist, 2, 0);
            Vector<UserType> tempDic = new Vector<UserType>();
            for (int i = 0; i < userTypeList.size(); ++i)
            {
                if (varName != userTypeList.get(i).GetName())
                {
                    tempDic.push_back(userTypeList.get(i));
                    k = _GetIndex(ilist, 2, k + 1);
                }
                else
                {
                    if (chk)
                    {
                        userTypeList.set(i, null);
                    }
                    // remove usertypeitem, ilist left shift 1.
                    for (int j = k + 1; j < ilist.size(); ++j)
                    {
                        ilist.set(j - 1, ilist.get(j));
                    }
                    ilist.pop_back();
                    k = _GetIndex(ilist, 2, k);
                }
            }
            userTypeList = tempDic;
        }
        //			
        public void RemoveList(int idx) // ilist_idx!
        {
            // chk whether item or usertype.
            // find item_idx or usertype_idx.
            // remove item or remove usertype.
            if (IsItemType(idx))
            {
                int item_idx = -1;

                for (int i = 0; i < ilist.size() && i <= idx; ++i)
                {
                    if (IsItemType(i)) { item_idx++; }
                }

                RemoveItemList(item_idx);
            }
            else
            {
                int usertype_idx = -1;

                for (int i = 0; i < ilist.size() && i <= idx; ++i)
                {
                    if (IsUserType(i)) { usertype_idx++; }
                }

                RemoveUserTypeList(usertype_idx);
            }
        }

        //
        public bool empty() { return ilist.empty(); }

        // chk
        public void InsertItemByIlist(int ilist_idx, String name, String item)
        {
            int itemIndex = _GetItemIndexFromIlistIndex(ilist, ilist_idx);

            ilist.push_back(0);
            for (int i = ilist_idx + 1; i < ilist.size(); ++i)
            {
                ilist.set(i, ilist.get(i - 1));
            }
            ilist.set(ilist_idx, 1);

            itemList.push_back(new ItemType<String>("", ""));
            for (int i = itemIndex + 1; i < itemList.size(); ++i)
            {
                itemList.set(i, itemList.get(i - 1));
            }
            itemList.set(itemIndex, new ItemType<String>(name, item));
        }
        public void InsertUserTypeByIlist(int ilist_idx, UserType item)
        {
            int userTypeIndex = _GetUserTypeIndexFromIlistIndex(ilist, ilist_idx);
            UserType temp = item; // new UserType(item);

            temp.parent = this;

            ilist.push_back(0);
            for (int i = ilist_idx + 1; i < ilist.size(); ++i)
            {
                ilist.set(i, ilist.get(i - 1));
            }
            ilist.set(ilist_idx, 2);

            userTypeList.push_back(null);
            for (int i = userTypeIndex + 1; i < userTypeList.size(); ++i)
            {
                userTypeList.set(i, userTypeList.get(i-1));
            }
            userTypeList.set(userTypeIndex, temp);
        }
        //
        public void AddItemType(ItemType<String> item)
        {
            itemList.push_back(item);
            ilist.push_back(1);
        }
        public void AddItem(String name, String val)
        {
            itemList.push_back(new ItemType<String>(name, val));
            ilist.push_back(1);
        }
        public void AddUserTypeItem(UserType item)
        {
            UserType temp = item; // new UserType(item); //new UserType 제외??
            temp.parent = this;

            ilist.push_back(2);

            userTypeList.push_back(temp);
        }
        public void AddItemAtFront(String name, String item)
        {
            itemList.insert(0, new ItemType<String>(name, item));

            ilist.insert(0, 1);
        }
        public void AddUserTypeItemAtFront(UserType item)
        {
            UserType temp = item;// new UserType(item);
            temp.parent = this;

            ilist.insert(0, 2);

            userTypeList.insert(0, temp);
        }
        
        public Vector<ItemType<String>> GetItem(String name)
        {
			Vector<ItemType<String>> temp = new Vector<ItemType<String>>();

			for (int i = 0; i < itemList.size(); ++i) {
				if (itemList.get(i).GetName() == name) {
					temp.push_back(itemList.get(i));
				}
			}
			return temp;
		}
		public bool SetItem(String name, String value)
        {
            int index = -1;

            for (int i = 0; i < itemList.size(); ++i)
            {
                if (itemList.get(i).GetName() == name)
                {
                    itemList.get(i).SetVal(value);
                    index = i;
                }
            }

            return -1 != index;
        }
            /// add set Data
        public bool SetItem(int var_idx, String value)
        {
             itemList.get(var_idx).SetVal(value);
             return true;
        }

        public Vector<UserType> GetUserTypeItem(String name)
        { /// chk...
			Vector<UserType> temp = new Vector<UserType>();

			for (int i = 0;  i < userTypeList.size(); ++i) {
				if (userTypeList.get(i).GetName() == name) {
					temp.push_back(userTypeList.get(i));
				}
			}

			return temp;
		}
        public bool GetLastUserTypeItemRef(String name, ref UserType refUT)
        {
            int idx = -1;

            for (int i = 0; i < userTypeList.size(); ++i)
            {
                if (name == userTypeList.get(i).GetName())
                {
                    idx = i;
                }
            }
            if (idx > -1)
            {
                refUT = userTypeList.get(idx);
            }
            return idx > -1;
        }

        // Save?
        /// save1 - like EU4 savefiles. , FileStream . StreamWriter?
        private void Save1(StreamWriter sw, UserType ut, int depth = 0)
        {
			int itemListCount = 0;
            int userTypeListCount = 0;

            for (int i = 0; i < ut.commentList.size(); ++i)
            {
                for (int k = 0; k < depth; ++k)
                {
                    sw.Write("\t");
                }
                sw.Write(ut.commentList.get(i));

                if (i < ut.commentList.size() - 1 || false == ut.ilist.empty())
                {
                    sw.Write("\n");
                }

            }
            for (int i = 0; i < ut.ilist.size(); ++i) {
				if (ut.IsItemType(i)) {
					for (int j = 0; j < ut.itemList.get(itemListCount).size(); j++) {
						for (int k = 0; k<depth; ++k) {
							sw.Write("\t");
						}
						if (ut.itemList.get(itemListCount).GetName() != "") {	
							sw.Write(ut.itemList.get(itemListCount).GetName() + "=");
						}
						sw.Write(ut.itemList.get(itemListCount).GetVal());
						if (j != ut.itemList.get(itemListCount).size() - 1) {
                            sw.WriteLine("");
						}
					}
					if (i != ut.ilist.size() - 1) {
                        sw.WriteLine("");
                    }
					itemListCount++;
				}
				else {
					for (int k = 0; k < depth; ++k) {
						sw.Write("\t");
					}

					if (ut.userTypeList.get(userTypeListCount).GetName() != "") {
						sw.Write(ut.userTypeList.get(userTypeListCount).GetName() + "=");
					}
                    sw.WriteLine("{");
                    
                    Save1(sw, ut.userTypeList.get(userTypeListCount), depth + 1);
                    sw.WriteLine("");
                    						
					for (int k = 0; k < depth; ++k) {
                        sw.Write("\t");
					}
					sw.Write("}");

					if (i != ut.ilist.size() - 1) {
                        sw.WriteLine("");
                    }

					userTypeListCount++;
				}
			}
		}
		/// save2 - for more seed loading data!
		private void Save2(StreamWriter sw, UserType ut, int depth = 0)
        {
			int itemListCount = 0;
            int userTypeListCount = 0;

            for (int i = 0; i < ut.commentList.size(); ++i)
            {
                for (int k = 0; k < depth; ++k)
                {
                    sw.Write("\t");
                }
                sw.Write(ut.commentList.get(i));

                if (i < ut.commentList.size() - 1 || false == ut.ilist.empty())
                {
                    sw.Write("\n");
                }

            }
            for (int i = 0; i < ut.ilist.size(); ++i) {
				if (ut.IsItemType(i)) {
					for (int j = 0; j < ut.itemList.get(itemListCount).size(); j++) {
						for (int k = 0; k < depth; ++k) {
                            sw.Write("\t");
						}
						if (ut.itemList.get(itemListCount).GetName() != "")
							sw.Write(ut.itemList.get(itemListCount).GetName() + " = ");
						sw.Write(ut.itemList.get(itemListCount).GetVal());
						if (j != ut.itemList.get(itemListCount).size() - 1)
							sw.Write(" ");
					}
					if (i != ut.ilist.size() - 1) {
						sw.WriteLine("");
					}
					itemListCount++;
				}
				else {
					if (ut.userTypeList.get(userTypeListCount).GetName() != "")
					{
						sw.Write(ut.userTypeList.get(userTypeListCount).GetName() + " = ");
					}
					sw.WriteLine("{");


                    Save2(sw, ut.userTypeList.get(userTypeListCount), depth + 1);
                    sw.WriteLine("");
						
					for (int k = 0; k < depth; ++k) {
                        sw.WriteLine("\t");
					}
                    sw.WriteLine("}");
					if (i != ut.ilist.size() - 1) {
                        sw.WriteLine("");
					}
					userTypeListCount++;
				}
			}
		}
	    public void Save1(StreamWriter sw)
        {
            Save1(sw, this);
		}
        public void Save2(StreamWriter sw)
        {
            Save2(sw, this);
		}
        public String ItemListToString()
		{
			String temp = "";
            int itemListCount = 0;

			for (int i = 0; i < itemList.size(); ++i) {
				for (int j = 0; j < itemList.get(itemListCount).size(); j++) {
					if (itemList.get(itemListCount).GetName() != "")
						temp = temp + itemList.get(itemListCount).GetName() + " = ";
					temp = temp + itemList.get(itemListCount).GetVal();
					if (j != itemList.get(itemListCount).size() - 1) {
						temp = temp + "/";
					}
				}
				if (i != itemList.size() - 1)
				{
					temp = temp + "/";
				}
				itemListCount++;
			}
			return temp;
		}
		public String ItemListNamesToString()
		{
			String temp = "";
            int itemListCount = 0;

			for (int i = 0; i < itemList.size(); ++i) {
				for (int j = 0; j < itemList.get(itemListCount).size(); j++) {
					if (itemList.get(itemListCount).GetName() != "")
						temp = temp + itemList.get(itemListCount).GetName();
					else
						temp = temp + " ";

					if (j != itemList.get(itemListCount).size() - 1) {
						temp = temp + "/";
					}
				}
				if (i != itemList.size() - 1)
				{
					temp = temp + "/";
				}
				itemListCount++;
			}
			return temp;
		}
		public Vector<String> userTypeListNamesToStringArray()
		{
			Vector<String> temp = new Vector<String>();
            int userTypeListCount = 0;

			for (int i = 0; i<userTypeList.size(); ++i) {
				if (userTypeList.get(userTypeListCount).GetName() != "") {
					temp.push_back(userTypeList.get(userTypeListCount).GetName());
				}
				else {
					temp.push_back(" "); // chk!! cf) wiz.load_data.Utility.Find function...
				}
				userTypeListCount++;
			}
			return temp;
		}
		public String UserTypeListNamesToString()
		{
			String temp = "";
            int userTypeListCount = 0;

			for (int i = 0; i<userTypeList.size(); ++i) {
				if (userTypeList.get(userTypeListCount).GetName() != "") {
					temp = temp + userTypeList.get(userTypeListCount).GetName();
				}
				else {
					temp = temp + " "; // chk!! cf) wiz.load_data.Utility.Find function...
				}

				if (i != itemList.size() - 1)
				{
					temp = temp + "/";
				}
				userTypeListCount++;
			}
			return temp;
		}
		public override String ToString()
		{
			String temp = "";
            int itemListCount = 0;
            int userTypeListCount = 0;

			for (int i = 0; i<ilist.size(); ++i) {
				//std.cout << "ItemList" << endl;
				if (IsItemType(i)) {
					for (int j = 0; j < itemList.get(itemListCount).size(); j++) {
						if (itemList.get(itemListCount).GetName() != "")
							temp += itemList.get(itemListCount).GetName() + " = ";
						temp += itemList.get(itemListCount).GetVal();
						if (j != itemList.get(itemListCount).size() - 1)
							temp += " ";
					}
					if (i != ilist.size() - 1) {
						temp += " ";
					}
					itemListCount++;
				}
				else {
					// std.cout << "UserTypeList" << endl;
					if (userTypeList.get(userTypeListCount).GetName() != "") {
						temp = temp + userTypeList.get(userTypeListCount).GetName() + " = ";
					}
					temp += " { ";
					temp += (userTypeList.get(userTypeListCount).ToString()) + " ";
					temp += " }";
					if (i != ilist.size() - 1) {
						temp += " ";
					}

					userTypeListCount++;
				}
			}
			return temp;
		}

        // Find Directory?
        public static Pair<bool, Vector<UserType>> Find(UserType global, String _position) /// option, option_offset
		{
			String position = _position;
            Vector<UserType> temp = new Vector<UserType>();

            if (position.Length != 0 && position[0] == '@') { position.Remove(0, 1); } 
		    if (position.Length == 0) { temp.push_back(global); return new Pair<bool, Vector<UserType>>(true, temp); }
			if (position == ".") { temp.push_back(global); return new Pair<bool, Vector<UserType>>(true, temp); }
			if (position == "/./") { temp.push_back(global); return new Pair<bool, Vector<UserType>>(true, temp); } // chk..
			if (position == "/.") { temp.push_back(global); return new Pair<bool, Vector<UserType>>(true, temp); }
			if (StringUtility.startsWith(position, "/."))
			{
				position = StringUtility.subString(position, 3);
			}

			StringTokenizer tokenizer = new StringTokenizer(position, "/");
            Vector<String> strVec = new Vector<String>();
            Deck<Pair<UserType, int>> utDeck = new Deck<Pair<UserType, int>>();
            Pair<UserType, int> utTemp = new Pair<UserType, int>(global, 0);
            ItemType<UserType> utTemp2 = new ItemType<UserType>();

			for (int i = 0; i<tokenizer.countTokens(); ++i) {
				String strTemp = tokenizer.nextToken();
				if (strTemp == "root" && i == 0) {
				}
				else {
					strVec.push_back(strTemp);
				}

				if ((strVec.size() >= 1) && (" " == strVec.back())) /// chk!!
				{
					strVec.set(strVec.size() - 1, "");
				}
			}

			// maybe, has bug!
			{
				int count = 0;

				for (int i = 0; i<strVec.size(); ++i) {
					if (strVec.get(i) == "..") {
						count++;
					}
					else {
						break;
					}
				}

				strVec.reverse();

				for (int i = 0; i<count; ++i) {
					if (utTemp.first == null) {
                        return new Pair<bool, Vector<UserType>>(false, new Vector<UserType>());
					}
					utTemp.first = utTemp.first.GetParent();
                    strVec.pop_back();
				}
                strVec.reverse();
			}

			utDeck.push_front(utTemp);

			bool exist = false;
			while (false == utDeck.empty()) {
				utTemp = utDeck.pop_front();
					
				if (utTemp.second<strVec.size() &&
					StringUtility.startsWith(strVec.get(utTemp.second), "$ut")
					)
				{
                    long idx = 0;
                    if( false  == long.TryParse(StringUtility.subString(strVec.get(utTemp.second), 3), out idx))
                    {
                        throw new Exception("$ut..., ... is not long(integer)"); // 
                    }

					if (idx< 0 || idx >= utTemp.first.GetUserTypeListSize()) {
						throw new Exception ("ERROR NOT VALID IDX");
					}

					utDeck.push_front(new Pair<UserType, int>(utTemp.first.GetUserTypeList((int)idx), utTemp.second + 1));
				}
				else if (utTemp.second < strVec.size() && strVec.get(utTemp.second) == "$")
				{
					for (int j = utTemp.first.GetUserTypeListSize() - 1; j >= 0; --j) {
						UserType x = utTemp.first.GetUserTypeList(j);
                        utDeck.push_front(new Pair<UserType, int>(x, utTemp.second + 1));
					}
				}
				else if (utTemp.second<strVec.size() &&
					(utTemp.first.GetUserTypeItem(strVec.get(utTemp.second)).empty() == false))
				{
					Vector<UserType> x = utTemp.first.GetUserTypeItem(strVec.get(utTemp.second));
					for (int j = x.size() - 1; j >= 0; --j) {
						utDeck.push_front(new Pair<UserType, int>(x.get(j), utTemp.second + 1));
					}
				}

				if (utTemp.second == strVec.size()) {
					exist = true;
					temp.push_back(utTemp.first);
				}
			}
			if (false == exist) { return new Pair<bool, Vector<UserType>>(false, new Vector<UserType>()); }
            else {        
                return new Pair<bool, Vector<UserType>>(true, temp);
		    }	
        }
    }
}
