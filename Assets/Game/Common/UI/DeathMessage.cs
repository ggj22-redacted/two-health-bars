using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class DeathMessage : MonoBehaviour
{
    [Inject]
    EntityState playerState;

    //playerState.OnDied+=  

    private string messageToShow;
    private CanvasGroup screenCanvasGroup;
    private bool activeScreen;
    private float timeElapsed;
    public float timeToShow;
    public TMPro.TextMeshProUGUI textMessage;
    public CanvasGroup[] buttonCanvasGroup;

    private void Awake()
    {
        screenCanvasGroup = gameObject.GetComponent<CanvasGroup>();
        activeScreen = false;
        textMessage = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        buttonCanvasGroup = gameObject.GetComponentsInChildren<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        playerState.OnDied += ActivateScreen;
        playerState.OnRespawned += DeActivateScreen;
    }

    private void OnDisable()
    {
        playerState.OnDied -= ActivateScreen;
    }

    string ChooseMessage()
    {
        string message = "";

        return message;
    }

    void DeActivateScreen(EntityState state)
    {
        screenCanvasGroup.alpha = 0;
        for (int x= 0; x < buttonCanvasGroup.Length; x++)
        {
            buttonCanvasGroup[x].alpha = 0;
            buttonCanvasGroup[x].interactable = false;
        }
    }

    void ActivateScreen(EntityState state)
    {
        activeScreen = true;
        StartCoroutine(ShowButtons(timeToShow));
    }

    IEnumerator ShowButtons(float time)
    {
        yield return new WaitForSeconds(time);
        for (int x = 0; x < buttonCanvasGroup.Length; x++)
        {
            buttonCanvasGroup[x].alpha = 1;
            buttonCanvasGroup[x].interactable = true;
        }
    }

    void ShowCanvas()
    {
        if (screenCanvasGroup.alpha < 1)
        {
            screenCanvasGroup.alpha = (Mathf.Lerp(0f, 1f, timeElapsed/timeToShow));
            timeElapsed += Time.deltaTime;

        }
        else
        {
            activeScreen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeScreen)
        {
            ShowCanvas();
        }
    }
}
