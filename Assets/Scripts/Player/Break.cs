using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    public GameObject potion;
    public int MaxValue;

    // Start is called before the first frame update
    void Start()
    {   
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy() {
        int random = UnityEngine.Random.Range(1, MaxValue);
        if (random == 1) {
            Debug.Log("Potion should go off");
            GameObject newPotion = Instantiate(potion, transform.position, Quaternion.identity);
            newPotion.SetActive(true);
        }
    }
   

}
