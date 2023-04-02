using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private bool entryExitAnimation;
    
    void Start()
    {
        foreach (var obj in objects) 
        {
            // Setup the initial positions when the scene starts
            obj.gameObject.transform.position = obj.posOut.transform.position;
            
            // Run all the Entry Animations
            MoveAnimation(obj.gameObject, obj.posIn.transform.position, obj.delay);
        }
    }

    public void MoveInAnimation() {
        foreach (var obj in objects) 
        {
            MoveAnimation(obj.gameObject, obj.posIn.transform.position, obj.delay);
        }
    }
    
    // Tween Move Animation
    private void MoveAnimation(GameObject obj, Vector3 newPos, float delay = 0f)
    {
        LeanTween.cancel(obj);
        LeanTween.move(obj, newPos, 0.5f).setEaseOutExpo().setDelay(delay);
    }
}
