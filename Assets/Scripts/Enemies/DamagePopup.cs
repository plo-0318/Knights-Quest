using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private GameObject damageTextPrefab;

    //How much the text moves up
    public float moveUp = 1.0f;

    //How far out it goes
    public float amplitude = 0.15f;

    //How fast it goes side to side
    public float frequency = 1.5f;

    //How much it rotates left and right
    public float rotationAmount = 5f;

    //How fast the object moves up
    public float moveSpeed = 2f;

    //How long it takes to fade in the text
    public float fadeInTime = .25f;

    //When to destroy the object
    //   public float whenDestroy = 1.0f;

    private void Start()
    {
        damageTextPrefab = GameManager.damagePopupText;
    }

    public void ShowDamagePopup(float damage)
    {
        // string damageStr = Mathf.Round(damage).ToString();

        // Generate a random damage, for testing only
        // string damageStr = Random.Range(100, 1000).ToString();
        string damageStr = damage.ToString();

        //Generate random movements for the rotation amount
        int chooseSign = Random.Range(0, 2);
        if (chooseSign == 1)
        {
            rotationAmount = rotationAmount * -1;
        }

        GameObject damageText = Instantiate(
            damageTextPrefab,
            transform.position,
            Quaternion.identity
        );

        damageText.transform.parent = GameManager.GameSession().damagePopupParent;

        damageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damageStr);

        //Functions for moving and rotating text
        StartCoroutine(MoveTextUpAndBounce(damageText, moveSpeed, amplitude, frequency));
        // StartCoroutine(RotateText(damageText, rotationAmount));

        //Accessing TextMeshPro of child and calling the fading in function
        TextMeshPro textrenderer = damageText.transform.GetChild(0).GetComponent<TextMeshPro>();
        StartCoroutine(fadeIn(textrenderer, fadeInTime));
    }

    private IEnumerator MoveTextUpAndBounce(
        GameObject obj,
        float speed,
        float amplitude,
        float frequency
    )
    {
        float elapsedTime = 0.0f;
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * moveUp;

        while (elapsedTime < 1.0f)
        {
            float x = Mathf.PingPong(elapsedTime * frequency, 1.0f) * amplitude;
            Vector3 offset = new Vector3(x, 0.0f, 0.0f);
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime) + offset;
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }
        obj.transform.position = endPosition;
        Destroy(obj);
    }

    private IEnumerator RotateText(GameObject obj, float rotation)
    {
        float elapsedTime = 0.0f;
        Quaternion startRotation = obj.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(Vector3.forward * rotation);

        while (elapsedTime < 1.0f)
        {
            obj.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.rotation = endRotation;
    }

    private IEnumerator fadeIn(TextMeshPro textrenderer, float fadeInTime)
    {
        Debug.Log("Opacity should be changing");
        float elapsedTime = 0.0f;
        float startAlpha = 0.0f;
        float targetAlpha = 1.0f;

        while (elapsedTime < fadeInTime)
        {
            float t = elapsedTime / fadeInTime;
            // Debug.Log(textrenderer.alpha);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            textrenderer.alpha = alpha;
            ;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textrenderer.alpha = targetAlpha;
    }
}
