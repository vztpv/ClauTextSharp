﻿using System;
using System.Collections.Generic;

namespace ClauTextSharp.wiz
{
    // chk!!
    public class ArrayQueue<T>
    {
        private List<T> arr;
        private int start = 0;
        private int num = 0;
        private int capacity = 2; // must be 2^n

        public void clear()
        {
            num = 0;
            start = 0;
        }
        public ArrayQueue(int size = 2) // size must be 2^n.
        {
            capacity = size;
            arr = new List<T>();
            for (int i = 0; i < capacity; ++i)
            {
                arr.Add(default(T));
            }
        }

        public ArrayQueue(ArrayQueue<T> other)
        {
            arr = new List<T>(other.arr);
            start = other.start;
            num = other.num;
            capacity = other.capacity;
        }

        private bool is_full()
        {
            return capacity <= num;
        }
        private void expand()
        {
            // expand array queue.
            ArrayQueue<T> temp = new ArrayQueue<T>(capacity * 2);
            //
            for (int i = 0; i < capacity; ++i)
            {
                temp.set(i, arr[(start + i) & (capacity - 1)]);
            }
            temp.start = 0;
            temp.num = size();
            this.num = temp.num;
            this.arr = temp.arr;
            this.start = temp.start;
            this.capacity = temp.capacity;
        }
        public void push(ArrayQueue<T> other) // chk!!
        {
            if (other.empty()) { return; }
            int newSize = capacity;

            while (newSize - num < other.size())
            {
                newSize = newSize * 2;
            }
            if (newSize != this.capacity)
            {
                // expand array queue.
                ArrayQueue<T> temp = new ArrayQueue<T>(newSize);
                temp.start = 0;
                //
                for (int i = 0; i < this.size(); ++i)
                {
                    temp.set(i, this.get(i));
                }

                int iend = other.num;
                for (int i = 0; i < iend; ++i)
                {
                    temp.set(i + this.num, other.get(i));
                }

                temp.num = this.num + other.num;
                this.arr = temp.arr;
                this.num = temp.num;
                this.start = temp.start;
                this.capacity = temp.capacity;
            }
            else
            {
                for (int i = 0; i < other.size(); ++i)
                {
                    this.push((other.get(i)));
                }
            }
        }

        public void push(T val)
        {
            if( is_full() ) { expand(); }
            arr[(start + num) & (capacity - 1)] = val; //
            num++;
        }
        
        public T pop()
        {
            if( empty()) { throw new Exception("array queeue is empty."); }
            T temp = arr[start]; //

            arr[start] = default(T);

            start = (start + 1) & (capacity - 1);

            num--;

            return temp;
        }
        public void set(int idx, T val) { arr[(start + idx) & (capacity - 1)] = val; } //
        public T get(int idx) { return arr[(start + idx) & (capacity - 1)]; } //
        public bool empty() { return this.num == 0; }
        public int size() { return this.num; }
        public override string ToString()
        {
            string temp = "";

            for (int i = 0; i < size(); ++i)
            {
                temp = temp + get(i).ToString() + "\n";
            }
            return temp;
        }
    }
}
