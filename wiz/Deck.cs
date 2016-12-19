using System;
using System.Collections.Generic;
using System.Linq;

namespace ClauTextSharp.wiz
{
    public class Deck<T> : ICloneable where T : ICloneable
    {
        private LinkedList<T> list;

        public Deck()
        {
            list = new LinkedList<T>();
        }

        public Object Clone()
        {
            LinkedList<T> temp = new LinkedList<T>();

            foreach (T x in this.list)
            {
                temp.AddLast((T)x.Clone());
            }

            return temp;
        }

        public void push_front( T val ) { list.AddFirst((T)val.Clone()); } //
        public void push_back( T val ) { list.AddLast((T)val.Clone()); } //

        public T pop_front() { T temp = (T)list.First().Clone(); list.RemoveFirst(); return temp; } //
        public T pop_back() { T temp = (T)list.Last().Clone(); list.RemoveLast(); return temp; } //

        public bool empty() { return list.Count == 0; }
        public int size() { return list.Count; }
    }
}
