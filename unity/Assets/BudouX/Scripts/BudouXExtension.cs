using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BudouX
{
    public static class BudouXExtension
    {
        public static void SetTextWithBudouX(this TMP_Text tmpro, Parser parser, string text)
        {
            tmpro.text = parser.ParseToTMProString(text);
        }

        public static void SetTextWithBudouX(this TMP_Text tmpro, string text)
        {
            tmpro.text = BudouXInitializer.DefaultParser.ParseToTMProString(text);
        }
    }
}