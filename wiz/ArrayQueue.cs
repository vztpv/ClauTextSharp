using System;
using System.Collections.Generic;

namespace ClauTextSharp.wiz
{
    public class ArrayQueue<T> : ICloneable where T : ICloneable
    {
        private List<T> arr;
        private int start = 0;
        private int num = 0;
        private int capacity = 2; // must be 2^n

        public ArrayQueue( int size = 2) // size must be 2^n.
        {
            capacity = size;
            arr = new List<T>();
            for (int i = 0; i < capacity; ++i)
            {
                arr.Add(default(T));
            }
        }

        public Object Clone()
        {
            ArrayQueue<T> temp = new ArrayQueue<T>();

            temp.start = this.start;
            temp.num = this.num;
            temp.capacity = this.capacity;

            foreach(T x in this.arr)
            {
                temp.arr.Add(x);
            }

            return temp;
        }

        private bool is_full()
        {
            return capacity <= num;
        }
        private void expand()
        { 
            arr.AddRange(arr.GetRange(0, capacity)); // chk??
            for( int i=0; i < capacity; ++i)
            {
                arr[i] = arr[capacity + (start + i) & (capacity - 1)];
            }
            capacity = capacity * 2;
            start = 0;
        }
        public void push(ArrayQueue<T> other)
        {
            if (other.empty()) { return; }
            int newSize = capacity;

            while (newSize - num < other.size())
            {
                newSize = newSize * 2;
            }
            if (newSize != capacity)
            {
                for( int i=num; i < newSize; ++i )
                {
                    this.arr.Add(default(T));
                }
            }
            for( int i=0; i < other.num; ++i)
            {
                this.push2(other.get(i));
            }
        }
        public void push(T val)
        {
            if( is_full() ) { expand(); }
            arr[(start + num) & (capacity - 1)] = val; //
            num++;
        }
        private void push2(T val) 
        {
            if (is_full()) { expand(); }
            arr[(start + num) & (capacity - 1)] = val; //
            num++;
        }
        public T pop()
        {
            T temp = arr[start]; //

            arr[start] = default(T);

            start = (start + 1) & (capacity - 1);

            num--;

            return temp;
        }
        public void set(int idx, T val) { arr[(start + idx) & (capacity - 1)] = val; } //
        public T get(int idx) { return arr[(start + idx) & (capacity - 1)]; } //
        public bool empty() { return arr.Count == 0; }
        public int size() { return arr.Count; }
    }
}
