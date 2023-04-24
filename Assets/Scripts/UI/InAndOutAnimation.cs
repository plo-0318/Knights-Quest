using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct AnimatedObject 
{
    public GameObject gameObject;
    public GameObject posIn;
    public GameObject posOut;
    public float delay;
}

public class InAndOutAnimation : MonoBehaviour {
    
    [SerializeField] private AnimatedObject[] objects;
    [SerializeField] private bool startPosIn;

    void Start()
    {
        foreach (var obj in objects) 
        {
            // Setup the initial positions when the scene starts 
            obj.gameObject.transform.position = startPosIn ? obj.posIn.transform.position : obj.posOut.transform.position;
            obj.gameObject.SetActive(startPosIn);
        }
    }

    public void MoveInAnimation(float startOffset = 0f) {
        foreach (var obj in objects) 
        {
            obj.gameObject.SetActive(true);
            
            var objButton = obj.gameObject.GetComponent<Button>();

            if (objButton) {
                objButton.interactable = true;
            }
            
            MoveAnimation(obj.gameObject, obj.posIn.transform.position, obj.delay + startOffset);
        }
    }

    public void MoveOutAnimation(float startOffset = 0f) {
        foreach (var obj in objects) {
            var objButton = obj.gameObject.GetComponent<Button>();

            if (objButton) {
                objButton.interactable = false;
            }
            
            MoveAnimation(obj.gameObject, obj.posOut.transform.position, obj.delay + startOffset, true);
        }
    }

    // Tween Move Animation
    private void MoveAnimation(GameObject obj, Vector3 newPos, float delay = 0f, bool disableOnComplete = false)
    {
        LeanTween.cancel(obj);
        LeanTween.move(obj, newPos, 0.5f)
            .setEaseOutExpo()
            .setDelay(delay)
            .setOnComplete(() => { obj.gameObject.SetActive(!disableOnComplete); })
            .setIgnoreTimeScale(true);
    }
}
