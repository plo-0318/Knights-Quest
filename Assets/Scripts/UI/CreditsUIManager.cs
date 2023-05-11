using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUIManager : MonoBehaviour
{
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private RectTransform namesList;

    [SerializeField]
    private float scrollDuration = 10f;

    private void Start()
    {
        backButton.onClick.AddListener(HandleBackButtonClick);

        StartCoroutine(Scroll());
    }

    private void HandleBackButtonClick()
    {
        GameManager.LoadScene("Main Menu", true);
    }

    private IEnumerator Scroll()
    {
        float elapsedTime = 0;
        float initialY = namesList.localPosition.y;
        float targetY = 3100f;

        while (elapsedTime < scrollDuration)
        {
            float newY = Mathf.Lerp(initialY, targetY, elapsedTime / scrollDuration);

            namesList.localPosition = new Vector3(0f, newY, 0f);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        namesList.localPosition = new Vector3(0f, targetY, 0f);
    }
}
