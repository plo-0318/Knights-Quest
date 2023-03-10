using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TakeDamageNew : MonoBehaviour
{
    [SerializeField]
    private GameObject damageTextPrefab;

    public void Hurt(float damage)
    {
        // string damageStr = Mathf.Round(damage).ToString();

        // Generate a random damage, for testing only
        string damageStr = Random.Range(100, 1000).ToString();

        GameObject damageText = Instantiate(
            damageTextPrefab,
            transform.position,
            Quaternion.identity
        );

        damageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damageStr);
    }
}
