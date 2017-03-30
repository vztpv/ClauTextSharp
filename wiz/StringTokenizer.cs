using System;
using System.Collections.Generic;
using System.Linq;

namespace ClauTextSharp.wiz
{
    public class StringTokenizer
    {
        private Vector<String> _m_str = new Vector<String>();
        private int _m_count;
        private int option;
        private bool _m_exist;
        Vector<String> whitespaceVec = new Vector<String>();

        public bool init(String str)
        {
            return init(str, whitespaceVec);
        }
        public bool init(String str, Vector<String> separator) // assumtion : separators are sorted by length?, long . short
        {
            { // reset..
                _m_str.clear();
                _m_count = 0;
                _m_exist = false;
            }

            if (separator.empty() || str.Length == 0) { return false; } // if str.empty() == false then _m_str.push_back(str); // ?
            
            int left = 0;
            int right = 0;
            int state = 0;

            _m_exist = false;
            int count = str.Length;
            for (int i = 0; i < count; ++i) {
                right = i;
                int _select = -1;
                bool pass = false;

                // chk " "
                if (1 == option && 0 == state && '\"' == str[i])
                {
                    if (i == 0)
                    {
                        state = 1;
                        continue;
                    }
                    else if (i > 0 && '\\' == str[i - 1])
                    {
                        throw new Exception("ERROR for Init on StringTokenizer"); // error!
                    }
                    else if (i > 0)
                    {
                        state = 1;
                        continue;
                    }
                }
                else if (1 == option && 1 == state && '\"' == str[i])
                {
                    if ('\\' == str[i - 1])
                    {
                        continue;
                    }
                    else
                    {
                        state = 0;
                    }
                }
                else if (1 == option && 1 == state)
                {
                    continue;
                }

                for (int j = 0; j < separator.size(); ++j) {
                    int sep_count = separator.get(j).Length;
                    for (int k = 0; k < sep_count; ++k) {
                        if (str[i] == separator.get(j)[k]) {
                            pass = true;
                        }
                        else {
                            pass = false;
                            break;
                        }
                    }
                    if (pass) { _select = j; break; }
                }

                if (pass) {
                    this._m_exist = true;

                    String temp = str.Substring(left, right - 1 - left + 1);

                    if (temp.Length != 0) {
                        _m_str.push_back(temp);
                    }
                    i = i + separator.get(_select).Length - 1;
                    left = i + 1;
                    right = left;
                }
                else if (!pass && i == count - 1) {
                    String temp = str.Substring(left, right - left + 1);
                    if (temp.Count() != 0) {
                        _m_str.push_back(temp);
                    }
                }
            }

            return true;
        }
		
        public StringTokenizer()
        {
            _m_count = 0; _m_exist = false; option = 0;
            whitespaceVec.push_back(" ");
            whitespaceVec.push_back("\t");
            whitespaceVec.push_back("\r");
            whitespaceVec.push_back("\n");
        }
        public StringTokenizer(String str, String separator, int option = 0)
        {
            whitespaceVec.push_back(" ");
            whitespaceVec.push_back("\t");
            whitespaceVec.push_back("\r");
            whitespaceVec.push_back("\n");
            
            _m_count = 0;
            _m_exist = false;

            Vector<String> vec = new Vector<String>();
            vec.push_back(separator);
            this.option = option;
            init(str, vec);
        }
        public StringTokenizer(String str, Vector<String> separator, int option=0)
        {
            whitespaceVec.push_back(" ");
            whitespaceVec.push_back("\t");
            whitespaceVec.push_back("\r");
            whitespaceVec.push_back("\n");
            
            _m_count = 0;
            _m_exist = false;

            this.option = option;
            init(str, separator);
        }
        public StringTokenizer(String str, int option=0)
        {
            whitespaceVec.push_back(" ");
            whitespaceVec.push_back("\t");
            whitespaceVec.push_back("\r");
            whitespaceVec.push_back("\n");

            _m_str = new Vector<String>();
            _m_count = 0;
            _m_exist = false;


            this.option = option;
            init(str, whitespaceVec);
        }

        public int countTokens()
        {
            return _m_str.size();
        }
        public String nextToken()
        {
            if (hasMoreTokens())
            {
                _m_count++;
                return _m_str.get(_m_count - 1); //
            }
            else {
                return "";
            }
        }
        public bool hasMoreTokens()
        {
            return _m_count < _m_str.size();
        }

        public bool isFindExist()
		{
			return _m_exist;
		}

    }
}
