using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIndicatorImage : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    public void SetItemIcon(Sprite sprite)
    {
        itemIcon.sprite = sprite;
    }
}
