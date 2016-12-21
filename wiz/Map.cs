using System;
using System.Collections.Generic;


namespace ClauTextSharp.wiz
{
    class Map<Key, Data>
    {
        private Dictionary<Key, Data> dicitonary;

        public Map()
        {
            dicitonary = new Dictionary<Key, Data>();
        }

        public Data getData(Key key)
        {
            return dicitonary[key];
        }
        public void setData(Key key, Data val)
        {
            dicitonary[key] = val;
        }
        public void addData(Key key, Data val)
        {
            dicitonary.Add(key, val);
        }

        public int size() { return dicitonary.Count; }
        public bool empty() { return dicitonary.Count == 0; }
    }
}
