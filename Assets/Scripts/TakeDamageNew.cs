using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TakeDamageNew : MonoBehaviour
{
    [SerializeField]
    private GameObject damageTextPrefab;
    public float moveUp = 1.0f;
    public float rotationAmount = 1.0f;
    public float opacityTime = 1.0f;
    public float moveSpeed =  1.0f;
    public float whenDestroy = 1.0f;

    public void Hurt(float damage)
    {
        // string damageStr = Mathf.Round(damage).ToString();

        // Generate a random damage, for testing only
        string damageStr = Random.Range(100, 1000).ToString();

        //Generate random movements for the rotation amount
        int chooseSign = Random.Range(0, 2);
        if (chooseSign == 1 ) {
            rotationAmount = rotationAmount * -1;
        }

        CanvasRenderer canvasRenderer = GetComponentInChildren<CanvasRenderer>();


        GameObject damageText = Instantiate(
            damageTextPrefab,
            transform.position,
            Quaternion.identity
        );

        damageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damageStr);

        StartCoroutine(MoveTextUp(damageText, moveSpeed));
        StartCoroutine(RotateText(damageText, rotationAmount));

        Destroy(damageText, whenDestroy);

    }
    
    private IEnumerator MoveTextUp(GameObject obj, float speed) {
        float elapsedTime = 0.0f;
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * moveUp;
        while (elapsedTime < 1.0f) {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }
        obj.transform.position = endPosition;
    }

    private IEnumerator RotateText(GameObject obj, float rotation) {
        float elapsedTime = 0.0f;
        Quaternion startRotation = obj.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(Vector3.forward * rotation);

        while (elapsedTime < 1.0f) {
            obj.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.rotation = endRotation;

    }

    private IEnumerator opacityChange(CanvasRenderer canvasRenderer, float opacityTime){
        float elapsedTime = 0.0f;
        Color color = canvasRenderer.GetColor();
        color.a = 0.0f;
        canvasRenderer.SetColor(color);

        while (elapsedTime < opacityTime) {
            float alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / opacityTime);
            color.a = alpha;
            canvasRenderer.SetColor(color);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1.0f;
        canvasRenderer.SetColor(color);
    }
}
