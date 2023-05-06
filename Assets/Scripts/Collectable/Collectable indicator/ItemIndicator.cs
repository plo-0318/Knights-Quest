using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIndicator : MonoBehaviour
{
    private Camera mainCamera;
    private Transform player;

    private static GameObject indicatorPrefab;

    [SerializeField]
    private Sprite indicatorIcon;
    private GameObject indicator;
    private float indicatorOffset;

    private bool showIndicator;

    private static HashSet<ItemIndicator> itemIndicators;

    static ItemIndicator()
    {
        itemIndicators = new HashSet<ItemIndicator>();
    }

    private void Awake()
    {
        if (indicatorPrefab == null)
        {
            indicatorPrefab = Resources.Load<GameObject>("collectables/item indicator image");
        }

        indicatorOffset = 50f;

        itemIndicators.Add(this);
    }

    private void Start()
    {
        player = GameManager.PlayerMovement().transform;
        mainCamera = Camera.main;

        showIndicator = GameManager.ItemIndicatorManager().ShowIndicators;
    }

    private void FixedUpdate()
    {
        if (showIndicator)
        {
            DisplayIndicator();
        }
    }

    private void OnDestroy()
    {
        itemIndicators.Remove(this);

        if (indicator != null)
        {
            Destroy(indicator);
        }
    }

    public void Show()
    {
        showIndicator = true;
    }

    public void Hide()
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
        }

        showIndicator = false;
    }

    private void DisplayIndicator()
    {
        // Check if the item is visible on the camera
        bool isItemVisible = IsObjectVisible();

        if (!isItemVisible)
        {
            // Calculate the direction between the player and the item
            Vector3 direction = transform.position - player.position;

            // Convert the direction to a screen space position
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);

            // Calculate the indicator position at the border of the screen
            Vector3 indicatorPosition = CalculateIndicatorPosition(
                screenPosition,
                mainCamera.pixelWidth,
                mainCamera.pixelHeight,
                indicatorOffset
            );

            if (indicator == null)
            {
                // Create the indicator if it doesn't exist
                indicator = Instantiate(
                    indicatorPrefab,
                    transform.position,
                    Quaternion.identity,
                    GameManager.ItemIndicatorManager().transform
                );

                indicator.GetComponent<ItemIndicatorImage>().SetItemIcon(indicatorIcon);
            }

            // Position the indicator at the calculated position
            indicator.transform.position = indicatorPosition;
            indicator.SetActive(true);
        }
        // Item is not visible, and there is an indicator
        else if (indicator != null)
        {
            // Hide or disable the indicator if the item is visible
            indicator.SetActive(false);
        }
    }

    private bool IsObjectVisible()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is inside the viewport bounds
        return viewportPosition.x >= 0
            && viewportPosition.x <= 1
            && viewportPosition.y >= 0
            && viewportPosition.y <= 1;
    }

    private Vector3 CalculateIndicatorPosition(
        Vector3 screenPosition,
        float screenWidth,
        float screenHeight,
        float offset
    )
    {
        // Determine the indicator's position at the border of the screen with an offset
        float x = Mathf.Clamp(screenPosition.x, 0f, screenWidth);
        float y = Mathf.Clamp(screenPosition.y, 0f, screenHeight);

        // Adjust the position based on the screen edges with an offset
        if (screenPosition.x <= 0)
            x = offset;
        else if (screenPosition.x >= screenWidth)
            x = screenWidth - offset;

        if (screenPosition.y <= 0)
            y = offset;
        else if (screenPosition.y >= screenHeight)
            y = screenHeight - offset;

        // Return the indicator position
        return new Vector3(x, y, 0f);
    }

    public static void ShowAllIndicators()
    {
        foreach (ItemIndicator item in itemIndicators)
        {
            item.Show();
        }
    }

    public static void HideAllIndicators()
    {
        foreach (ItemIndicator item in itemIndicators)
        {
            item.Hide();
        }
    }
}
