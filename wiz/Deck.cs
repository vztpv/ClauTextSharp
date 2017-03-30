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

        public LinkedListNode<T> GetFirst()
        {
            return list.First;
        }
        public LinkedListNode<T> remove(LinkedListNode<T> node)
        {
            LinkedListNode<T> temp = node.Next;
            list.Remove(node);
            return temp;
        }
        public void push_front(T val) { list.AddFirst(val); } //
        public void push_back(T val) { list.AddLast(val); } //

        public T pop_front() { T temp = list.First(); list.RemoveFirst(); return temp; } //
        public T pop_back() { T temp = list.Last(); list.RemoveLast(); return temp; } //

        public bool empty() { return list.Count == 0; }
        public int size() { return list.Count; }

        public LinkedList<T>.Enumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public T get(int idx)
        {
            LinkedListNode<T> val = GetFirst();

            for( int i=0; i < idx; ++i)
            {
                val = val.Next;
            }

            return val.Value;
        }

    }
}
