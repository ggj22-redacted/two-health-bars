using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCanvasControl : MonoBehaviour
{

    private CanvasGroup groupCanvas;

    // Start is called before the first frame update
    void Start()
    {
        groupCanvas = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            groupCanvas.interactable = true;
            groupCanvas.alpha = 1;
        }
        else
        {
            groupCanvas.interactable = false;
            groupCanvas.alpha = 0;
        }
    }
}
