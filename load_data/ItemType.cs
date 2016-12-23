using System;
using System.Collections.Generic;

namespace ClauTextSharp.load_data
{
    public class ItemType<T> : Type
    {
        private T _m_val;
        private bool _m_inited;

        // ructor.
        public ItemType() : base("")
        {
            _m_inited = false;
            _m_val = default(T);
        }
        public ItemType(String name, T value) : base(name)
        {
            _m_inited = true;
            _m_val = value;
        }

        public void Remove(int idx) { _m_val = default(T); }
        public bool Push(T val)
        {
            if (_m_inited) { return false; }
            _m_val = val;
            _m_inited = true;

            return true;
        }
        public T GetVal() { if (_m_inited) { return _m_val; } return default(T); } // chk.. change to throw exception.
        public bool SetVal(T val) { if (_m_inited) { _m_val = val; return true; } return false; }

        public int size() { return _m_inited ? 1 : 0; }
        public bool empty() { return !_m_inited; }
    }
}
