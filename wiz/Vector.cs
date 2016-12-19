using System;
using System.Collections.Generic;
using System.Linq; // in Ling, there is Last method?

namespace ClauTextSharp.wiz
{
    public class Vector<T> : ICloneable where T : ICloneable
    {
        private List<T> arr;

        public Vector(int size)
        {
            arr = new List<T>();
            arr.Capacity = size;

            for( int i=0; i < size;++i)
            {
                arr.Add(default(T));
            }
        }
        public Vector()
        {
            arr = new List<T>();
        }

        public Object Clone()
        {
            List<T> temp = new List<T>();

            temp.Capacity = this.arr.Capacity;
            for (int i = 0; i < this.arr.Count; ++i)
            {
                temp.Add((T)(this.arr[i].Clone()));
            }

            return temp;
        }

        public void push_back(T val)
        {
            arr.Add((T)val.Clone()); //
        }
        public T back()
        {
            return (T)(arr.Last().Clone()); //
        }
        public void pop_back()
        {
            arr.RemoveAt(arr.Count - 1);
        }

        // [] ?
        public void set(int idx, T val) { arr[idx] = (T)val.Clone(); } //
        public T get(int idx) { return (T)arr[idx].Clone();  } //

        public int size() { return arr.Count; }
        public bool empty() { return arr.Count == 0; }
    }
}
