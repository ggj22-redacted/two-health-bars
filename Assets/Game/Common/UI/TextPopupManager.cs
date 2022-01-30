using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Common.Areas;

public class TextPopupManager : MonoBehaviour
{
    [SerializeField]
    private TextPopup textPopupPrefab;


    [Inject]
    private AreaSystem areaSystem;

    private float timeout = 2f;
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
        StartCoroutine(SpawnTextPopup(stat, amount));
    }

    IEnumerator SpawnTextPopup(Stat stat, float amount) {
        TextPopup textPopup = Instantiate(textPopupPrefab, Vector3.zero, Quaternion.identity);
        textPopup.transform.SetParent(transform, false);
        textPopup.gameObject.layer = LayerMask.NameToLayer("UI");
        textPopup.transform.localPosition = new Vector3(150f, -200f, 0f);
        textPopup.Setup(stat.ToString(), amount);
        yield return new WaitForSeconds(timeout);
        Destroy(textPopup.gameObject);
    }
}
