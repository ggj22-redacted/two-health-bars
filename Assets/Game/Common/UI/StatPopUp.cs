using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPopUp : MonoBehaviour
{
    public float timeOut;
    public string statText;
    public float value;
    public Sprite[] statIcons;
    public Sprite[] modIcons;
    private float timeOutPos;
    private float time;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float startAlpha;
    private float endAlpha;
    private CanvasGroup canvas;
    private TMPro.TextMeshProUGUI textChild;
    private Image[] imgComponents;
    private Color32 upColor = new Color32(0,255,0,255);
    private Color32 downColor = new Color32(255, 0, 0, 255);

    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponent<CanvasGroup>();
        textChild = transform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        imgComponents = transform.GetComponentsInChildren<Image>();
        //textChild.text = statText;
        startPosition = transform.localPosition;
        endPosition = transform.localPosition + new Vector3(0, 150, 0);
        timeOutPos = timeOut / 2;
        timeOut = timeOut / 3;
        startAlpha = canvas.alpha;
        endAlpha = 0;
        SetValue(value, statText, imgComponents[1]);
        SetIcon(statText, imgComponents[0]);
        
    }

    void SetIcon(string stat, Image icon)
    {
        switch (stat)
        {
            case "Shield":
                icon.sprite = statIcons[0];
                textChild.text = stat;
                break;
            case "Speed":
                icon.sprite = statIcons[1];
                textChild.text = stat;
                break;
            case "Gravity": //remove gravity mod
            case "JumpHeight":
                icon.sprite = statIcons[2];
                textChild.text = "Booster Pack";
                break;
            case "ProjectileDamage":
                icon.sprite = statIcons[3];
                textChild.text = "Projectile Damage";
                break;
            case "ProjectileFireRate":
                icon.sprite = statIcons[4];
                textChild.text = "Projectile Fire Rate";
                break;
            case "ProjectileSpeed":
                icon.sprite = statIcons[5];
                textChild.text = "Projectile Speed";
                break;
            case "ProjectileSize":
                icon.sprite = statIcons[6];
                textChild.text = "Projectile Size";
                break;
            case "ProjectileRange":
                icon.sprite = statIcons[7];
                textChild.text = "Projectile Range";
                break;
            case "ProjectileSpread":
                icon.sprite = statIcons[8];
                textChild.text = "Projectile Spread";
                break;
            case "ProjectileCount":
                icon.sprite = statIcons[8];
                textChild.text = "Projectile Number";
                break;
        }
    }

    void SetValue(float value, string stat, Image icon)
    {
        if (value >= 0)
        {
            icon.sprite = modIcons[0];
            textChild.color = upColor;
            icon.color = upColor;
        }
        else
        {
            icon.sprite = modIcons[1];
            textChild.color = downColor;
            icon.color = downColor;
        }
    }

    void MovementUp()
    {
        if (time < timeOutPos)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, time / timeOutPos);
            canvas.alpha = Mathf.Lerp(startAlpha, endAlpha, time/timeOut);
            time += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementUp();
    }
}
