using System;
using System.Collections.Generic;

namespace ClauTextSharp.wiz
{
    public class StringUtility
    {
        public static bool Comp(String str, int start, String changed_str, int changed_str_len)
        {
            for (int i = 0; i < changed_str_len; ++i)
            {
                if (str[start + i] != changed_str[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool startsWith(String str, String start)
        {
            if (str.Length < start.Length) { return false; }
            return Comp(str, 0, start, start.Length);
        }
        public static bool endsWith(String str, String last)
        {
            if (str.Length < last.Length) { return false; }
            if (last.Length == 0) { return true; } /// chk... return false; 
            return Comp(str, str.Length - last.Length, last, last.Length);
        }
        public static String subString(String str, int start, int last)
        {
            return str.Substring(start, last - start + 1);
        }
        public static String subString(String str, int start)
        {
            return subString(str, start, str.Length - 1);
        }
        public static String replace(String str, String target, String result)
        {
            return str.Replace(target, result);
        }
        public static String reverse(String str) // chk!
        {
            char[] strArr = str.ToCharArray();
            Array.Reverse(strArr);
            return new String(strArr);
        }
    }
}
