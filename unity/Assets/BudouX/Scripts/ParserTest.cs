using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BudouX;
using TMPro;

public class ParserTest : MonoBehaviour
{
    [SerializeField]
    private TextAsset modelJson;
    [SerializeField]
    private TextAsset text;
    [SerializeField]
    private TextMeshProUGUI tmpro;

    private Parser parser;

    private void Start()
    {
        parser = Parser.LoadFrom(modelJson.text);

        var result = parser.Parse(text.text);

        foreach (var token in result)
        {
            Debug.Log(token);
        }

        var stopWatch = new System.Diagnostics.Stopwatch();

        stopWatch.Start();
        tmpro.SetTextWithBudouX(text.text);
        stopWatch.Stop();

        Debug.Log($"Time: {stopWatch.ElapsedMilliseconds}ms");
    }
}
