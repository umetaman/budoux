using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BudouX
{
    public static class BudouXInitializer
    {
        private static Parser defaultParser = null;

        public static Parser DefaultParser
        {
            get
            {
                if (defaultParser == null)
                {
                    defaultParser = Parser.LoadDefaultJapanese();
                }
                return defaultParser;
            }
        }
    }
}