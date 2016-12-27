using System;
using System.Collections.Generic;

using System.IO;
using ClauTextSharp.wiz;

namespace ClauTextSharp.load_data
{
    public class Reserver
    {
        public Reserver() {  }
        public virtual int Num() { return 0; }
        public virtual bool end() { return true; }
        public virtual bool functor(MovableDeck<String> strVec)
        {
            return false;
        }
        public virtual bool IsFromFile()
        {
            return false;
        }
        public virtual bool IsFromNone() // from String?
        {
            return false;
        }
    }
    public class InFileReserver : Reserver
    {
        private TextReader tr;
        private bool _end = false;
        private static readonly int thread_num = 4; // 1, 2, 4...
        private Vector<String> strVecTemp = new Vector<String>(1024 << 5);
        private MovableDeck<String>[] MovableDeck = new MovableDeck<String>[thread_num];

        public int num;
        public override int Num() { return num; }
        public void SetNum(int val) { this.num = val; }
        public InFileReserver(TextReader inFile)
        {
            num = 1;
            tr = inFile;

            for (int i = 0; i < thread_num; ++i)
            {
                MovableDeck[i] = new MovableDeck<String>(); // chk..
            }
        }
        public override bool end() { return _end; } 

        // need to rename?
        public override bool functor (MovableDeck<String> strVec)
		{
			return Utility.Reserve2(tr, strVec, num, thread_num, strVecTemp, MovableDeck, ref _end).second > 0;
		}

        public override bool IsFromFile()
        {
            return true;
        }
        public override bool IsFromNone() // from String?
        {
            return false;
        }
    }
    class NoneReserver : Reserver
    {
        private int count;
        public NoneReserver()  { count = 0; }

        public override int Num() { return count; }

        public override bool functor (MovableDeck<String> strVec)
		{
			count = 1;
			return false;
	   	}
        public override bool end() { return 1 == count; }

        public override bool IsFromFile()
        {
            return false;
        }
        public override bool IsFromNone() // from String?
        {
            return true;
        }
    }
}
