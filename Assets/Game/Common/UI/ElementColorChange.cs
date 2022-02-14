using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementColorChange : MonoBehaviour
{

    public Color32 highlightColor;
    public Color32 normalColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ImgColorSelect(Image img)
    {
        img.color = highlightColor;
    }

    public void TextColorSelect(TMP_Text text)
    {
        text.color = highlightColor;
    }

    public void ImgColorDeselect(Image img)
    {
        img.color = normalColor;
    }

    public void TextColorDeselect(TMP_Text text)
    {
        text.color = normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
