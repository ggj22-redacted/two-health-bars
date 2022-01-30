using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopupManager : MonoBehaviour
{
    [SerializeField]
    private TextPopup textPopupPrefab;

    private bool spawnable = true;

    private float timeout = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnTextPopup() {
        TextPopup textPopup = Instantiate(textPopupPrefab, Vector3.zero, Quaternion.identity);
        textPopup.transform.SetParent(transform, false);
        textPopup.gameObject.layer = LayerMask.NameToLayer("UI");
        textPopup.transform.localPosition = new Vector3(150f, -200f, 0f);
        textPopup.Setup("testStat", Random.Range(-1f,1f));
        yield return new WaitForSeconds(timeout);
        Destroy(textPopup.gameObject);
    }
}
