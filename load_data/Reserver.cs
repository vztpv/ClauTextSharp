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
        public virtual bool functor(ArrayQueue<String> strVec)
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
        private StreamReader sr;

        public int num;
        public override int Num() { return num; }
        public void SetNum(int val) { this.num = val; }
        public InFileReserver(StreamReader inFile)
        {
            num = 1;
            sr = inFile;
        }
        public override bool end() { return sr.EndOfStream; } 

        // need to rename?
        public override bool functor (ArrayQueue<String> strVec)
		{
			return Utility.Reserve2(sr, strVec, num).second > 0;
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

        public override bool functor (ArrayQueue<String> strVec)
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
