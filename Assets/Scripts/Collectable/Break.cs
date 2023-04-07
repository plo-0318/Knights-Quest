using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    public GameObject potion;
    public int spawnChance;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnDestroy()
    {
        //Potion spawn chance is based on  1-spawnChance
        int random = UnityEngine.Random.Range(1, spawnChance);
        if (random == 1)
        {
            Debug.Log("Potion Spawn");
            Instantiate(potion, transform.position, Quaternion.identity);
        }
    }
}
