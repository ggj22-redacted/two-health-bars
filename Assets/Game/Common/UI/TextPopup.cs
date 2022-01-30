using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textMesh;

    public void Setup(string statName, float amount) {
        textMesh.SetText(statName + (amount > 0 ? " + " : " ") + amount);
    }

    public void Update() {
        float moveYSpeed = 80f;
        float fadeSpeed = 0.6f;
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, textMesh.color.a - (fadeSpeed * Time.deltaTime));
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
    }
}
