using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
struct AnimatedObject
{
    public GameObject gameObject;
    public GameObject posIn;
    public GameObject posOut;
    public float delay;
}

public class InAndOutAnimation : MonoBehaviour
{
    [SerializeField]
    private AnimatedObject[] objects;

    [SerializeField]
    private bool startPosIn;

    void Start()
    {
        foreach (var obj in objects)
        {
            // Setup the initial positions when the scene starts
            obj.gameObject.transform.position = startPosIn
                ? obj.posIn.transform.position
                : obj.posOut.transform.position;
        }
    }

    public void MoveInAnimation(float startOffset = 0f)
    {
        foreach (var obj in objects)
        {
            var objButton = obj.gameObject.GetComponent<Button>();

            if (objButton)
            {
                objButton.interactable = true;
            }

            MoveAnimation(obj.gameObject, obj.posIn.transform.position, obj.delay + startOffset);
        }
    }

    public void MoveOutAnimation(float startOffset = 0f, Action callback = null)
    {
        // foreach (var obj in objects)
        // {
        //     var objButton = obj.gameObject.GetComponent<Button>();

        //     if (objButton)
        //     {
        //         objButton.interactable = false;
        //     }

        //     MoveAnimation(
        //         obj.gameObject,
        //         obj.posOut.transform.position,
        //         obj.delay + startOffset,
        //     );
        // }

        // Only execute the callback function when the last object finishes it animation
        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];

            var objButton = obj.gameObject.GetComponent<Button>();

            if (objButton)
            {
                objButton.interactable = false;
            }

            Action _callback = i == objects.Length - 1 ? callback : null;

            MoveAnimation(
                obj.gameObject,
                obj.posOut.transform.position,
                obj.delay + startOffset,
                _callback
            );
        }
    }

    // Tween Move Animation
    private void MoveAnimation(
        GameObject obj,
        Vector3 newPos,
        float delay = 0f,
        Action callback = null
    )
    {
        LeanTween.cancel(obj);
        LeanTween
            .move(obj, newPos, 0.5f)
            .setEaseOutExpo()
            .setDelay(delay)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
    }
}
