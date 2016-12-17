using System;
using System.Collections.Generic;
using System.Linq;

namespace ClauTextSharp.wiz
{
    public class Deck<T>
    {
        private LinkedList<T> list;

        public Deck()
        {
            list = new LinkedList<T>();
        }

        void push_front( T val ) { list.AddFirst(val); }
        void push_back( T val ) { list.AddLast(val); }

        T pop_front() { T temp = list.First(); list.RemoveFirst(); return temp; }
        T pop_back() { T temp = list.Last(); list.RemoveLast(); return temp; }

        bool empty() { return list.Count == 0; }
        int size() { return list.Count; }
    }
}
