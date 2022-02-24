using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Common.Areas;

public class TextPopupManager : MonoBehaviour
{

    [SerializeField]
    private GameObject StatUpCanvasPrefab;

    [Inject]
    private AreaSystem areaSystem;

    private float timeout = 4f;
    // Start is called before the first frame update
    void Start()
    {
        areaSystem.OnStatUpdated += OnStatUpdated;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnStatUpdated(Stat stat, float amount) {
        if (stat != Stat.None)
            StartCoroutine(SpawnTextPopup(stat, amount));
    }

    IEnumerator SpawnTextPopup(Stat stat, float amount) {
        GameObject statUp = Instantiate(StatUpCanvasPrefab, Vector3.zero, Quaternion.identity);
        StatPopUp newPopUp = statUp.GetComponent<StatPopUp>();
        statUp.transform.SetParent(transform, false);
        statUp.gameObject.layer = LayerMask.NameToLayer("UI");
        statUp.transform.localPosition = new Vector3(50f, -100f, 0f);
        newPopUp.timeOut = timeout;
        newPopUp.statText = stat.ToString();
        newPopUp.value = amount;
        yield return new WaitForSeconds(timeout);
        Destroy(statUp.gameObject);
    }
}
