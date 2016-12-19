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

        public void push_front( T val ) { list.AddFirst(val); } //
        public void push_back( T val ) { list.AddLast(val); } //

        public T pop_front() { T temp = list.First(); list.RemoveFirst(); return temp; } //
        public T pop_back() { T temp = list.Last(); list.RemoveLast(); return temp; } //

        public bool empty() { return list.Count == 0; }
        public int size() { return list.Count; }
    }
}
