using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JwDeveloper
{

    public class UtilityFunction
    {
        static public string FirstLetterUppercase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}