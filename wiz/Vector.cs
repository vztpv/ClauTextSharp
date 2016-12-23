using System;
using System.Collections.Generic;
using System.Linq; // in Ling, there is Last method?

namespace ClauTextSharp.wiz
{
    public class Vector<T>
    {
        private List<T> arr;
        private int num;

        public Vector(int size)
        {
            arr = new List<T>();
            arr.Capacity = size;

            for( int i=0; i < size;++i)
            {
                arr.Add(default(T));
            }

            num = size;
        }
        public Vector()
        {
            arr = new List<T>();
            num = 0;
        }
        public Vector(List<T> other)
        {
            int size = other.Count;
            num = 0;
            arr = new List<T>();

            for( int i=0; i < size; ++i)
            {
                this.push_back(other[i]);
            }
        }

        public Vector(Vector<T> other)
        {
            num = other.size();

            arr = new List<T>(other.arr);
        }


        public void push_back(T val)
        {
            arr.Add(val); //
            num++;
        }
        public T back()
        {
            return arr.Last(); //
        }
        public void pop_back()
        {
            arr.RemoveAt(num - 1);
            num--;
        }

        public void clear()
        {
            arr.Clear();
            num = 0;
        }
        // [] ?
        public void set(int idx, T val) { arr[idx] = val; } //
        public T get(int idx) { return arr[idx];  } //

        public int size() { return this.num; }
        public bool empty() { return this.num == 0; }

        public void insert(int idx, T val)
        {
            arr.Insert(idx, val);
            num++;
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

        public void sort()
        {
            arr.Sort();
        }
    }
}
