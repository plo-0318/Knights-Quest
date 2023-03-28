using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//Import Text Mesh Pro
using TMPro;

public class TakeDamage : MonoBehaviour
{
    public GameObject damageTextPrefab, enemyInstance;
    public float damageTaken;

    // Update is called once per frame
    void Start()
    {
            string strDamageTaken = damageTaken.ToString();
            GameObject DamageText = Instantiate(damageTextPrefab, enemyInstance.transform);
            DamageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(strDamageTaken);
    }
    
}
