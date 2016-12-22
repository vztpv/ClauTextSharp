using System;
using System.Collections.Generic;

namespace ClauTextSharp.load_data
{
    public class Type
    {
        private String _m_name;

        // 생성자
        public Type()
        {
            _m_name = ""; //
        }

        public Type(String name)
        {
            _m_name = name;
        }

        public String GetName() { return _m_name; } //
        public void SetName(String name) { _m_name = name; }
    }
}
