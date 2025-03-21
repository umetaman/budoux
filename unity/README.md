# BudouX Unity module

BudouX is a standalone, small, and language-neutral phrase segmenter tool that
provides beautiful and legible line breaks.

For more details about the project, please refer to the [project README](https://github.com/google/budoux/).

![capture]("./capture.gif")

### Simple usage

You can get a list of phrases by feeding a sentence to the parser.
The easiest way is to get a parser is loading the default parser for each language.

```cs
using TMPro;
using BudouX;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        // Load Japanese Model
        var model = Resources.Load<TextAsset>("ja");
        // Create Parser
        var parser = new Parser(model);
        
        List<string> parsedStrings = parser.Parse("今日はいい天気ですね。");
        // [今日は, 良い, 天気ですね。]
    }
}
```

### Working with TextMeshPro

If you want to use the result in TextMeshPro, you can use the `SetTextWithBudouX` method to set a rich tag text that wraps phrases with non-breaking markup, speicifcally `<nobr></nobr>` and `<zwsp>`.

```cs
public class Sample : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private void Start()
    {
        // Load Japanese Model
        var model = Resources.Load<TextAsset>("ja");
        // Create Parser
        var parser = new Parser(model);
        

        // Set Text Directly
        textMesh.SetTextWithBudouX(parser, "今日はいい天気ですね。")
        // Use Default Parser
        textMesh.SetTextWithBudouX("今日はいい天気ですね。")
    }
}
```

## Disclaimer

This is not an officially supported Google product.