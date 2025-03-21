using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BudouX
{
    public class Parser
    {
        private readonly Dictionary<string, Dictionary<string, int>> model;
        public static readonly List<string> Empty = new List<string>();

        public const string UW1 = "UW1";
        public const string UW2 = "UW2";
        public const string UW3 = "UW3";
        public const string UW4 = "UW4";
        public const string UW5 = "UW5";
        public const string UW6 = "UW6";
        public const string BW1 = "BW1";
        public const string BW2 = "BW2";
        public const string BW3 = "BW3";
        public const string TW1 = "TW1";
        public const string TW2 = "TW2";
        public const string TW3 = "TW3";
        public const string TW4 = "TW4";
        public const string ZWSP = "<zwsp>";
        public const string NOBREAK_ = "<nobr>";
        public const string _NOBREAK = "</nobr>";

        public Parser(Dictionary<string, Dictionary<string, int>> model)
        {
            this.model = model;

            foreach(var (k, v) in model)
            {
                foreach (var (kk, vv) in v)
                {
                    Debug.Log($"{k} {kk} {vv}");
                }
            }
        }

        public static Parser LoadDefaultJapanese()
        {
            var json = Resources.Load<TextAsset>("ja").text;
            var model = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(json);
            return new Parser(model);
        }

        public static Parser LoadFrom(string modelJson)
        {
            Debug.Log(modelJson);
            var model = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(modelJson);
            return new Parser(model);
        }

        private int GetScore(string key, string sequence)
        {
            model.TryGetValue(key, out var dict);
            if (dict == null)
            {
                return 0;
            }

            dict.TryGetValue(sequence, out var score);
            return score;
        }

        private int Evaluate(string key, string sentence, int begin, int end)
        {
            return 2 * this.GetScore(key, sentence.Substring(begin, end - begin));
        }

        public string ParseToTMProString(string sentence)
        {
            if (string.IsNullOrEmpty(sentence) || string.IsNullOrWhiteSpace(sentence))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            builder.Append(NOBREAK_);
            builder.Append(sentence[0]);

            int total = model.Values.SelectMany(x => x.Values).Sum();

            for (int i = 1; i < sentence.Length; i++)
            {
                int score = -total;

                if (i - 2 > 0)
                {
                    score += this.Evaluate(UW1, sentence, i - 3, i - 2);
                }
                if (i - 1 > 0)
                {
                    score += this.Evaluate(UW2, sentence, i - 2, i - 1);
                }
                score += this.Evaluate(UW3, sentence, i - 1, i);
                score += this.Evaluate(UW4, sentence, i, i + 1);

                if (i + 1 < sentence.Length)
                {
                    score += this.Evaluate(UW5, sentence, i + 1, i + 2);
                }
                if (i + 2 < sentence.Length)
                {
                    score += this.Evaluate(UW6, sentence, i + 2, i + 3);
                }

                if (i > 1)
                {
                    score += this.Evaluate(BW1, sentence, i - 2, i);
                }
                score += this.Evaluate(BW2, sentence, i - 1, i + 1);
                if (i + 1 < sentence.Length)
                {
                    score += this.Evaluate(BW3, sentence, i, i + 2);
                }

                if (i - 2 > 0)
                {
                    score += this.Evaluate(TW1, sentence, i - 3, i);
                }
                if (i - 1 > 0)
                {
                    score += this.Evaluate(TW2, sentence, i - 2, i + 1);
                }
                if (i + 1 < sentence.Length)
                {
                    score += this.Evaluate(TW3, sentence, i - 1, i + 2);
                }
                if (i + 2 < sentence.Length)
                {
                    score += this.Evaluate(TW4, sentence, i, i + 3);
                }

                if (score > 0)
                {
                    builder.Append(_NOBREAK);
                    builder.Append(ZWSP);
                    builder.Append(NOBREAK_);
                    builder.Append("");
                }

               builder.Append(sentence[i]);
            }

            return builder.ToString();
        }

        public IEnumerable<string> Parse(string sentence)
        {
            if (string.IsNullOrEmpty(sentence) || string.IsNullOrWhiteSpace(sentence))
            {
                return Empty;
            }

            var result = new List<string>();

            result.Add(sentence[0].ToString());

            int total = model.Values.SelectMany(x => x.Values).Sum();

            for (int i = 1; i < sentence.Length; i++)
            {
                int score = -total;

                if(i - 2 > 0)
                {
                    score += this.Evaluate(UW1, sentence, i - 3, i - 2);
                }
                if(i - 1 > 0)
                {
                    score += this.Evaluate(UW2, sentence, i - 2, i - 1);
                }
                score += this.Evaluate(UW3, sentence, i - 1, i);
                score += this.Evaluate(UW4, sentence, i, i + 1);

                if (i + 1 < sentence.Length)
                {
                    score += this.Evaluate(UW5, sentence, i + 1, i + 2);
                }
                if (i + 2 < sentence.Length)
                {
                    score += this.Evaluate(UW6, sentence, i + 2, i + 3);
                }

                if(i > 1)
                {
                    score += this.Evaluate(BW1, sentence, i - 2, i);
                }
                score += this.Evaluate(BW2, sentence, i - 1, i + 1);
                if (i + 1 < sentence.Length)
                {
                    score += this.Evaluate(BW3, sentence, i, i + 2);
                }

                if(i - 2 > 0)
                {
                    score += this.Evaluate(TW1, sentence, i - 3, i);
                }
                if (i - 1 > 0)
                {
                    score += this.Evaluate(TW2, sentence, i - 2, i + 1);
                }
                if(i + 1 < sentence.Length)
                {
                    score += this.Evaluate(TW3, sentence, i - 1, i + 2);
                }
                if (i + 2 < sentence.Length)
                {
                    score += this.Evaluate(TW4, sentence, i, i + 3);
                }

                if(score > 0)
                {
                    result.Add("");
                }

                result[result.Count - 1] += sentence[i];
            }

            return result;
        }
    }
}