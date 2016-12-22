using System;
using System.Collections.Generic;
using System.Linq; // in Ling, there is Last method?

namespace ClauTextSharp.wiz
{
    public class Vector<T>
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
        public Vector(List<T> other)
        {
            arr = new List<T>();

            for( int i=0; i < other.Count; ++i)
            {
                arr.Add(other[i]);
            }
        }

        public void push_back(T val)
        {
            arr.Add(val); //
        }
        public T back()
        {
            return arr.Last(); //
        }
        public void pop_back()
        {
            arr.RemoveAt(arr.Count - 1);
        }

        public void clear()
        {
            arr.Clear();
        }
        // [] ?
        public void set(int idx, T val) { arr[idx] = val; } //
        public T get(int idx) { return arr[idx];  } //

        public int size() { return arr.Count; }
        public bool empty() { return arr.Count == 0; }

        public void insert(int idx, T val)
        {
            arr.Insert(idx, val);
        }
        public void reverse() // chk!
        {
            int middle = arr.Count / 2;
            T temp = default(T);
               
            for( int i=0; i < middle; ++i)
            {
                // swap arr[i] and arr[arr.Count-1 - i]
                temp = arr[i];
                arr[i] = arr[arr.Count - 1 - i];
                arr[arr.Count - 1 - i] = temp;
            }
        }
    }
}
