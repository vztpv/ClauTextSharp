using System;
using System.Collections.Generic;


namespace ClauTextSharp.wiz
{
    public class ArrayStack<T> : ICloneable where T : ICloneable
    {
        private List<T> arr;

        public ArrayStack()
        {
            arr = new List<T>();
        }

        public Object Clone()
        {
            List<T> temp = new List<T>();

            foreach (T x in this.arr)
            {
                temp.Add((T)x.Clone());
            }

            return temp;
        }

        public void push( T val )
        {
            arr.Add((T)val.Clone());
        }
        public T top()
        {
            return (T)arr[arr.Count - 1].Clone(); //
        }
        public void pop() // C++ std style.
        {
            arr.RemoveAt(arr.Count - 1);
        }
        public T get( int idx ) { return (T)arr[idx].Clone(); } //
        public void set( int idx, T val) { arr[idx] = (T)val.Clone(); } //
        public bool empty() { return arr.Count == 0; }
        public int size() { return arr.Count; }
    }
}
