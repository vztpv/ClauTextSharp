using System;
using System.Collections.Generic;

namespace ClauTextSharp.wiz
{
    public class Pair<T, U>
    {
        public T first;
        public U second;

        public Pair() {
            first = default(T);
            second = default(U);
        }

        public Pair(T first, U second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
