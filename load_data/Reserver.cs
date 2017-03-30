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
        public virtual bool Functor(Deck<Token> strVec)
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

        private int num;

        public override int Num() { return num; }
        public void SetNum(int val) { this.num = val; }
        public InFileReserver(TextReader inFile)
        {
            num = 1;
            tr = inFile;
        }
        public override bool end() { return _end; } 

        // need to rename?
        public override bool Functor(Deck<Token> strVec)
		{
			return Utility.Reserve2(tr, strVec, num, ref _end).second > 0;
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
    public class NoneReserver : Reserver
    {
        private int count;
        public NoneReserver()  { count = 0; }

        public override int Num() { return count; }

        public override bool Functor(Deck<Token> strVec)
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
