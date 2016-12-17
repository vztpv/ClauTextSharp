using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace ClauTextSharp.wiz
{
    public class ArrayStack<T>
    {
        private List<T> arr;

        public ArrayStack()
        {
            arr = new List<T>();
        }

        public void push( T val )
        {
            arr.Add(val);
        }
        public T top()
        {
            return arr[arr.Count - 1];
        }
        public void pop() // C++ std style.
        {
            arr.RemoveAt(arr.Count - 1);
        }
        public T get( int idx ) { return arr[idx]; }
        public void set( int idx, T val) { arr[idx] = val; }
        public bool empty() { return arr.Count == 0; }
        public int size() { return arr.Count; }
    }
}
