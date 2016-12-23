using System;
using System.Collections.Generic;
using System.Linq;

namespace ClauTextSharp.wiz
{
    public class StringTokenizer
    {
        private Vector<String> _m_str;
        private int _m_count;
        private bool _m_exist;

        private void Init(String str, Vector<String> separator) // assumtion : separators are sorted by length?, long . short
        {
            if (separator.empty() || str.Count() == 0) { return; } // if str.empty() == false then _m_str.push_back(str); // ?

            int left = 0;
            int right = 0;

            _m_exist = false;
            int count = str.Count();
            for (int i = 0; i < count; ++i) {
                right = i;
                int _select = -1;
                bool pass = false;

                for (int j = 0; j < separator.size(); ++j) {
                    int sep_count = separator.get(j).Count();
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

                    if (temp.Count() != 0) {
                        _m_str.push_back(temp);
                    }
                    i = i + separator.get(_select).Count() - 1;
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
        }
		
        public StringTokenizer() { _m_count = 0; _m_exist = false; _m_str = new Vector<String>(); }
        public StringTokenizer(String str, String separator)
        {
            _m_str = new Vector<String>();
            _m_count = 0;
            _m_exist = false;

            Vector<String> vec = new Vector<String>();
            vec.push_back(separator);
            Init(str, vec);
        }
        public StringTokenizer(String str, Vector<String> separator)
        {
            _m_str = new Vector<String>();
            _m_count = 0;
            _m_exist = false;

            Init(str, separator);
        }
        public StringTokenizer(String str)
        {
            _m_str = new Vector<String>();
            _m_count = 0;
            _m_exist = false;

            Vector<String> vec = new Vector<String>();

            vec.push_back(" ");
            vec.push_back("\t");
            vec.push_back("\r");
            vec.push_back("\n");

            Init(str, vec);
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
            return _m_count<_m_str.size();
        }

        public bool isFindExist()
		{
			return _m_exist;
		}

    }
}
