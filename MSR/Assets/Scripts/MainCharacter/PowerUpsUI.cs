using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpsUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    private AttributesControler atributesScript;

    // Start is called before the first frame update
    void Start()
    {
        GameObject aux = GameObject.Find("GameManager");
        atributesScript = aux.GetComponent<AttributesControler>();
        text.text = atributesScript.playerDamageMultiplier.ToString("0.00");

    }

    // Update is called once per frame
    void Update()
    {
        text.text = atributesScript.playerDamageMultiplier.ToString("0.00");
    }
}
