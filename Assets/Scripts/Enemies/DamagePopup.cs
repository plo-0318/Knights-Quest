using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup
{
    private static GameSession gameSession = null;
    private static GameObject damageTextPrefab = null;
    private static GameObject healTextPrefab = null;

    private static bool SetReferences()
    {
        gameSession = GameManager.GameSession();
        damageTextPrefab = GameManager.DamagePopupText;
        healTextPrefab = GameManager.HealPopupText;

        return gameSession != null && damageTextPrefab != null && healTextPrefab != null;
    }

    private static bool ValidReferences()
    {
        return gameSession != null && damageTextPrefab != null && healTextPrefab != null;
    }

    public static void ShowDamagePopup(
        float damage,
        Transform transform,
        Quaternion quaternion,
        DamagePopupOptions options = null
    )
    {
        ShowPopup(true, damage, transform, quaternion, null, options);
    }

    public static void ShowHealPopup(
        float damage,
        Transform transform,
        Quaternion quaternion,
        Transform parent,
        DamagePopupOptions options = null
    )
    {
        ShowPopup(false, damage, transform, quaternion, parent, options);
    }

    private static void ShowPopup(
        bool isDamage,
        float damage,
        Transform transform,
        Quaternion quaternion,
        Transform parent = null,
        DamagePopupOptions options = null
    )
    {
        // Check if game session and damage text prefab references are set
        // If not, set their references
        // If unsuccessful, abort
        if (!ValidReferences())
        {
            if (!SetReferences())
            {
                return;
            }
        }

        GameObject popupText = isDamage ? damageTextPrefab : healTextPrefab;

        // If no damage popup options are passed in, use the default one
        options = options == null ? DamagePopupOptions.DEFAULT : options;

        // Round the damage number to the nearest integer
        string damageStr = Mathf.RoundToInt(damage).ToString();

        //Generate random movements for the rotation amount
        // int chooseSign = Random.Range(0, 2);
        // if (chooseSign == 1)
        // {
        //     rotationAmount = rotationAmount * -1;
        // }

        Transform parentTrans =
            parent == null ? GameManager.GameSession().damagePopupParent : parent;

        GameObject damageText = GameObject.Instantiate(
            popupText,
            transform.position,
            quaternion,
            parentTrans
        );

        damageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damageStr);

        //Accessing TextMeshPro of child and calling the fading in function
        TextMeshPro textRenderer = damageText.transform.GetChild(0).GetComponent<TextMeshPro>();
        gameSession.StartCoroutine(fadeIn(textRenderer, options));

        //Functions for moving and rotating text
        gameSession.StartCoroutine(MoveTextUpAndBounce(damageText, options));

        // StartCoroutine(RotateText(damageText, rotationAmount));
    }

    private static IEnumerator MoveTextUpAndBounce(GameObject obj, DamagePopupOptions options)
    {
        float elapsedTime = 0.0f;
        // Vector3 startPosition = obj.transform.position;
        // Vector3 endPosition = startPosition + Vector3.up * options.moveUp;

        // while (elapsedTime < 1.0f)
        // {
        //     float x = Mathf.PingPong(elapsedTime * options.frequency, 1.0f) * options.amplitude;
        //     Vector3 offset = new Vector3(x, 0.0f, 0.0f);
        //     obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime) + offset;
        //     elapsedTime += Time.deltaTime * options.moveSpeed;
        //     yield return null;
        // }


        // obj.transform.position = endPosition;
        // GameObject.Destroy(obj.gameObject);

        // Changed the text to have an alive time
        while (elapsedTime < options.aliveTime)
        {
            float x = Mathf.PingPong(Time.time * options.frequency, 1f) * options.amplitude;
            obj.transform.position = Vector3.Lerp(
                obj.transform.position,
                obj.transform.position + Vector3.up + new Vector3(x, 0, 0),
                options.moveSpeed * Time.deltaTime
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameObject.Destroy(obj.gameObject);
    }

    private static IEnumerator RotateText(GameObject obj, float rotation)
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

    private static IEnumerator fadeIn(TextMeshPro textRenderer, DamagePopupOptions options)
    {
        // Commented out logging, changed targetAlpha from 1 to 0.8
        // Debug.Log("Opacity should be changing");
        float elapsedTime = 0.0f;
        float startAlpha = 0.0f;
        float targetAlpha = 0.8f;

        // setting text alpha to 0, so it will actually fade in
        textRenderer.alpha = 0;

        while (elapsedTime < options.fadeInTime)
        {
            float t = elapsedTime / options.fadeInTime;
            // Debug.Log(textRenderer.alpha);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            textRenderer.alpha = alpha;
            ;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textRenderer.alpha = targetAlpha;
    }
}

public class DamagePopupOptions
{
    //How much the text moves up
    // public float moveUp;

    //How far out it goes
    public float amplitude;

    //How fast it goes side to side
    public float frequency;

    //How much it rotates left and right
    public float rotationAmount;

    //How fast the object moves up
    public float moveSpeed;

    //How long it takes to fade in the text
    public float fadeInTime;

    //How long before the text is destroyed
    public float aliveTime;

    public static readonly DamagePopupOptions DEFAULT;

    static DamagePopupOptions()
    {
        DEFAULT = new DamagePopupOptions();
    }

    public DamagePopupOptions(
        float amplitude = .5f,
        float frequency = 4f,
        float rotationAmount = 5f,
        float moveSpeed = 2.5f,
        float fadeInTime = .25f,
        float aliveTime = 0.5f
    )
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.rotationAmount = rotationAmount;
        this.moveSpeed = moveSpeed;
        this.fadeInTime = fadeInTime;
        this.aliveTime = aliveTime;
    }
}
